# MyCalc - .NET Calculator with Operation Discovery

A modern .NET 8.0 console calculator application that automatically discovers mathematical operations through attributes and reflection, featuring an interactive menu system with categorized operations.

## Features

- 🔍 **Automatic Operation Discovery** - Operations are automatically found using `[Discover]` attributes
- 📂 **Categorized Menu System** - Operations organized into logical categories with submenus
- 🌍 **International Support** - Handles both `.` and `,` as decimal separators for different locales
- 🎯 **Type Safety** - Uses `decimal` for financial-grade precision
- 🖥️ **Beautiful CLI** - Interactive menus powered by Spectre.Console
- ✅ **Comprehensive Tests** - Full test coverage with xUnit
- 🏗️ **Clean Architecture** - Layered design with Core/CLI/Tests separation

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Windows with PowerShell (development environment)

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
├── MyCalcTests/            # Unit tests
│   ├── BasicArithmeticTests.cs
│   └── AdvancedArithmeticTests.cs
├── run.ps1                 # Quick start script
└── README.md
```

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

### Basic Arithmetic
- **Add** - Adds two decimal numbers
- **Subtract** - Subtracts second number from first

### Advanced Math
- **Square** - Calculates the square of a number

## Technical Details

- **Framework**: .NET 8.0
- **UI Library**: Spectre.Console for interactive menus
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

### Development Environment

- **IDE**: Visual Studio Code on Windows
- **Shell**: PowerShell (note: use `;` instead of `&&` for command chaining)
- **Culture**: Uses InvariantCulture to ensure consistent decimal parsing worldwide

## Contributing

1. Add new operation methods with proper `[Discover]` attributes
2. Include comprehensive unit tests
3. Follow the existing naming patterns
4. Test with both `.` and `,` decimal separators

## License

This project is open source and available under the MIT License.
