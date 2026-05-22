using SnapForge.Cli.Commands;
using Spectre.Console;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("snapforge");

    config.AddCommand<CardCommand>("card")
        .WithDescription("Generate a polished image card from a screenshot.")
        .WithExample([
            "card",
            "./examples/input/api-screen.png",
            "--output",
            "./examples/output/api-card.png",
            "--title",
            "GrowthOS API",
            "--subtitle",
            "ASP.NET Core / PostgreSQL / Docker",
            "--preset",
            "github",
            "--theme",
            "dark"
        ]);

    config.AddCommand<BatchCommand>("batch")
        .WithDescription("Generate multiple image cards from a batch JSON config.")
        .WithExample([
            "batch",
            "./examples/snapforge.batch.json"
        ]);
});

try
{
    return app.Run(args);
}
catch (Exception exception)
{
    AnsiConsole.MarkupLine("[red]SnapForge failed:[/]");
    AnsiConsole.MarkupLine($"[grey]{Markup.Escape(exception.Message)}[/]");

    return 1;
}
