using MsTest.To.FluentAssertions.FileHelper;
using MsTest.To.FluentAssertions.SyntaxParsing;
using Spectre.Console;

var path = args[0];

var fileHelper = new FileHelper(path);

var status = AnsiConsole.Status().Spinner(Spinner.Known.Pipe);
status.Start("Processing files",
    context =>
    {
        foreach (var filePath in fileHelper.FilePaths)
        {
            var fileName = Path.GetFileName(filePath);
            context.Status($"[red]Processing {fileName}[/]");

            var fileText = File.ReadAllText(filePath);
            var hadMsTestAssertions = CSharpMsTestToFluentConverter.TryConvert(fileText, out var newFileText);

            if (hadMsTestAssertions)
            {
                fileHelper.SaveFile(fileName, newFileText);
            }
        }

        context.Status = "Finished processing files";
    });

AnsiConsole.Markup("[green]Finished processing files[/]");