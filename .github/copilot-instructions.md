# MyCalc Project - AI Coding Agent Instructions

This is a complete .NET CLI calculator application with attribute-driven operation discovery, categorized interactive menus, comprehensive testing, and automated cross-platform deployment via GitHub Actions.

## Development Environment

- **IDE**: Visual Studio Code on Windows
- **Shell**: PowerShell (Windows) - Note: Use `;` instead of `&&` for command chaining
- **Framework**: .NET 8.0
- **CI/CD**: GitHub Actions with cross-platform matrix builds
- **Quality Gates**: All 42+ tests must pass before builds are created

## Project Architecture

This is a production-ready .NET 8.0 calculator solution with a layered architecture:
- **MyCalcCore**: Core business logic library with operation discovery engine
- **MyCalcCli**: Console application with Spectre.Console categorized menu system
- **MyCalcTests**: Comprehensive xUnit test suite (42+ tests)
- **.github/workflows**: Cross-platform CI/CD pipeline

### Key Structural Patterns

- **Operation Discovery**: Uses `[Discover]` attribute with parameter descriptions and reflection-based execution
- **Operation Categories**: Uses `[OperationCategory]` attribute for menu organization and sorting
- **Namespace Organization**: Each project uses its own namespace (`MyCalcCore`, `MyCalcCli`, `MyCalcTests`)
- **Function Organization**: Mathematical functions in `MyCalcCore/Operations/` with category-based grouping
- **Decimal Precision**: Uses `decimal` type with InvariantCulture for international compatibility
- **Attributes System**: `MyCalcCore/Attributes/` contains discovery and categorization attributes
- **Two-Level Menu**: Category selection → Operation selection → Parameter input
- **International Input**: Handles both `.` and `,` decimal separators automatically
- **Structured Logging**: Serilog-based logging with configurable console output and global exception handling

## Development Workflows

### Building and Testing
```powershell
# Build entire solution from root
dotnet build

# Run all 42+ tests (required before deployment)
dotnet test

# Run the CLI application
dotnet run --project MyCalcCli

# Quick start script
.\run.ps1

# Build specific project
dotnet build MyCalcCore/MyCalcCore.csproj
```

### Cross-Platform Deployment
```powershell
# GitHub Actions automatically builds for:
# - win-x64 (Windows executable + PowerShell launcher)
# - linux-x64 (Linux executable + Bash launcher)  
# - osx-x64 (macOS executable + Bash launcher)

# Quality gate: All tests must pass before any builds are created
# Artifacts include self-contained executables and documentation
```

### Project Structure Rules
- Mathematical operations belong in `MyCalcCore/Operations/` 
- Each operation method should return `decimal` and be decorated with `[Discover]` attribute
- Interactive menu is automatically generated in `MyCalcCli/Program.cs` using Spectre.Console
- Test files should mirror structure: test `BasicArithmetic` in `MyCalcTests/BasicArithmeticTests.cs`

## Current Implementation Status

**Completed Features**:
- ✅ Project references properly configured (CLI→Core, Tests→Core)
- ✅ `BasicArithmetic.Add()` and `Subtract()` methods with `[Discover]` attributes
- ✅ `AdvancedArithmetic.Square()` method demonstrating category system
- ✅ Operation discovery system with reflection-based parameter handling
- ✅ Interactive Spectre.Console categorized menu system with breadcrumbs
- ✅ Comprehensive unit tests (42+ tests) with Theory/InlineData patterns
- ✅ International decimal input handling (supports both `.` and `,`)
- ✅ Cross-platform GitHub Actions workflow with quality gates
- ✅ Self-contained executable builds for Windows, Linux, and macOS
- ✅ Automated artifact creation with platform-specific launchers

**Current Operation Inventory**:
- `BasicArithmetic` class: Add(), Subtract() [Category: "Basic Arithmetic", Priority: 1]
- `AdvancedArithmetic` class: Square() [Category: "Advanced Math", Priority: 2]

**Areas for Expansion**:
- Additional methods in existing classes: Multiply(), Divide(), etc.
- New operation categories: Statistics, Trigonometry, etc.

## Adding New Operations

1. Add method to appropriate class in `MyCalcCore/Operations/` with `[Discover]` attribute:
```csharp
[Discover("Multiply", "Multiplies two numbers", "First number", "Second number")]
public decimal Multiply(decimal a, decimal b)
{
    return a * b;
}
```

2. Create corresponding tests in `MyCalcTests/` using descriptive names:
```csharp
[Theory]
[InlineData(2, 3, 6)]
[InlineData(5.5, 2, 11)]
public void Multiply_TwoNumbers_ReturnsProduct(decimal a, decimal b, decimal expected)
{
    // Arrange, Act, Assert
}
```

3. **No CLI changes needed** - operations are automatically discovered and added to categorized menu

## Adding New Operation Categories

1. Create new class in `MyCalcCore/Operations/` with category attribute:
```csharp
[OperationCategory("Statistics", "Statistical operations", 3)]
public class Statistics
{
    [Discover("Average", "Calculates average of two numbers", "First number", "Second number")]
    public decimal Average(decimal a, decimal b) => (a + b) / 2;
}
```

2. Category automatically appears in main menu with proper sorting

## Testing Conventions
- Uses xUnit framework with `[Fact]` and `[Theory]` attributes
- Test methods: `MethodName_Scenario_ExpectedResult` pattern
- Parameterized tests using `[Theory]` with `[InlineData]` for multiple test cases
- Include edge cases: negative numbers, zero, very large numbers, decimal precision
- Test files named after the class being tested (e.g., `BasicArithmeticTests.cs`)
- Current test count: 42+ comprehensive tests covering all operations

## Dependencies
- .NET 8.0 target framework
- Nullable reference types enabled
- Implicit usings enabled
- xUnit 2.5.3 for testing with Theory/InlineData support
- Spectre.Console 0.50.0 for interactive CLI menus
- Serilog 4.1.0 for structured logging with console sink
- Microsoft.Extensions.Configuration.Json for appsettings.json support
- Custom reflection-based operation discovery system
- GitHub Actions for automated cross-platform builds

## Logging System

MyCalc implements comprehensive structured logging using Serilog:

### Configuration
- **Location**: `appsettings.json` in both CLI and Core projects
- **Format**: `[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}`
- **Default Level**: Information with Debug available for troubleshooting
- **Sink**: Console only (no files or remote endpoints)

### Usage Guidelines
- **Information**: Normal application flow (startup, operation selections, results)
- **Debug**: Detailed troubleshooting (parameter parsing, configuration loading)
- **Warning**: Recoverable issues (API retries, invalid input)
- **Error**: Operation failures with full exception details
- **Fatal**: Application-level crashes with stack traces

### Logging Points
- **CLI**: Application lifecycle, user interactions, operation results, errors
- **Core**: Operation execution, API calls, configuration loading, retry logic
- **Global**: Unhandled exceptions caught and logged before shutdown

### Customization
```json
{
    "Serilog": {
        "MinimumLevel": {
            "Default": "Information",
            "Override": {
                "MyCalcCore": "Debug"  // Enable debug for Core operations
            }
        }
    }
}
```

## CI/CD Pipeline Details
- **Trigger**: Push to main branch or pull requests
- **Quality Gates**: All 42+ tests must pass before builds are created
- **Build Matrix**: win-x64, linux-x64, osx-x64 
- **Artifacts**: Self-contained executables with launcher scripts
- **Testing**: Comprehensive validation on every commit
- **Deployment**: Automatic artifact creation for successful builds
