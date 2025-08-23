namespace MyCalcCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class OperationCategoryAttribute : Attribute
    {
        public string Name { get; }
        public string? Description { get; }
        public int SortOrder { get; }

        public OperationCategoryAttribute(string name, string? description = null, int sortOrder = 0)
        {
            Name = name;
            Description = description;
            SortOrder = sortOrder;
        }
    }
}
