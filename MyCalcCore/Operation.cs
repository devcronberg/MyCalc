using System.Reflection;
using MyCalcCore.Attributes;

namespace MyCalcCore
{
    public class Operation
    {
        public string Name { get; }
        public string? Description { get; }
        public Func<decimal[], decimal> Delegate { get; }

        public Operation(string name, string? description, Func<decimal[], decimal> @delegate)
        {
            Name = name;
            Description = description;
            Delegate = @delegate;
        }

        public static IList<Operation> GetOperations()
        {
            IList<Operation> operations = new List<Operation>();
            var assembly = Assembly.GetExecutingAssembly();

            foreach (var type in assembly.GetTypes())
            {
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                {
                    var discoverAttribute = method.GetCustomAttribute<DiscoverAttribute>();
                    if (discoverAttribute != null)
                    {
                        var @delegate = CreateDelegate(method, type);
                        if (@delegate != null)
                        {
                            operations.Add(new Operation(
                                discoverAttribute.Name,
                                discoverAttribute.Description,
                                @delegate
                            ));
                        }
                    }
                }
            }

            return operations;
        }

        private static Func<decimal[], decimal>? CreateDelegate(MethodInfo method, Type type)
        {
            var parameters = method.GetParameters();

            // Only support methods that take decimal parameters and return decimal
            if (method.ReturnType != typeof(decimal) ||
                !parameters.All(p => p.ParameterType == typeof(decimal)))
            {
                return null;
            }

            return (args) =>
            {
                if (args.Length != parameters.Length)
                {
                    throw new ArgumentException($"Method {method.Name} expects {parameters.Length} arguments, but {args.Length} were provided.", nameof(args));
                }

                object? instance = null;
                if (!method.IsStatic)
                {
                    instance = Activator.CreateInstance(type);
                }

                return (decimal)method.Invoke(instance, args.Cast<object>().ToArray())!;
            };
        }
    }
}
