# MyCalc - .NET Calculator with Operation Discovery

A modern .NET 8.0 console application that automatically discovers operations through attributes and reflection, featuring an interactive menu system with categorized operations and cross-platform deployment automation.

## Features

- 🔍 **Automatic Operation Discovery** - Operations are automatically found using `[Discover]` attributes
- 📂 **Categorized Menu System** - Operations organized into logical categories with submenus
- 🌍 **International Support** - Handles both `.` and `,` as decimal separators for different locales
- 🎯 **Type Safety** - Uses `decimal` for financial-grade precision
- 🖥️ **Beautiful CLI** - Interactive menus powered by Spectre.Console
- ✅ **Comprehensive Tests** - Full test coverage with xUnit (44 tests)
- 🏗️ **Clean Architecture** - Layered design with Core/CLI/Tests separation
- 🚀 **Cross-Platform Builds** - Automated builds for Windows, Linux, and macOS via GitHub Actions
- 📦 **Release Automation** - Automatic artifact creation with platform-specific executables

## Getting Started

### Quick Start (Development)

1. Clone the repository
2. Run the calculator:
   ```powershell
   .\run.ps1
   ```

### Download Pre-built Releases

1. Go to the [Actions](https://github.com/devcronberg/MyCalc/actions) page
2. Find the latest workflow run and download the appropriate package for your platform:
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
├── MyCalcTests/            # Unit tests (44 comprehensive tests)
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

- ✅ **Quality Gates** - All 44 tests must pass before any builds are created
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

### Financial (Category 4)
- **Bitcoin Price** - Gets current Bitcoin price in USD from CoinGecko API
- **Ethereum Price** - Gets current Ethereum price in USD from CoinGecko API

*Operations are automatically sorted by category priority and displayed in interactive submenus*

## Build & Deployment

### Local Development
```powershell
# Build entire solution
dotnet build --configuration Release

# Run all 44 tests
dotnet test

# Run the application
dotnet run --project MyCalcCli --configuration Release
```

### Cross-Platform Release Builds
The GitHub Actions workflow automatically creates platform-specific builds:

```yaml
# Builds for: win-x64, linux-x64, osx-x64
# Includes: Self-contained executable, launcher scripts, documentation
# Quality gate: All 44 tests must pass before builds are created
```

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
- **Quality Assurance**: Comprehensive test coverage with automated CI/CD quality gates

### Development Environment

- **IDE**: Visual Studio Code on Windows
- **Shell**: PowerShell (note: use `;` instead of `&&` for command chaining)
- **Culture**: Uses InvariantCulture to ensure consistent decimal parsing worldwide
- **CI/CD**: GitHub Actions with cross-platform matrix builds
- **Testing**: 44 comprehensive unit tests with Theory/InlineData patterns

## Contributing

1. Fork the repository
2. Add new operation methods with proper `[Discover]` attributes
3. Include comprehensive unit tests following existing patterns
4. Ensure all 44 tests pass locally (`dotnet test`)
5. Follow the existing naming patterns and architectural principles
6. Test with both `.` and `,` decimal separators for international compatibility
7. Submit a pull request - GitHub Actions will automatically validate your changes

*Note: The CI/CD pipeline will only create builds if all tests pass, ensuring code quality*

## License

This project is open source and available under the MIT License.

## Examples of prompting AI for help

### Upgrade to .NET 10

```markdown
# Upgrade the project to .NET 10

## Overview
Upgrade the entire MyCalc solution to target .NET 10, ensuring all three projects (MyCalcCore, MyCalcCli, MyCalcTests), dependencies, GitHub Actions workflow, and documentation are updated accordingly. Verify that the application builds, all 44 tests pass, and runs correctly after the upgrade.

## Requirements
- Update all `.csproj` files in MyCalcCore, MyCalcCli, and MyCalcTests to target `net10.0`.
- Review and update all NuGet package versions to their latest .NET 10-compatible versions:
  - Spectre.Console (currently 0.50.0)
  - xUnit and related test packages (currently 2.5.3)
  - Microsoft.Extensions.Configuration packages
  - Microsoft.NET.Test.Sdk
- Update `.github/workflows/build-and-release.yml` to use .NET 10 SDK (`dotnet-version: '10.0.x'`).
- Update all documentation references from ".NET 8.0" to ".NET 10":
  - README.md (multiple locations)
  - .github/copilot-instructions.md
  - Any other markdown files
- Verify all 44 tests pass after upgrade.
- Test the interactive CLI menu system to ensure compatibility.
- Use the branch naming convention: `feature/upgrade-dotnet10`.

## Out of Scope
- No changes to application functionality, features, or operation logic.
- No changes to test coverage or test implementation beyond compatibility fixes.
- No UI/UX changes to the Spectre.Console menu system.

## Branch
Please name the branch `feature/upgrade-dotnet10`

## Acceptance Criteria
- [ ] All three `.csproj` files target `net10.0`.
- [ ] All NuGet packages are updated to .NET 10-compatible versions.
- [ ] GitHub Actions workflow uses .NET 10 SDK.
- [ ] All documentation updated to reference .NET 10.
- [ ] Solution builds successfully with `dotnet build`.
- [ ] All 44 tests pass with `dotnet test`.
- [ ] Application runs successfully with `dotnet run --project MyCalcCli`.
- [ ] Interactive menu system works correctly.
- [ ] Cross-platform builds succeed in GitHub Actions.
- [ ] Changes are committed to the `feature/upgrade-dotnet10` branch.

## Testing Steps
1. Run `dotnet build` - should succeed
2. Run `dotnet test` - all 44 tests should pass
3. Run `.\run.ps1` - CLI should launch and operate normally
4. Test both Bitcoin and Ethereum price operations
5. Test Basic Arithmetic and Advanced Math operations
6. Verify international decimal input (both `.` and `,`)

## References
- [.NET 10 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)
- [Breaking changes in .NET 10](https://learn.microsoft.com/en-us/dotnet/core/compatibility/10.0)
```

### Structured Logging with Serilog

```
# Implement Serilog-based structured logging in CLI and Core projects

## Overview
Introduce structured logging using Serilog for both the CLI and Core projects, with output to the console and configuration via `appsettings.json`. Replace any ad-hoc logging with Serilog and ensure all relevant operations and error handling are logged at information, debug, or error levels as appropriate.

## Requirements
- Integrate Serilog as the exclusive logging provider (do not use the Microsoft logging abstraction).
- Apply logging to both the CLI and Core projects - in all operations.
- Configure Serilog via `appsettings.json` (do not hard-code sinks or levels).
- Enable output to the console sink only (plain text, template: `[{{Timestamp:HH:mm:ss}} {{Level:u3}}] {{Message:lj}} {{Properties:j}}{{NewLine}}{{Exception}}`).
- Minimum log level: Information; also enable Debug-level logging for deeper diagnostics.
- Log all "normal" operations at Information or Debug level as appropriate.
- Log all error/exception scenarios at Error level, with stack trace.
- Add a top-level error handler to capture and log unhandled exceptions.
- Remove or replace any existing Console.WriteLine or alternative logging statements in the CLI and Core code.
- Ensure logging configuration can be changed via `appsettings.json` (e.g., log level override).
- Update documentation in both `README.md` and `copilot-instructions.md` to reflect logging usage and how to configure it.
- Use the branch naming convention: `feature/logging-serilog` or similar.

## Out of Scope
- No persistent/file/remote sinks for now.
- No changes to test projects or test logging.

## Branch

Please name the branch `feature/logging-serilog`

## Acceptance Criteria
- [ ] Serilog is integrated and initialized via `appsettings.json` in the CLI  project.
- [ ] Console output uses the specified template, with both Information and Debug levels working.
- [ ] All error handling paths log at Error level with stack trace.
- [ ] No ad-hoc logging remains.
- [ ] Documentation updated in both `README.md` and `copilot-instructions.md`.
- [ ] Implementation is committed using the `feature/logging-serilog` branch.

## References
- [Serilog Documentation](https://serilog.net/)
- [Serilog Console Sink](https://github.com/serilog/serilog-sinks-console)
```

### Add Multiply and Divide Operations

```markdown
# Add Multiply and Divide operations to Basic Arithmetic with comprehensive tests

## Overview
Add `Multiply` and `Divide` methods to the `BasicArithmetic` class in MyCalcCore with `[Discover]` attributes, ensuring they automatically appear in the CLI menu. Include comprehensive unit tests following the exact same patterns used in the existing `BasicArithmeticTests.cs` file.

## Requirements
- Add two new methods to `MyCalcCore/Operations/BasicArithmetic.cs`:
  - `Multiply(decimal a, decimal b)` - Multiplies two decimal numbers
  - `Divide(decimal a, decimal b)` - Divides first number by second number
- Use `[Discover]` attributes with descriptive parameters matching existing pattern.
- Methods should return `decimal` type for consistency.
- Add comprehensive tests to `MyCalcTests/BasicArithmeticTests.cs`:
  - Follow the **exact same test patterns** as existing Add and Subtract tests
  - Use `[Theory]` with `[InlineData]` for parameterized tests (10+ test cases each)
  - Use `[Fact]` for specific edge case tests
  - Follow naming pattern: `MethodName_Scenario_ExpectedResult`
  - Include similar edge cases as Add/Subtract: positive, negative, zero, decimals, large numbers
  - For Divide: Include division by zero test (should throw exception)
- Ensure all tests pass (target: 64+ tests total, currently 44).
- No CLI changes needed - operations will be auto-discovered.
- Use branch naming convention: `feature/multiply-divide-operations`.

## Implementation Guidelines
- **Study existing code**: Review `Add` and `Subtract` methods in `BasicArithmetic.cs` for implementation pattern
- **Study existing tests**: Review all Add and Subtract tests in `BasicArithmeticTests.cs` and replicate the exact same patterns
- **Follow conventions**: Use same naming, structure, and Assert patterns as existing tests
- **Edge cases**: Cover similar scenarios as existing tests (positive, negative, zero, decimals, large numbers)
- **Division by zero**: Handle appropriately with exception

## Out of Scope
- No changes to other operations or operation classes.
- No changes to CLI, menu system, or Program.cs.
- No changes to other test files (AdvancedArithmeticTests.cs, FinancialTests.cs).
- No changes to project structure or configuration.

## Branch
Please name the branch `feature/multiply-divide-operations`

## Acceptance Criteria
- [ ] Both `Multiply` and `Divide` methods added to `BasicArithmetic.cs` with correct `[Discover]` attributes.
- [ ] Methods return `decimal` and accept two `decimal` parameters.
- [ ] Divide method includes zero-check with appropriate exception.
- [ ] 20+ comprehensive unit tests added (10+ for each operation) to `BasicArithmeticTests.cs`.
- [ ] Tests follow **exact same patterns** as existing Add/Subtract tests.
- [ ] All edge cases covered including division by zero exception test.
- [ ] Solution builds successfully with `dotnet build`.
- [ ] All tests pass (64+ total tests expected: 44 existing + ~20 new).
- [ ] Both operations appear automatically in CLI menu under "Basic Arithmetic".
- [ ] Manual testing confirms correct behavior for both operations.
- [ ] Division by zero is properly handled with exception.
- [ ] Changes committed to `feature/multiply-divide-operations` branch.

## Testing Steps
1. Run `dotnet build` - should succeed
2. Run `dotnet test` - should show 64+ tests passing
3. Run `.\run.ps1` and navigate to Basic Arithmetic category
4. Verify "Multiply" and "Divide" appear in the operation list
5. Test multiplication with various inputs (positive, negative, decimals)
6. Test division with various inputs including edge cases
7. Verify division by zero shows appropriate error message
8. Test with both `.` and `,` as decimal separators

## References
- **PRIMARY**: Study existing test patterns in `MyCalcTests/BasicArithmeticTests.cs`
- Existing `Add` and `Subtract` methods in `MyCalcCore/Operations/BasicArithmetic.cs`
- Current test count: 44 tests (all passing)
```

### Implement Meziantou.Analyzer

```markdown
# Implement Meziantou.Analyzer for code quality

## Overview
Integrate Meziantou.Analyzer (https://github.com/meziantou/Meziantou.Analyzer) into the MyCalc solution to enforce code quality standards and best practices. Ensure that MyCalcCore and MyCalcCli projects build without any analyzer warnings.

## Requirements
- Add Meziantou.Analyzer NuGet package to MyCalcCore and MyCalcCli projects (latest version).
- Do NOT add the analyzer to MyCalcTests project.
- Configure analyzer settings appropriately (use `.editorconfig` or project properties).
- Fix all analyzer warnings in MyCalcCore and MyCalcCli projects:
  - Review each warning and fix according to best practices
  - Suppress warnings only if absolutely necessary with proper justification
  - Maintain existing functionality while improving code quality
- Ensure all 44 existing tests still pass after code changes.
- Build should complete with zero warnings from Meziantou.Analyzer.
- Use branch naming convention: `feature/meziantou-analyzer`.

## Implementation Guidelines
- Add the NuGet package to both MyCalcCore and MyCalcCli `.csproj` files
- Run `dotnet build` and review all analyzer warnings
- Fix warnings systematically, testing after each change
- Common areas to address:
  - Async/await patterns
  - Disposal patterns (IDisposable)
  - String handling and culture specifications
  - Exception handling best practices
  - Naming conventions
  - Performance optimizations
- If suppression is needed, use `#pragma warning disable` with a comment explaining why

## Out of Scope
- No analyzer for test projects (MyCalcTests).
- No changes to application functionality or features.
- No breaking changes to public APIs or operation signatures.

## Branch
Please name the branch `feature/meziantou-analyzer`

## Acceptance Criteria
- [ ] Meziantou.Analyzer NuGet package added to MyCalcCore and MyCalcCli projects.
- [ ] Package version documented in acceptance criteria or commit message.
- [ ] Both MyCalcCore and MyCalcCli build with **zero warnings** from the analyzer.
- [ ] All 44 existing tests pass without modification (unless required by analyzer fixes).
- [ ] Code improvements maintain backward compatibility.
- [ ] Solution builds successfully with `dotnet build`.
- [ ] `dotnet build` output shows no analyzer warnings.
- [ ] Application runs correctly with `dotnet run --project MyCalcCli`.
- [ ] No functionality regressions in CLI or operations.
- [ ] Changes committed to `feature/meziantou-analyzer` branch.

## Testing Steps
1. Add Meziantou.Analyzer package to both projects
2. Run `dotnet build` and note all warnings
3. Fix warnings one by one, running `dotnet build` after each fix
4. Run `dotnet test` to ensure all 44 tests still pass
5. Run `.\run.ps1` and manually test all operations
6. Final `dotnet build` should show zero analyzer warnings
7. Verify Bitcoin and Ethereum price operations work
8. Verify Basic Arithmetic and Advanced Math operations work

## References
- [Meziantou.Analyzer GitHub](https://github.com/meziantou/Meziantou.Analyzer)
- [Meziantou.Analyzer Rules Documentation](https://github.com/meziantou/Meziantou.Analyzer/tree/main/docs)
- Current solution structure: MyCalcCore, MyCalcCli (targets for analyzer)
- All tests remain passing
```