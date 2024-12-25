
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
var vj1 = await video.SendVideoJson(vj);
Console.WriteLine(vj.scriptName);
var data= await video.GetVideo(vj.scriptName);
Console.WriteLine(data.scriptName);
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

//app.Run();
