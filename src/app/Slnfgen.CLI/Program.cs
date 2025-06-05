using Cocona;
using Microsoft.Extensions.Logging;
using Slnfgen.Application.Module;
using Slnfgen.CLI.Commands;

var builder = CoconaApp.CreateBuilder();
builder.Logging.AddConsole();
builder.Services.AddApplication();
var app = builder.Build();

app.AddCommands<GenerateSolutionFiltersCommand>();

app.Run();