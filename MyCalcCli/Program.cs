using System.Globalization;
using Microsoft.Extensions.Configuration;
using MyCalcCore.Operations;
using Serilog;
using Spectre.Console;

// Configure Serilog from appsettings.json
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

// Add top-level exception handler
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception occurred");
    Log.CloseAndFlush();
};

try
{
    Log.Information("MyCalc application starting up");

    // Set culture to InvariantCulture for consistent decimal parsing worldwide
    // InvariantCulture ensures "." is always decimal separator (not "," in EU locales)
    // This guarantees 5.5 parses as five-and-a-half on Danish, German, French systems
    // InvariantCulture is better than en-US for calculators - pure number focus, no regional assumptions
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    Log.Debug("Culture set to InvariantCulture for consistent decimal parsing");

    var operations = Operation.GetOperations();
    Log.Information("Discovered {OperationCount} operations", operations.Count);

// Helper method to get decimal input that handles both '.' and ',' as decimal separators
static decimal GetDecimalInput(string prompt)
{
    while (true)
    {
        var input = AnsiConsole.Ask<string>(prompt);

        // Replace comma with dot to handle Danish/European keyboard input
        // Danish users will type ',' on numeric keypad but we need '.' for InvariantCulture parsing
        var normalizedInput = input.Replace(',', '.');

        if (decimal.TryParse(normalizedInput, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
        {
            Log.Debug("Successfully parsed input '{Input}' as decimal {Value}", input, result);
            return result;
        }

        Log.Debug("Invalid number format for input '{Input}'", input);
        AnsiConsole.MarkupLine("[red]Invalid number format. Please try again.[/]");
    }
}

while (true)
{
    // Group operations by category and sort by sort order
    var categories = operations
        .GroupBy(op => new { op.CategoryName, op.CategoryDescription, op.CategorySortOrder })
        .OrderBy(g => g.Key.CategorySortOrder)
        .ThenBy(g => g.Key.CategoryName)
        .ToList();

    // Create category menu choices
    var categoryChoices = new List<string>();
    foreach (var category in categories)
    {
        var displayName = category.Key.CategoryDescription != null
            ? $"{category.Key.CategoryName} - {category.Key.CategoryDescription}"
            : category.Key.CategoryName;
        categoryChoices.Add(displayName);
    }
    categoryChoices.Add("Exit");

    // Display category menu
    var categoryChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("MyCalc - Choose a category:")
            .PageSize(10)
            .AddChoices(categoryChoices));

    // Handle exit
    if (categoryChoice == "Exit")
    {
        Log.Information("User requested exit");
        AnsiConsole.MarkupLine("[green]Goodbye![/]");
        break;
    }

    // Find selected category
    var selectedCategoryIndex = categoryChoices.IndexOf(categoryChoice);
    if (selectedCategoryIndex < categories.Count)
    {
        var selectedCategory = categories[selectedCategoryIndex];
        Log.Information("User selected category: {CategoryName}", selectedCategory.Key.CategoryName);
        var categoryOperations = selectedCategory.ToList();

        // Show operations in selected category
        while (true)
        {
            var operationChoices = new List<string>();

            // Add operations to submenu
            foreach (var operation in categoryOperations)
            {
                var displayName = operation.Description != null
                    ? $"{operation.Name} - {operation.Description}"
                    : operation.Name;
                operationChoices.Add(displayName);
            }

            // Add back option
            operationChoices.Add("← Back to Categories");

            // Display operation menu with breadcrumb
            AnsiConsole.MarkupLine($"[blue]MyCalc > {selectedCategory.Key.CategoryName}[/]");
            var operationChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Choose an operation from {selectedCategory.Key.CategoryName}:")
                    .PageSize(10)
                    .AddChoices(operationChoices));

            // Handle back to categories
            if (operationChoice == "← Back to Categories")
            {
                Log.Debug("User selected back to categories");
                AnsiConsole.Clear();
                break;
            }

            // Find selected operation
            var selectedOperationIndex = operationChoices.IndexOf(operationChoice);
            if (selectedOperationIndex < categoryOperations.Count)
            {
                var selectedOperation = categoryOperations[selectedOperationIndex];
                Log.Information("User selected operation: {OperationName} from category: {CategoryName}", 
                    selectedOperation.Name, selectedCategory.Key.CategoryName);

                try
                {
                    // Get parameters using the enhanced Operation system
                    var parameters = new List<decimal>();
                    Log.Debug("Operation {OperationName} requires {ParameterCount} parameters", 
                        selectedOperation.Name, selectedOperation.ParameterCount);

                    // Use parameter descriptions from the attribute for user-friendly prompts
                    for (int i = 0; i < selectedOperation.ParameterCount; i++)
                    {
                        var prompt = i < selectedOperation.ParameterDescriptions.Length
                            ? selectedOperation.ParameterDescriptions[i]
                            : $"Parameter {i + 1}";

                        var param = GetDecimalInput($"{prompt}:");
                        parameters.Add(param);
                    }

                    // Execute operation using the new Execute method
                    Log.Debug("Executing operation {OperationName} with parameters: [{Parameters}]", 
                        selectedOperation.Name, string.Join(", ", parameters));
                    
                    var result = selectedOperation.Execute(parameters.ToArray());
                    
                    Log.Information("Operation {OperationName} completed successfully. Result: {Result}", 
                        selectedOperation.Name, result);

                    // Display result
                    AnsiConsole.MarkupLine($"[green]Result: {result}[/]");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error executing operation {OperationName}: {ErrorMessage}", 
                        selectedOperation.Name, ex.Message);
                    AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
                }

                // Pause before showing menu again
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
                Console.ReadKey(true);
                AnsiConsole.Clear();
            }
        }
    }
}
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal error in main application loop: {ErrorMessage}", ex.Message);
    AnsiConsole.MarkupLine($"[red]Fatal error: {ex.Message}[/]");
}
finally
{
    Log.Information("MyCalc application shutting down");
    Log.CloseAndFlush();
}
