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

List<Device> devices = [];
List<Recipe> recipes = [];
List<Job> jobs = [];

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapGet("/device", () =>
{
    return devices.ToArray();
});

app.MapPost("/device/register", (Device device) =>
{
    devices.Add(device);

    return Results.Created($"/device/{device.Id}", device);
});

app.MapPost("/device/assign-job", (Job job) =>
{
    // TODO: check whether device already has an active job.
    jobs.Add(job);

    return Results.Created($"/job/{job.Id}", job);
});

app.MapGet("/recipe", () =>
{
    return recipes.ToArray();
});

app.MapPost("/recipe", (Recipe recipe) =>
{
    recipes.Add(recipe);
    recipe.Id = recipes.Count;

    Console.WriteLine("Recipe received: ");

    return Results.Created($"/recipe/{recipe.Id}", recipe);
});

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
