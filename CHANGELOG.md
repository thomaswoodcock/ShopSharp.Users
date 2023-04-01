# Changelog

All notable changes to this project will be documented in this file.

## v0.3.1 - 2023-04-01

### Fixed

- `Application.UnitTests` root namespace.

## v0.3.0 - 2023-03-31

### Changed

- Updated `CreateUserCommandHandler` to check for existing users with the same email address before creating a new user.

## v0.2.0 - 2023-03-30

### Added

- `Application` project.
- `CreateUserCommand` with handler and validator.

### Changed

- Moved error enums outside of classes but within same file.
- Upgraded `ShopSharp.Core.Domain` to v0.3.0.

## v0.1.7 - 2023-03-29

### Added

- `User` aggregate with `Create` factory method.