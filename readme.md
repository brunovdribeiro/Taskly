# Taskly

A sophisticated task management system implementing Event Sourcing and CQRS patterns. Built with clean architecture
principles and Domain-Driven Design (DDD), it provides a robust foundation for task creation, assignment, and tracking
with complete event history.

## Architecture Overview

### Solution Structure

- `src/Api`: REST API endpoints and application bootstrapping
- `src/Application`: Command/Query handlers, DTOs, and business logic
- `src/Domain`: Domain models, events, and business rules
- `src/Infrastructure`: External dependencies and data persistence
- `src/Projector`: Event subscription handlers for read models and notifications

### Design Patterns

- Event Sourcing: Store state changes as events
- CQRS: Separate read and write operations
- DDD: Rich domain model with bounded contexts
- Clean Architecture: Independent layers with dependency rules

## Tech Stack

### Core Technology

- .NET 9.0
- C# 12
- ASP.NET Core Web API

### Data Storage

- EventStoreDB: Event storage and subscriptions
- PostgreSQL 17: Snapshot storage
- Redis: Read model caching with RedisJSON support

### Tools & Libraries

- Entity Framework Core
- NSwag: API documentation
- Docker & Docker Compose: Containerization

## Prerequisites

- .NET 9.0 SDK
- Docker Desktop
- IDE (recommended: JetBrains Rider or Visual Studio)
- Git

## Getting Started

### 1. Clone Repository

```bash
git clone https://github.com/brunovdribeiro/Taskly.git
cd taskly