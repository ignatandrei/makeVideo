
using GeneratorVideo;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;

Console.WriteLine("Hello, World!");
var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
GVApiSettings gVApiSettings = new();
builder.Configuration.GetSection("Services:gv-api").Bind(gVApiSettings);
var url = gVApiSettings.GetUrl();
Console.WriteLine(url);
builder.Services.AddTransient<IVideoApi, VideoApi>(sp =>
{
    return new VideoApi(url);
}

);




var app = builder.Build();


//var file = await File.ReadAllTextAsync("test.json");
var video=  app.Services.GetRequiredService<IVideoApi>();
var vj=await VideoJson.DeserializeFromFile("test.json",StepParser.Parse);
ArgumentNullException.ThrowIfNull(vj);
var id = await video.SendVideoJson(vj);
Console.WriteLine(vj.scriptName);
var data= await video.GetVideo(vj.scriptName);

VideoData vd = new(vj);
HubConnection _connection = new HubConnectionBuilder()
    .WithUrl(url + "/stepsHub")
    .Build();

var h = TypedSignalR.Client.HubConnectionExtensions.CreateHubProxy<IStepsHub>(_connection);
await h.Start(vj.scriptName);
await h.Start(id);


_connection.On<PlayStep>(nameof(IStepsHub.SendNextStep),async (step) =>
{
    Console.WriteLine("!!"+step);
    if (id == step.scriptName)
        await vd.ExecuteStep(0);
    else
        Console.WriteLine($"wrong script name {step.scriptName} for {id}");
    await vd.ExecuteStep(0);

});
_connection.On<string>(nameof(IStepsHub.Start), async (scriptName) =>
{
    Console.WriteLine("!!" + scriptName);
    if (id == scriptName)
        await vd.ExecuteStep(0);
    else
        Console.WriteLine($"wrong script name {scriptName} for {id}");
});

await _connection.StartAsync();


app.MapDefaultEndpoints();


//Console.WriteLine(gVApiSettings.https.Count);
//Console.WriteLine(gVApiSettings.https.First().Value);
//app.MapApis();
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenAPISwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
