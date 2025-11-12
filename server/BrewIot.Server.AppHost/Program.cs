var builder = DistributedApplication.CreateBuilder(args);

var sqlite = builder.AddSqlite("my-database");

var apiService = builder.AddProject<Projects.BrewIoT_Server_ApiService>("apiservice")
    .WithReference(sqlite);
    //.WaitFor(beerDb);

builder.AddProject<Projects.BrewIoT_Server_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);
    //.WaitFor(apiService);

builder.Build().Run();
