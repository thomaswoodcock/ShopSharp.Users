# Changelog

All notable changes to this project will be documented in this file.

## v0.5.1 - 2023-04-07

### Changed

- `EmailAddress.Equals` and `EmailAddress.GetHashCode` to ignore casing of email address value.

## v0.5.0 - 2023-04-06

### Added

- `EventSourcing.EventStoreDB` project with integration tests.
- `docker-compose.yml` with EventStoreDB service.
- `IClock` interface with `SystemClock` implementation.
- `IDomainEventVersioningStrategy` with `SimpleDomainEventVersioningStrategy` implementation.
- `EventRecord` and factory class.
- `EventStoreDbEventStore` class.

### Changed

- Separated 'Arrange', 'Act', and 'Assert' sections in all unit tests.
- Updated NUKE build to run integration tests.
- `IEventStore` to use `EventRecord` objects.

## v0.4.0 - 2023-04-01

### Added

- `EventSourcing` project.
- `EventStoreUserRepository` class.

### Changed

- Unit test project assembly names.
- Upgrade `ShopSharp.Core.Domain` dependency.

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