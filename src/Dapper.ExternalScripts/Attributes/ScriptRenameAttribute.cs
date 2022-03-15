namespace Dapper.ExternalScripts.Attributes;

/// <summary>
///     Allows the renaming of filenames and extensions per method
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ScriptRenameAttribute : Attribute
{
    public string FileName { get; }
    public string FileExtension { get; }
    public ScriptRenameAttribute(string fileName, string fileExtension = "sql")
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException(nameof(fileName));

        if (string.IsNullOrEmpty(fileExtension))
            throw new ArgumentException(nameof(fileExtension));

        var fileNameChars = fileName.Trim().ToCharArray();

        if (fileNameChars.Any(c => char.IsWhiteSpace(c)))
            throw new InvalidOperationException($"{nameof(fileName)} can't contain spaces.");

        if (fileNameChars.Any(c => c == '/' || c == '\\'))
            throw new InvalidOperationException($"{nameof(fileName)} can't contain any '\' or '/' characters");


        var fileExtensionChars = fileExtension.Trim().ToCharArray();
        if (fileExtensionChars.Any(c => char.IsWhiteSpace(c)))
            throw new InvalidOperationException($"{nameof(fileExtension)} can't contain spaces.");
        if (fileExtensionChars.Any(c => c == '/' || c == '\\' || c == '.'))
            throw new InvalidOperationException($"{nameof(fileExtension)} can't contain any '\\' or '/' or '.' characters");

        this.FileName = fileName;
        this.FileExtension = fileExtension;
    }
}
