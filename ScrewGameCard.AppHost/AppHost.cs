var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.ScrewGameCard_HttpApi_Host>("screwgamecard-httpapi-host");

var portal = builder.AddNpmApp("screwgamecard-portal", "../ScrewGameCard.Portal", "start")
.WithHttpEndpoint(port:54435)
.WithUrl("http://localhost:54435/")
.WithReference(api)
.WaitFor(api);

builder.Build().Run();
