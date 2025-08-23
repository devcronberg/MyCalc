# TODO - Future Operations and Features

This document outlines potential new operations and features that could be added to MyCalc. Each item includes implementation suggestions and category placement.

## Financial Operations

### VAT (Value Added Tax) Functions
**Category**: "Financial", Priority: 4
**Configuration**: VAT percentage from config file or user setting

- **AddVAT** - Add VAT to a net amount
  - `[Discover("Add VAT", "Adds VAT to net amount", "Net amount", "VAT percentage (optional)")]`
  - Example: 100 + 25% VAT = 125

- **RemoveVAT** - Remove VAT from gross amount  
  - `[Discover("Remove VAT", "Removes VAT from gross amount", "Gross amount", "VAT percentage (optional)")]`
  - Example: 125 - 25% VAT = 100

- **CalculateVAT** - Calculate VAT amount only
  - `[Discover("Calculate VAT", "Calculates VAT amount", "Net amount", "VAT percentage (optional)")]`
  - Example: 100 * 25% = 25

### Investment & Loan Calculations
**Category**: "Financial", Priority: 4

- **CompoundInterest** - Calculate compound interest
  - `[Discover("Compound Interest", "Calculates compound interest", "Principal", "Interest rate (%)", "Years")]`

- **LoanPayment** - Calculate monthly loan payment
  - `[Discover("Loan Payment", "Calculates monthly loan payment", "Principal", "Annual interest rate (%)", "Years")]`

- **FutureValue** - Calculate future value of investment
  - `[Discover("Future Value", "Calculates future value", "Present value", "Interest rate (%)", "Years")]`

- **PresentValue** - Calculate present value
  - `[Discover("Present Value", "Calculates present value", "Future value", "Interest rate (%)", "Years")]`

## Cryptocurrency Integration

### Crypto Rate Functions
**Category**: "Crypto Rates", Priority: 6
**Implementation**: HTTP API calls to crypto exchanges

- **GetBitcoinPrice** - Get current Bitcoin price in USD
  - `[Discover("Bitcoin Price", "Gets current Bitcoin price in USD")]`
  - API: CoinGecko, CryptoCompare, or Binance

- **GetCryptoPrice** - Get price for any cryptocurrency
  - `[Discover("Crypto Price", "Gets current crypto price", "Crypto symbol (BTC, ETH, etc.)", "Currency (USD, EUR, etc.)")]`

- **ConvertCrypto** - Convert between cryptocurrencies
  - `[Discover("Convert Crypto", "Converts between cryptocurrencies", "Amount", "From crypto", "To crypto")]`

- **CryptoToFiat** - Convert crypto to fiat currency
  - `[Discover("Crypto to Fiat", "Converts crypto to fiat", "Crypto amount", "Crypto symbol", "Fiat currency")]`

## Mathematical Extensions

### Statistics Operations
**Category**: "Statistics", Priority: 3

- **Mean** - Calculate arithmetic mean
  - `[Discover("Mean", "Calculates arithmetic mean", "Numbers (comma-separated)")]`

- **Median** - Calculate median value
  - `[Discover("Median", "Calculates median", "Numbers (comma-separated)")]`

- **StandardDeviation** - Calculate standard deviation
  - `[Discover("Standard Deviation", "Calculates standard deviation", "Numbers (comma-separated)")]`

- **Variance** - Calculate variance
  - `[Discover("Variance", "Calculates variance", "Numbers (comma-separated)")]`

### Trigonometry Operations
**Category**: "Trigonometry", Priority: 5

- **Sin** - Calculate sine
  - `[Discover("Sine", "Calculates sine", "Angle in degrees")]`

- **Cos** - Calculate cosine
  - `[Discover("Cosine", "Calculates cosine", "Angle in degrees")]`

- **Tan** - Calculate tangent
  - `[Discover("Tangent", "Calculates tangent", "Angle in degrees")]`

- **ArcSin** - Calculate arcsine
  - `[Discover("Arcsine", "Calculates arcsine", "Value")]`

