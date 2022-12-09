namespace MsTest.To.FluentAssertions.FileHelper;

public class FileHelper
{
    private const string NewFileSuffix = "_Fluent";
    private const string CsFileExtension = ".cs";

    public List<string> FilePaths { get; }

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

        this.Path = path;
        this.PathIsDirectory = System.IO.Path.GetExtension(path) == string.Empty;
        this.Directory = System.IO.Path.GetDirectoryName(path)!;
        this.FilePaths = this.GetFilePaths();
    }

    private List<string> GetFilePaths()
    {
        var filePaths = new List<string>();
        if (this.PathIsDirectory)
        {
            filePaths.AddRange(System.IO.Directory.GetFiles(this.Path).Where(fileName => fileName.EndsWith(CsFileExtension) && !fileName.Contains(NewFileSuffix)));
        }
        else
        {
            filePaths.Add(this.Path);
        }

        return filePaths;
    }

    public void SaveFile(string fileName, string fileContent)
    {
        var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
        File.WriteAllText(@$"{this.Directory}\{fileNameWithoutExtension}{NewFileSuffix}{CsFileExtension}", fileContent);
    }
}