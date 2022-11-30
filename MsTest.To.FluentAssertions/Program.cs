using System.Text;
using MsTest.To.FluentAssertions;
using Spectre.Console;

var path = args[0];

var fileHelper = new FileHelper(path);
var filePaths = fileHelper.GetFilePaths();

var stringBuilder = new StringBuilder();
var status = AnsiConsole.Status().Spinner(Spinner.Known.Pipe);

status.Start("Processing files",
    context =>
    {
        foreach (var filePath in filePaths)
        {
            var fileName = Path.GetFileName(filePath);
            context.Status($"[red]Processing {fileName}[/]");

            var fileText = File.ReadAllText(filePath);
            var indexesOfAssertions = fileText.AllIndexesOf("Assert.");

            for (var i = 0; i < fileText.Length;)
            {
                if (indexesOfAssertions.Contains(i))
                {
                    var assertion = new string(fileText[i..].TakeWhile(c => c != ';').ToArray());
                    var fluentAssertion = FrameworksConverter.ToFluentAssertion(assertion);
                    stringBuilder.Append(fluentAssertion);
                    i += assertion.Length + 1 /* semicolon */;
                    continue;
                }

                stringBuilder.Append(fileText[i]);
                i++;
            }

            fileHelper.SaveFile(fileName, stringBuilder.ToString());
            stringBuilder.Clear();
        }

        context.Status = "Finished processing files";
    });

AnsiConsole.Markup("[green]Finished processing files[/]");