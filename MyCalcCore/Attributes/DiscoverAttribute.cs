namespace MyCalcCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DiscoverAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; }
        public string[] ParameterDescriptions { get; }

        public DiscoverAttribute(string name, string? description = null, params string[] parameterDescriptions)
        {
            Name = name;
            Description = description;
            ParameterDescriptions = parameterDescriptions ?? Array.Empty<string>();
        }
    }
}
