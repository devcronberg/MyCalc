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


