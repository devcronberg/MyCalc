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

// Add top-level exception handler for unhandled exceptions
AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
{
    Log.Fatal(e.ExceptionObject as Exception, "An unhandled exception occurred");
    Log.CloseAndFlush();
};

TaskScheduler.UnobservedTaskException += (sender, e) =>
{
    Log.Fatal(e.Exception, "An unobserved task exception occurred");
    e.SetObserved();
};

try
{
    Log.Information("MyCalc application starting");

    // Set culture to InvariantCulture for consistent decimal parsing worldwide
    // InvariantCulture ensures "." is always decimal separator (not "," in EU locales)
    // This guarantees 5.5 parses as five-and-a-half on Danish, German, French systems
    // InvariantCulture is better than en-US for calculators - pure number focus, no regional assumptions
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
    Log.Debug("Culture set to InvariantCulture for consistent decimal parsing");

    var operations = Operation.GetOperations();
    Log.Information("Discovered {OperationCount} operations across {CategoryCount} categories", 
        operations.Count,
        operations.GroupBy(op => op.CategoryName).Count());

    // Helper method to get decimal input that handles both '.' and ',' as decimal separators
    static decimal GetDecimalInput(string prompt)
    {
        while (true)
        {
            var input = AnsiConsole.Ask<string>(prompt);
            Log.Debug("User input received for prompt '{Prompt}': '{Input}'", prompt, input);

            // Replace comma with dot to handle Danish/European keyboard input
            // Danish users will type ',' on numeric keypad but we need '.' for InvariantCulture parsing
            var normalizedInput = input.Replace(',', '.');

            if (decimal.TryParse(normalizedInput, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
            {
                Log.Debug("Successfully parsed input '{Input}' as decimal value {Value}", input, result);
                return result;
            }

            Log.Warning("Invalid number format entered: '{Input}'", input);
            AnsiConsole.MarkupLine("[red]Invalid number format. Please try again.[/]");
        }
    }

    while (true)
    {
        try
        {
            // Group operations by category and sort by sort order
            var categories = operations
                .GroupBy(op => new { op.CategoryName, op.CategoryDescription, op.CategorySortOrder })
                .OrderBy(g => g.Key.CategorySortOrder)
                .ThenBy(g => g.Key.CategoryName)
                .ToList();

            Log.Debug("Displaying main menu with {CategoryCount} categories", categories.Count);

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

            Log.Information("User selected category: '{CategoryChoice}'", categoryChoice);

            // Handle exit
            if (categoryChoice == "Exit")
            {
                Log.Information("User chose to exit application");
                AnsiConsole.MarkupLine("[green]Goodbye![/]");
                break;
            }

            // Find selected category
            var selectedCategoryIndex = categoryChoices.IndexOf(categoryChoice);
            if (selectedCategoryIndex < categories.Count)
            {
                var selectedCategory = categories[selectedCategoryIndex];
                var categoryOperations = selectedCategory.ToList();
                
                Log.Information("Entered category '{CategoryName}' with {OperationCount} operations", 
                    selectedCategory.Key.CategoryName, categoryOperations.Count);

                // Show operations in selected category
                while (true)
                {
                    try
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

                        Log.Debug("User selected operation: '{OperationChoice}'", operationChoice);

                        // Handle back to categories
                        if (operationChoice == "← Back to Categories")
                        {
                            Log.Information("User navigated back to categories");
                            AnsiConsole.Clear();
                            break;
                        }

                        // Find selected operation
                        var selectedOperationIndex = operationChoices.IndexOf(operationChoice);
                        if (selectedOperationIndex < categoryOperations.Count)
                        {
                            var selectedOperation = categoryOperations[selectedOperationIndex];
                            Log.Information("Executing operation '{OperationName}' with {ParameterCount} parameters", 
                                selectedOperation.Name, selectedOperation.ParameterCount);

                            try
                            {
                                // Get parameters using the enhanced Operation system
                                var parameters = new List<decimal>();

                                // Use parameter descriptions from the attribute for user-friendly prompts
                                for (int i = 0; i < selectedOperation.ParameterCount; i++)
                                {
                                    var prompt = i < selectedOperation.ParameterDescriptions.Length
                                        ? selectedOperation.ParameterDescriptions[i]
                                        : $"Parameter {i + 1}";

                                    var param = GetDecimalInput($"{prompt}:");
                                    parameters.Add(param);
                                }

                                Log.Debug("Collected parameters for operation '{OperationName}': [{Parameters}]", 
                                    selectedOperation.Name, string.Join(", ", parameters));

                                // Execute operation using the new Execute method
                                var result = selectedOperation.Execute(parameters.ToArray());

                                Log.Information("Operation '{OperationName}' completed successfully with result: {Result}", 
                                    selectedOperation.Name, result);

                                // Display result
                                AnsiConsole.MarkupLine($"[green]Result: {result}[/]");
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Error executing operation '{OperationName}': {ErrorMessage}", 
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
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Error in operation menu loop: {ErrorMessage}", ex.Message);
                        AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
                        AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
                        Console.ReadKey(true);
                        AnsiConsole.Clear();
                        break; // Go back to main menu
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error in main menu loop: {ErrorMessage}", ex.Message);
            AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
            AnsiConsole.MarkupLine("[grey]Press any key to continue...[/]");
            Console.ReadKey(true);
            AnsiConsole.Clear();
        }
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fatal error during application startup: {ErrorMessage}", ex.Message);
    AnsiConsole.MarkupLine($"[red]Fatal error: {ex.Message}[/]");
    Environment.Exit(1);
}
finally
{
    Log.Information("MyCalc application shutting down");
    Log.CloseAndFlush();
}
