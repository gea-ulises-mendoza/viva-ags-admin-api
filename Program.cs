using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VivaAguascalientesAPI;

var builder = WebApplication.CreateBuilder(args);

// Configure services
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
var env = app.Environment;
var provider = app.Services.GetRequiredService<Microsoft.AspNetCore.Mvc.ApiExplorer.IApiVersionDescriptionProvider>();
startup.Configure(app, env, provider);

app.Run();
