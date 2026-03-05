# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.0.2] - (05/03/2026)
### Fixed
- `CLIParse.Parse(string[]?)` now correctly uses the configurable `CLIParse.ArgumentCode` and 
`CLIParse.EndCode` properties instead of the hard‑coded default token values 
(`CLIDefaultToken.Argument` and `CLIDefaultToken.EndCode`). \
This resolves an issue where the token list was generated with incorrect type codes when custom 
token codes were set.

## [2.0.1] - (01/03/2026)
### Added
- Added a check in `DefaultFunction.Analyzer` to detect whether the `TokenList` has reached the end‑of‑input marker (`EndCode`). If the list still contains tokens after processing all defined options, the analyzer now returns an error with specific codes:
  - Error code 74 – "The argument is not defined for the function ({alias})!"
  - Error code 75 – "The option ({list.CurrentKey}) is not defined for the function ({alias})!"

### Changed
- Bumped package version to 2.0.1.

## [2.0.0-rc.16] - (28/02/2026)
### Added
- Completely rewritten `README.md` with comprehensive English documentation, including installation, features, usage examples, API overview, and advanced topics.

### Changed
- Merged `remove_func` and `create_func` into a single `cli_func` in the test program to simplify demonstration.
- Modified `IOptionFunc.TreatedValue` method to accept a nullable `TokenList` parameter.
- Updated `DefaultArgument` and `DefaultOption` to handle new error codes and improve exception messages.

## [2.0.0-rc.15] - (27/02/2026)
### Added
- Added `EndCode` and `ArgumentCode` properties to `CLIParse` for custom token types.

### Changed
- Reverted `README.md` to Portuguese, providing detailed descriptions of standard and custom implementations.
- Updated test program (`Program.cs`) to use `CLIParse.AddFunction` with function IDs and improved analyzer logic.
- Fixed analyzer in `DefaultOption` to correctly validate token order.
- Enhanced `DefaultArgument` exception messages with specific error codes for function and argument mismatches.

## [2.0.0-rc.14] - (26/02/2026)
### Added
- Initial implementation of version 2.0.0 with Portuguese README.
- Introduced `CLIParse` static class with function registry (`AddFunction`, `GetFunction`) and token‑based parsing.
- Added `CLIDefaultToken` enum to define standard token types (Function, Option, Argument, EndCode).
- Created `CLIKey` struct to handle compound aliases with separator `/`.
- Added `CLIValueOrder` class to store parsed values keyed by `CLIKey`.
- Implemented default structures: `DefaultFunction`, `DefaultOption`, `DefaultArgument` with support for registered delegates.
- Added `TokenList` class to wrap parsed token pairs with a movable cursor.
- Introduced `ErrorMessage` class for error handling.
- Added comprehensive XML documentation to all public APIs.

## [2.0.0-rc.13] - (26/02/2026)
### Changed
- Minor cleanup: removed unused `using` statement in `CLIParse.cs`.

## [2.0.0-rc.12] - (26/02/2026)
### Added
- Added `CLIValueOrder` class to manage ordered key‑value pairs.
- Added XML documentation for all core types and members.

### Changed
- Removed obsolete test interfaces from test project.
- Moved `CLIKey`, `CLIKeyValue`, `ErrorMessage`, `DefaultArgument`, `DefaultFunction`, `DefaultOption`, `TokenList` into `Cobilas.CLI.Manager` namespace.
- Updated `CLIParse` to store functions in a dictionary and provide generic `GetFunction<T>`.
- Improved exception handling with `ExceptionMessages` helper class.

## [2.0.0-rc.11] - (22/02/2026)
### Added
- Added `EndCode` property to `CLIParse` to mark end of input.
- Added `DictionaryExtension` class with `Find` method.
- Introduced `NotNullAttribute` for .NET Framework compatibility.

### Changed
- Updated test program to use new `IFunction` and default implementations.

## [2.0.0-rc.10] - (21/02/2026)
### Added
- Added `ArgumentCode` property to `CLIParse` to allow custom argument token codes.

### Changed
- Updated test program to set `ArgumentCode` explicitly.

## [2.0.0-rc.9] - (20/02/2026)
### Added
- Added `CLIKey` struct to represent compound aliases.
- Enhanced test program with `RemoveFunc` example using `IOptionFunc` and `IArgument`.
- Extended interfaces `IAlias`, `IFunction`, `IOptionFunc`, `IOption`, `IArgument` with `TypeCode` and methods.

