global using Dependous;
global using Dependous.Autofac;
global using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutoFacContainer(AssemblyPaths.From("WorkerExample", "ExamplesCommon"),
    configurationBuilder: (c) => c.PersistScanResults = true, logger: (e) => Console.WriteLine($"{e}"));
// Add services to the container.
builder.Services.AddDependencyScanning();
builder.Services.AddControllers();
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
