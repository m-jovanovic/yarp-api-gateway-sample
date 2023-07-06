using Bogus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var faker = new Faker();
var products = Enumerable.Range(1, 100).Select(num => new { Id = num, Name = faker.Commerce.ProductName() }).ToArray();

app.MapGet("/products", (string? filter) => products.Where(p => string.IsNullOrWhiteSpace(filter) || p.Name.Contains(filter)));

app.MapGet("/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);

    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.UseHttpsRedirection();

app.Run();
