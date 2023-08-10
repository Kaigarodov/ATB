namespace Dal.Helpers.Attributes;

public class CustomTableNameAttribute : Attribute
{
    public string Name { get; }

    public CustomTableNameAttribute(string name)
    {
        Name = name;
    }
}