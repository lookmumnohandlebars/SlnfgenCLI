using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Slnfgen.CLI.Presentation.StartUp;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddAllDependencies();
builder.Logging.AddConsole();

var app = builder.Build();

app.AddAllMiddleware(app.Services.GetRequiredService<ILogger>());
app.AddAllCommands();
app.Run();
