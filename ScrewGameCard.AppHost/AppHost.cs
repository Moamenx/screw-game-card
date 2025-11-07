var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ScrewGameCard_HttpApi_Host>("screwgamecard-httpapi-host");

builder.Build().Run();
