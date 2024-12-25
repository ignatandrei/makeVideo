var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.GV_API>("gv-api");

builder.Build().Run();
