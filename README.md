# MyCalc - .NET Calculator with Operation Discovery

A modern .NET 8.0 console calculator application that automatically discovers mathematical operations through attributes and reflection, featuring an interactive menu system with categorized operations and cross-platform deployment automation.

## Features

- 🔍 **Automatic Operation Discovery** - Operations are automatically found using `[Discover]` attributes
- 📂 **Categorized Menu System** - Operations organized into logical categories with submenus
- 🌍 **International Support** - Handles both `.` and `,` as decimal separators for different locales
- 🎯 **Type Safety** - Uses `decimal` for financial-grade precision
- 🖥️ **Beautiful CLI** - Interactive menus powered by Spectre.Console
- 📋 **Structured Logging** - Comprehensive Serilog-based logging with configurable levels and output formats
- ✅ **Comprehensive Tests** - Full test coverage with xUnit (42+ tests)
- 🏗️ **Clean Architecture** - Layered design with Core/CLI/Tests separation
- 🚀 **Cross-Platform Builds** - Automated builds for Windows, Linux, and macOS via GitHub Actions
- 📦 **Release Automation** - Automatic artifact creation with platform-specific executables

## Logging

MyCalc includes comprehensive structured logging using Serilog:

- **Console Output**: All logs are written to console with timestamp, level, and structured data
- **Configurable Levels**: Information (default) and Debug levels available
- **Log Configuration**: Fully configurable via `appsettings.json`
- **Operation Tracking**: All calculations, user actions, and errors are logged
- **Error Handling**: Structured error logging with full exception details

### Log Output Format

```
[HH:mm:ss INF] Message with {StructuredData} {}
[HH:mm:ss ERR] Error occurred: {ErrorMessage} {}
System.Exception: Full exception details with stack trace
```

### Configuring Logging

Edit `MyCalcCli/appsettings.json` to customize logging behavior:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

**Available Log Levels**: Debug, Information, Warning, Error, Fatal

## Getting Started

### Quick Start (Development)

1. Clone the repository
2. Run the calculator:
   ```powershell
   .\run.ps1
   ```

### Download Pre-built Releases

