using System.Reflection;
using MyCalcCore.Attributes;

namespace MyCalcCore.Operations
{
    public class Operation
    {
        public string Name { get; }
        public string? Description { get; }
        public string[] ParameterDescriptions { get; }
        public int ParameterCount { get; }
        public string CategoryName { get; }
        public string? CategoryDescription { get; }
        public int CategorySortOrder { get; }
        private readonly MethodInfo _method;
        private readonly Type _type;

        public Operation(string name, string? description, string[] parameterDescriptions, int parameterCount,
            string categoryName, string? categoryDescription, int categorySortOrder, MethodInfo method, Type type)
        {
            Name = name;
            Description = description;
            ParameterDescriptions = parameterDescriptions;
            ParameterCount = parameterCount;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            CategorySortOrder = categorySortOrder;
            _method = method;
            _type = type;
        }

        public decimal Execute(decimal[] args)
        {
            if (args.Length != ParameterCount)
            {
                throw new ArgumentException($"Method {Name} expects {ParameterCount} arguments, but {args.Length} were provided.");
            }

            try
            {
                object? instance = null;
                if (!_method.IsStatic)
                {
                    // For non-static methods, create instance
                    instance = Activator.CreateInstance(_type);
                }

                return (decimal)_method.Invoke(instance, args.Cast<object>().ToArray())!;
            }
            catch (Exception ex)
            {
                // Unwrap reflection exceptions to show the actual error
                if (ex is System.Reflection.TargetInvocationException tie && tie.InnerException != null)
                {
                    throw tie.InnerException;
                }
                throw;
            }
        }

        public static List<Operation> GetOperations()
        {
            var operations = new List<Operation>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                var categoryAttribute = type.GetCustomAttribute<OperationCategoryAttribute>();
                var categoryName = categoryAttribute?.Name ?? "Uncategorized";
                var categoryDescription = categoryAttribute?.Description;
                var categorySortOrder = categoryAttribute?.SortOrder ?? 999;

                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                {
                    var discoverAttribute = method.GetCustomAttribute<DiscoverAttribute>();
                    if (discoverAttribute != null)
                    {
                        var parameters = method.GetParameters();

                        // Only support methods that take decimal parameters and return decimal
                        if (method.ReturnType == typeof(decimal) &&
                            parameters.All(p => p.ParameterType == typeof(decimal)))
                        {
                            operations.Add(new Operation(
                                discoverAttribute.Name,
                                discoverAttribute.Description,
                                discoverAttribute.ParameterDescriptions,
                                parameters.Length,
                                categoryName,
                                categoryDescription,
                                categorySortOrder,
                                method,
                                type
                            ));
                        }
                    }
                }
            }

            return operations;
        }
    }
}
