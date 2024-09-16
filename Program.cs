using Practica02.Datos.Interfaces;
using Practica02.Datos.Repositorios;
using Practica02.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IArticulo, ArticuloRepositorio>(); //  A ESTO LO AGREGUE YO
builder.Services.AddScoped<ArticuloServicio>(); // A ESTO LO AGREGUE YO 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
