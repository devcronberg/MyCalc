# MyCalc Project - AI Coding Agent Instructions

This is a simple .NET CLI calculator application that discovers operations through attributes and reflection, and provides a command-line interface for users to perform calculations.

## Development Environment

- **IDE**: Visual Studio Code on Windows
- **Shell**: PowerShell (Windows) - Note: Use `;` instead of `&&` for command chaining
- **Framework**: .NET 8.0

## Project Architecture

This is a .NET 8.0 calculator solution with a layered architecture:
- **MyCalcCore**: Core business logic library containing arithmetic functions and operation discovery
- **MyCalcCli**: Console application with Spectre.Console interactive menu
- **MyCalcTests**: xUnit test project

### Key Structural Patterns

- **Operation Discovery**: Uses `[Discover]` attribute and reflection to automatically find operations
- **Namespace Organization**: Each project uses its own namespace (`MyCalcCore`, `MyCalcCli`, `MyCalcTests`)
- **Function Organization**: Mathematical functions in `MyCalcCore/Operations/` (e.g., `BasicArithmetic.cs`)
- **Decimal Precision**: Uses `decimal` type for all arithmetic operations for financial-grade precision
- **Attributes**: `MyCalcCore/Attributes/` contains `DiscoverAttribute` for operation registration

## Development Workflows

### Building and Testing
```powershell
# Build entire solution from root
dotnet build

# Run all tests
dotnet test

# Run the CLI application
dotnet run --project MyCalcCli

# Build specific project
dotnet build MyCalcCore/MyCalcCore.csproj
```

### Project Structure Rules
- Mathematical operations belong in `MyCalcCore/Operations/` 
- Each operation method should return `decimal` and be decorated with `[Discover]` attribute
- Interactive menu is automatically generated in `MyCalcCli/Program.cs` using Spectre.Console
- Test files should mirror structure: test `BasicArithmetic` in `MyCalcTests/BasicArithmeticTests.cs`

## Current Implementation Status

**Completed Features**:
- ✅ Project references properly configured (CLI→Core, Tests→Core)
- ✅ `BasicArithmetic.Add()` method with `[Discover("Add", "Adds two decimal numbers")]`
- ✅ Operation discovery system with reflection
- ✅ Interactive Spectre.Console menu system
- ✅ Comprehensive unit tests for Add operation in `BasicArithmeticTests.cs`

**Areas for Expansion**:
- `BasicArithmetic` class needs `Subtract()`, `Multiply()`, `Divide()` methods
- Additional operation classes (e.g., `AdvancedMath`, `Statistics`)

## Adding New Operations

1. Add method to appropriate class in `MyCalcCore/Operations/` with `[Discover]` attribute:
```csharp
[Discover("Subtract", "Subtracts second number from first")]
public decimal Subtract(decimal a, decimal b)
{
    return a - b;
}
```

2. Create corresponding tests in `MyCalcTests/` using descriptive names:
```csharp
[Fact]
public void Subtract_PositiveNumbers_ReturnsCorrectResult()
{
    // Arrange, Act, Assert
}
```

3. **No CLI changes needed** - operations are automatically discovered and added to menu

## Testing Conventions
- Uses xUnit framework with `[Fact]` attributes
- Test methods: `MethodName_Scenario_ExpectedResult` pattern
- Include edge cases: negative numbers, zero, very large numbers, decimal precision
- Test files named after the class being tested (e.g., `BasicArithmeticTests.cs`)

## Dependencies
- .NET 8.0 target framework
- Nullable reference types enabled
- Implicit usings enabled
- xUnit 2.5.3 for testing
- Spectre.Console 0.50.0 for interactive CLI
- Custom reflection-based operation discovery