### Advanced Math Operations
**Category**: "Advanced Math", Priority: 2

- **Factorial** - Calculate factorial
  - `[Discover("Factorial", "Calculates factorial", "Number")]`

- **Power** - Calculate power/exponent
  - `[Discover("Power", "Calculates number to the power of exponent", "Base", "Exponent")]`

- **SquareRoot** - Calculate square root
  - `[Discover("Square Root", "Calculates square root", "Number")]`

- **Logarithm** - Calculate natural logarithm
  - `[Discover("Natural Log", "Calculates natural logarithm", "Number")]`

- **Log10** - Calculate base-10 logarithm
  - `[Discover("Log Base 10", "Calculates base-10 logarithm", "Number")]`

## Unit Conversion Operations

### Length Conversions
**Category**: "Unit Conversions", Priority: 7

- **MetersToFeet** - Convert meters to feet
- **FeetToMeters** - Convert feet to meters
- **KilometersToMiles** - Convert kilometers to miles
- **MilesToKilometers** - Convert miles to kilometers

### Temperature Conversions
**Category**: "Unit Conversions", Priority: 7

- **CelsiusToFahrenheit** - Convert Celsius to Fahrenheit
- **FahrenheitToCelsius** - Convert Fahrenheit to Celsius
- **CelsiusToKelvin** - Convert Celsius to Kelvin
- **KelvinToCelsius** - Convert Kelvin to Celsius

### Weight Conversions
**Category**: "Unit Conversions", Priority: 7

- **KilogramsToPounds** - Convert kilograms to pounds
- **PoundsToKilograms** - Convert pounds to kilograms
- **GramsToOunces** - Convert grams to ounces
- **OuncesToGrams** - Convert ounces to grams

## Utility Operations

### Date/Time Calculations
**Category**: "Date & Time", Priority: 8

- **DateDifference** - Calculate difference between two dates
- **AddDaysToDate** - Add days to a date
- **WorkdaysBetween** - Calculate workdays between dates (excluding weekends)
- **AgeCalculator** - Calculate age from birth date

### Number Operations
**Category**: "Number Utils", Priority: 9

- **IsPrime** - Check if number is prime
- **GreatestCommonDivisor** - Calculate GCD of two numbers
- **LeastCommonMultiple** - Calculate LCM of two numbers
- **RandomNumber** - Generate random number in range

## Implementation Guidelines

### Configuration System
- Add `appsettings.json` for configurable values (VAT rates, API keys, etc.)
- Use `IConfiguration` for dependency injection
- Environment-specific configurations for development/production

### External API Integration
- Add HTTP client services for crypto APIs
- Implement caching for API responses
- Error handling for network failures
- Rate limiting compliance

### Enhanced Input Handling
- Support for comma-separated number lists for statistics functions
- Date parsing for date/time operations
- Currency symbol recognition
- Angle unit specification (degrees/radians)

### Testing Considerations
- Mock external APIs in tests
- Test edge cases for financial calculations
- Validate mathematical precision for advanced operations
- Test international number formats and currencies

### Performance Optimizations
- Cache crypto prices for short periods
- Optimize mathematical calculations for large numbers
- Implement async operations for API calls
- Consider parallel processing for complex calculations

## Priority Implementation Order

1. **Basic Math Extensions** (Priority 2) - Power, SquareRoot, Factorial
2. **Statistics** (Priority 3) - Mean, Median, StandardDeviation
3. **Financial** (Priority 4) - VAT functions, CompoundInterest
4. **Trigonometry** (Priority 5) - Sin, Cos, Tan
5. **Crypto Rates** (Priority 6) - Bitcoin price, crypto conversions
6. **Unit Conversions** (Priority 7) - Length, temperature, weight
7. **Date & Time** (Priority 8) - Date calculations
8. **Number Utils** (Priority 9) - Prime checks, GCD/LCM

Each category will automatically appear in the CLI menu system with proper sorting based on priority values.
