using GV.API;
using GV.General;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.AddServiceDefaults();
builder.Services.AddTransient<PlayOperations>();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

app.MapDefaultEndpoints();


app.MapApis();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
}
app.MapHub<StepsHub>("/stepsHub");
app.UseHttpsRedirection();

app.MapGet("/callStepsHub", async (IHubContext<StepsHub> hubContext) =>
{
    // Example of calling a method on the StepsHub
    PlayStep step = new("andrei", 0);

    await hubContext.Clients.All.SendAsync("SendNextStep", step);
    return Results.Ok("Message sent to StepsHub");
});

app.Run();

