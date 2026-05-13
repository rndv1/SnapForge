using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SnapForge.Cli.Commands;

public sealed class CardCommand : Command<CardCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<input>")]
        [Description("Path to the source screenshot.")]
        public string? Input { get; set; }

        [CommandOption("-o|--output <output>")]
        [Description("Path where the generated PNG card will be written.")]
        public string? Output { get; set; }

        [CommandOption("--title <title>")]
        [Description("Title rendered on the card.")]
        public string? Title { get; set; }

        [CommandOption("--subtitle <subtitle>")]
        [Description("Subtitle rendered below the title.")]
        public string? Subtitle { get; set; }

        [CommandOption("--preset <preset>")]
        [Description("Card preset: github, social, or portfolio.")]
        public string? Preset { get; set; }

        [CommandOption("--theme <theme>")]
        [Description("Card theme: light or dark.")]
        public string? Theme { get; set; }
    }

    protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine("[yellow]SnapForge card rendering is not wired yet.[/]");
        AnsiConsole.MarkupLine("[grey]This command will be connected to the renderer in a later PR.[/]");

        return 0;
    }
}
