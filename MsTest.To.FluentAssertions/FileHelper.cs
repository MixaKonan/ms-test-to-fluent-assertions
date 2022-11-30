namespace MsTest.To.FluentAssertions;

public class FileHelper
{
    private const string NewFileSuffix = "_Fluent";
    private const string CsFileExtension = ".cs";
    private string Path { get; }
    private bool PathIsDirectory { get; }
    private string Directory { get; }

    public FileHelper(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            throw new ArgumentException("Path cannot be empty", nameof(path));
        }

        if (!System.IO.Directory.Exists(path) && !File.Exists(path))
        {
            throw new ArgumentException($"No directory {path} found", nameof(path));
        }

        Path = path;
        PathIsDirectory = System.IO.Path.GetExtension(path) == string.Empty;
        Directory = System.IO.Path.GetDirectoryName(path)!;
    }

    public List<string> GetFilePaths()
    {
        var filePaths = new List<string>();
        if (PathIsDirectory)
        {
            filePaths.AddRange(System.IO.Directory.GetFiles(Path).Where(fileName => fileName.EndsWith(CsFileExtension) && !fileName.Contains(NewFileSuffix)));
        }
        else
        {
            filePaths.Add(Path);
        }

        return filePaths;
    }

    public void SaveFile(string fileName, string fileContent)
    {
        File.WriteAllText(@$"{Directory}\{fileName}{NewFileSuffix}.cs", fileContent);
    }
}