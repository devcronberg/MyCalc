using System.Reflection;
using MyCalcCore.Attributes;
using Serilog;

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
            Log.Debug("Executing operation '{OperationName}' with parameters [{Parameters}]", 
                Name, string.Join(", ", args));

            if (args.Length != ParameterCount)
            {
                var errorMessage = $"Method {Name} expects {ParameterCount} arguments, but {args.Length} were provided.";
                Log.Error("Parameter count mismatch for operation '{OperationName}': {ErrorMessage}", Name, errorMessage);
                throw new ArgumentException(errorMessage);
            }

            try
            {
                object? instance = null;
                if (!_method.IsStatic)
                {
                    // For non-static methods, create instance
                    instance = Activator.CreateInstance(_type);
                    Log.Debug("Created instance of type '{TypeName}' for operation '{OperationName}'", _type.Name, Name);
                }

                var result = (decimal)_method.Invoke(instance, args.Cast<object>().ToArray())!;
                Log.Debug("Operation '{OperationName}' completed with result: {Result}", Name, result);
                
                return result;
            }
            catch (Exception ex)
            {
                // Unwrap reflection exceptions to show the actual error
                if (ex is System.Reflection.TargetInvocationException tie && tie.InnerException != null)
                {
                    Log.Error(tie.InnerException, "Error executing operation '{OperationName}': {ErrorMessage}", 
                        Name, tie.InnerException.Message);
                    throw tie.InnerException;
                }
                
                Log.Error(ex, "Error executing operation '{OperationName}': {ErrorMessage}", Name, ex.Message);
                throw;
            }
        }

        public static List<Operation> GetOperations()
        {
            Log.Debug("Starting operation discovery from assembly");
            var operations = new List<Operation>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                var categoryAttribute = type.GetCustomAttribute<OperationCategoryAttribute>();
                var categoryName = categoryAttribute?.Name ?? "Uncategorized";
                var categoryDescription = categoryAttribute?.Description;
                var categorySortOrder = categoryAttribute?.SortOrder ?? 999;

                Log.Debug("Scanning type '{TypeName}' for operations in category '{CategoryName}'", 
                    type.Name, categoryName);

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
                            var operation = new Operation(
                                discoverAttribute.Name,
                                discoverAttribute.Description,
                                discoverAttribute.ParameterDescriptions,
                                parameters.Length,
                                categoryName,
                                categoryDescription,
                                categorySortOrder,
                                method,
                                type
                            );
                            
                            operations.Add(operation);
                            Log.Debug("Discovered operation '{OperationName}' in category '{CategoryName}' with {ParameterCount} parameters", 
                                operation.Name, categoryName, parameters.Length);
                        }
                        else
                        {
                            Log.Warning("Skipping method '{MethodName}' in type '{TypeName}' - invalid signature (must return decimal and take only decimal parameters)", 
                                method.Name, type.Name);
                        }
                    }
                }
            }

            Log.Information("Operation discovery completed - found {OperationCount} operations across {CategoryCount} categories", 
                operations.Count, operations.GroupBy(op => op.CategoryName).Count());

            return operations;
        }
    }
}
