namespace Dapper.ExternalScripts.Attributes;

/// <summary>
///     Allows the renaming of filenames and extensions per method
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class DapperRenameAttribute : Attribute
{
    public string FileName { get; }
    public string FileExtension { get; }
    public DapperRenameAttribute(string fileName, string fileExtension = "sql")
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException(nameof(fileName));

        if (string.IsNullOrEmpty(fileExtension))
            throw new ArgumentException(nameof(fileExtension));

        if (fileName.Trim().ToCharArray().Any(c => char.IsWhiteSpace(c)))
            throw new InvalidOperationException($"route can't contain spaces.");

        this.FileName = fileName;
        this.FileExtension = fileExtension;
    }
}
