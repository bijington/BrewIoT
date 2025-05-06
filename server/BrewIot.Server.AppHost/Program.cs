var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL container is configured with an auto-generated password by default
// and supports setting the default database name via an environment variable & running *.sql/*.sh scripts in a bind mount.
var beerDbName = "beer";

var postgres = builder.AddPostgres("postgres")
    // Set the name of the default database to auto-create on container startup.
    .WithEnvironment("POSTGRES_DB", beerDbName)
    // Mount the SQL scripts directory into the container so that the init scripts run.
    //.WithBindMount("../DatabaseContainers.ApiService/data/postgres", "/docker-entrypoint-initdb.d")
    // Configure the container to store data in a volume so that it persists across instances.
    .WithDataVolume()
    .WithPgWeb()
    // Keep the container running between app host sessions.
    .WithLifetime(ContainerLifetime.Persistent);

// Add the default database to the application model so that it can be referenced by other resources.
var beerDb = postgres.AddDatabase(beerDbName);

var apiService = builder.AddProject<Projects.BrewIoT_Server_ApiService>("apiservice")
    .WithReference(beerDb);
    //.WaitFor(beerDb);

builder.AddProject<Projects.BrewIoT_Server_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);
    //.WaitFor(apiService);

builder.Build().Run();
