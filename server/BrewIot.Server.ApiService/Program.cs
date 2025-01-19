using BrewIoT.Server.Data.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.AddNpgsqlDbContext<DeviceContext>("beer");

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add controllers services
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

List<Recipe> _recipes = [];

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/device", () =>
{
    using (var scope = app.Services.CreateScope())
    {
        //var context = scope.ServiceProvider.GetRequiredService<DeviceContext>();
        //context.Database.EnsureCreated();

        
    }

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new Device
        {
            Id = index,
            Name = $"Device {index}"
        })
        .ToArray();
    return forecast;
});

app.MapPost("/device", (Device device) =>
{
    using (var scope = app.Services.CreateScope())
    {
        //var context = scope.ServiceProvider.GetRequiredService<DeviceContext>();
        //context.Devices.Add(device);
        //context.SaveChanges();
    }

    return Results.Created($"/device/{device.Id}", device);
});

app.MapGet("/recipe", () =>
{
    return _recipes.ToArray();
});

app.MapPost("/recipe", (Recipe recipe) =>
{
    _recipes.Add(recipe);
    recipe.Id = _recipes.Count;

    Console.WriteLine("Recipe received: ");

    return Results.Created($"/recipe/{recipe.Id}", recipe);
});

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
