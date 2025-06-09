using Cocona;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Presentation.StartUp;

var builder = CoconaApp.CreateBuilder();
builder.Logging.AddConsole();
builder.Services.AddAllDependencies();
var app = builder.Build();

app.AddAllMiddleware();
app.AddAllCommands();

app.Run();
