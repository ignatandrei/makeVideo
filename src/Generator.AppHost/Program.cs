var builder = DistributedApplication.CreateBuilder(args);

var prjAPI = builder
        .AddProject<Projects.GV_API>("gv-api")
        .WithExternalHttpEndpoints()
    ;
builder
    .AddProject<Projects.GV_Execute>("gv-execute")
    .WithReference(prjAPI)
    .WaitFor(prjAPI);

builder.Build().Run();