## [2.0.0-rc.8] - (19/02/2026)
### Added
- Added `NotDescribedException` with serialization support.

### Changed
- Simplified `CLIParse` by removing rule engine, token data, and analysis delegates.
- `Parse` method now returns `List<KeyValuePair<string, long>>` instead of `TokenList`.
- Removed `CLITokenData`, `InfoTokenData`, `TokenList`, `TokenListReadOnly`, and related delegates.
- Updated `ExceptionMessages` with `ThrowIfDisposable` and `ThrowIfNullOrWhiteSpace`.

## [2.0.0-rc.7] - (18/12/2025)
### Added
- Added `TokenExecutor` class to manage token execution with registered info.
- Introduced `InfoTokenData` to hold token‑specific data during parsing.
- Added `InvalidCLIArgumentException`, `InvalidCLIArgumentTypeException`, `InvalidCLIFunctionException`, `InvalidCLIOptionException`.

### Changed
- Updated test program to use `TokenExecutor` and rule‑based validation.

## [2.0.0-rc.6] - (18/12/2025)
### Added
- Enhanced `CLIParse` with rule engine: added `IsAutoException`, `Rule` delegate, `Analysis` delegate, `EndCode`, `ArgumentCode`, `TokenData`.
- Added `TokenList` and `TokenListReadOnly` classes for token collection.
- Implemented `InfoTokenData` derived from `CLITokenData` to store dynamic data.
- Added `NotDescribedException` for rule violation without description.
- Updated `ExceptionMessages` with null checks.

## [2.0.0-rc.5] - (02/12/2025)
### Added
- Added test project `Cobilas.CLI.Manager.Test`.
- Simplified `CLIParse` to basic token dictionary and `Parse` method returning list of tuples.
- Introduced `CLIToken` enum with `Function`, `Option`, `Text`.

## [2.0.0-rc.4] - (02/12/2025)
### Changed
- Updated project file to set `OutputPath` per framework.

## [2.0.0-rc.3] - (02/12/2025)
### Changed
- Renamed folders and project files to follow consistent naming.
- Added package metadata (authors, license, repository, etc.).

## [2.0.0-rc.2] - (02/12/2025)
### Changed
- Restructured project: moved old implementations to `Old` folder, created new test classes.
- Updated `.gitignore` and solution file.

## [2.0.0-rc.1] - (02/12/2025)
### Added
- First official release candidate for version 2.0.0.
- Reorganised codebase and prepared for NuGet publishing.

## [2.0.0-rc.1] (initial) - 23/06/2025
### Added
- Initial commit of the 2.0.0 branch.
- Updated `.gitignore` to minimal.
- Added `CHANGELOG.md` (empty).
- Migrated existing code from previous versions to new structure.

## [1.3.0-rc.1] - (22/08/2023)
### Added
- Added `PackageTags` "CLI" to project file.

## [1.3.0] - (22/08/2023)
### Added
- Added XML documentation comments to all public classes and members.
- Added `GenerateId` method to `CLICommand` to generate function IDs from strings.
- Enhanced `Cateter` method with better error messages.

## [1.2.2] - (24/02/2023)
### Added
- Added `RepositoryUrl` and `RepositoryType` to project file.

### Changed
- Minor improvements in `CLICommand.Cateter` error reporting.

## [1.2.1] - (18/02/2023)
### Fixed
- Fixed argument collection in `CLICommand.Cateter` to use indexed keys for arguments.

## [1.2.0] - (17/02/2023)
### Added
- Added `Contains` and `IndexOf` methods to `CLIArgCollection`.
- Improved argument handling in `CLICommand.Cateter`.

## [1.1.0] - (17/02/2023)
### Added
- Made `CLIArgCollection` implement `IReadOnlyList<CLIArg>`.
- Added indexer to `CLIArgCollection`.

## [1.0.0] - (17/02/2023)
### Added
- Initial release of Cobilas.CLI.Manager.
- Basic classes: `CLIArg`, `CLIArgCollection`, `CLICommand`, `CLIOption`, `CLICMDArg`, `ErrorMensager`, `StringArrayToIEnumerator`.
- Core parsing logic in `CLICommand.Cateter`.