1. Go to the [Releases](https://github.com/devcronberg/MyCalc/releases) page
2. Download the appropriate package for your platform:
   - `MyCalc-win-x64.zip` for Windows
   - `MyCalc-linux-x64.tar.gz` for Linux
   - `MyCalc-osx-x64.tar.gz` for macOS
3. Extract and run:
   - **Windows**: Double-click `Start-MyCalc.ps1` or run `MyCalcCli.exe`
   - **Linux/macOS**: Run `./Start-MyCalc.sh` or `./MyCalcCli`

### Prerequisites (for development)

- .NET 8.0 SDK
- Windows with PowerShell (primary development environment)

### Quick Start

1. Clone the repository
2. Run the calculator:
   ```powershell
   .\run.ps1
   ```

### Manual Build & Run

```powershell
dotnet build --configuration Release
dotnet run --project MyCalcCli --configuration Release
```

### Running Tests

```powershell
dotnet test
```

## Project Structure

```
MyCalc/
├── MyCalcCore/              # Core business logic
│   ├── Attributes/          # Custom attributes for operation discovery
│   │   ├── DiscoverAttribute.cs
│   │   └── OperationCategoryAttribute.cs
│   └── Operations/          # Mathematical operation classes
│       ├── Operation.cs     # Operation discovery and execution engine
│       ├── BasicArithmetic.cs
│       └── AdvancedArithmetic.cs
├── MyCalcCli/              # Console application with interactive menu
│   └── Program.cs
├── MyCalcTests/            # Unit tests (42+ comprehensive tests)
│   ├── BasicArithmeticTests.cs
│   └── AdvancedArithmeticTests.cs
├── .github/                # GitHub Actions workflow
│   └── workflows/
│       └── build-and-release.yml  # Cross-platform CI/CD
├── run.ps1                 # Quick start script
└── README.md
```

## CI/CD Pipeline

The project includes a comprehensive GitHub Actions workflow that:

- ✅ **Quality Gates** - All 42+ tests must pass before any builds are created
- 🔨 **Cross-Platform Builds** - Automatically builds for Windows, Linux, and macOS
- 📦 **Artifact Creation** - Creates platform-specific packages with:
  - Self-contained executables (no .NET runtime required)
  - Platform-specific launcher scripts (`Start-MyCalc.ps1`/`.sh`)
  - Documentation and dependencies
- 🚀 **Automatic Releases** - Triggered on pushes to main branch

## How It Works

### Operation Discovery

Operations are automatically discovered using reflection and custom attributes:

```csharp
[OperationCategory("Basic Arithmetic", "Fundamental mathematical operations", 1)]
public class BasicArithmetic
{
    [Discover("Add", "Adds two decimal numbers", "First number", "Second number")]
    public decimal Add(decimal a, decimal b)
    {
        return a + b;
    }
}
```

### Menu System

The CLI automatically generates:
1. **Category menu** - Shows operation categories (Basic Arithmetic, Advanced Math)
2. **Operation menu** - Shows operations within selected category
3. **Parameter prompts** - Uses descriptions from attributes for user-friendly input

## Adding New Operations

### 1. Create Operation Method

```csharp
[Discover("Multiply", "Multiplies two numbers", "First number", "Second number")]
public decimal Multiply(decimal a, decimal b)
{
    return a * b;
}
```

### 2. Add Tests

```csharp
[Theory]
[InlineData(2.0, 3.0, 6.0)]
[InlineData(5.5, 2.0, 11.0)]
public void Multiply_TwoNumbers_ReturnsProduct(decimal a, decimal b, decimal expected)
{
    // Act
    decimal result = _arithmetic.Multiply(a, b);
    
    // Assert
    Assert.Equal(expected, result);
}
```

### 3. Run

The operation automatically appears in the menu - no CLI changes needed!

## Current Operations

### Basic Arithmetic (Category 1)
- **Add** - Adds two decimal numbers
- **Subtract** - Subtracts second number from first

### Advanced Math (Category 2)
- **Square** - Calculates the square of a number

*Operations are automatically sorted by category priority and displayed in interactive submenus*

## Build & Deployment

### Local Development
```powershell
# Build entire solution
dotnet build --configuration Release

# Run all 42+ tests
dotnet test

# Run the application
dotnet run --project MyCalcCli --configuration Release
```

### Cross-Platform Release Builds
The GitHub Actions workflow automatically creates platform-specific builds:

```yaml
# Builds for: win-x64, linux-x64, osx-x64
# Includes: Self-contained executable, launcher scripts, documentation
# Quality gate: All tests must pass before builds are created
```

## Technical Details

- **Framework**: .NET 8.0
- **UI Library**: Spectre.Console for interactive menus
- **Logging**: Serilog with structured logging and configurable output
- **Testing**: xUnit with comprehensive test coverage
- **Precision**: Uses `decimal` type for financial-grade accuracy
- **Culture**: InvariantCulture for consistent worldwide behavior
- **Input Handling**: Accepts both `.` and `,` as decimal separators

## Development

### Architecture Principles

- **Attribute-Driven**: Operations discovered through `[Discover]` and `[OperationCategory]` attributes
- **Reflection-Based**: Runtime operation discovery and execution
- **Type Safety**: Compile-time and runtime validation of operation signatures
- **Separation of Concerns**: Clear boundaries between Core logic, CLI interface, and Tests
- **Quality Assurance**: Comprehensive test coverage with automated CI/CD quality gates

### Development Environment

- **IDE**: Visual Studio Code on Windows
- **Shell**: PowerShell (note: use `;` instead of `&&` for command chaining)
- **Culture**: Uses InvariantCulture to ensure consistent decimal parsing worldwide
- **CI/CD**: GitHub Actions with cross-platform matrix builds
- **Testing**: 42+ comprehensive unit tests with Theory/InlineData patterns

## Contributing

1. Fork the repository
2. Add new operation methods with proper `[Discover]` attributes
3. Include comprehensive unit tests following existing patterns
4. Ensure all 42+ tests pass locally (`dotnet test`)
5. Follow the existing naming patterns and architectural principles
6. Test with both `.` and `,` decimal separators for international compatibility
7. Submit a pull request - GitHub Actions will automatically validate your changes

*Note: The CI/CD pipeline will only create builds if all tests pass, ensuring code quality*

## License

This project is open source and available under the MIT License.
