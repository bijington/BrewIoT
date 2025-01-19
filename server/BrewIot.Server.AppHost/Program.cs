var builder = DistributedApplication.CreateBuilder(args);

var connection = //builder.AddPostgres("beer").WithPgAdmin().AddDatabase("beerdb");//.AddConnectionString("beer");
    builder.AddConnectionString("beer");

var apiService = builder.AddProject<Projects.BrewIoT_Server_ApiService>("apiservice")
    .WithReference(connection);

builder.AddProject<Projects.BrewIoT_Server_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
