using Dependous;
using Dependous.Attributes;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseUnityContainer(AssemblyPaths.From("WorkerExample", "ExamplesCommon"),
    configurationBuilder: (c) =>
    {
        c.PersistScanResults = true;
        c.AddAdditionalDiscoveryTypes(x => x.RegisterAttribute<DiComponentAttribute>((d) =>
        {
            return new DiComponentAttribute(d.ResolveType, d.LifeTime, d.EnumerationOnly);
        }));
    }, logger: (e) => Console.WriteLine($"{e}"));
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