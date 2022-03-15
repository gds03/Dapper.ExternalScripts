namespace Dapper.ExternalScripts.Tests.ModelsToTest;
public class NoMethods
{
}
public class OneMethod
{
    public virtual string? GetAll() { return null; }
}
public class TwoMethods
{
    public virtual string? GetAll() { return null; }

    public virtual string? GetSingle() { return null; }
}

public class ThreeMethods
{
    public virtual string? GetAll() { return null; }
    public virtual string? GetAllByText(string text) { return null; }
    public virtual string? GetSingle() { return null; }
}

public class FourMethods
{
    public virtual string? GetAll() { return null; }
    public virtual string? GetAll(string text) { return null; }
    public virtual string? GetAllByText(string text) { return null; }
    public virtual string? GetSingle() { return null; }
}