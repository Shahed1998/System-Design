# System Design Implementations

A collection of practical system design implementations built to explore how real-world distributed systems work under the hood.

## Purpose

This repository serves as my personal learning and experimentation space for implementing various system design concepts and architectures. Rather than focusing only on theoretical designs, the goal is to build working implementations that demonstrate how these systems behave in practice.

## Repository Structure

Each system design implementation is maintained in its own branch.

### Available Implementations

| Branch                  | Description                           |
| ----------------------- | ------------------------------------- |
| `main`                  | Repository overview and documentation |
| `feature/url-shortener` | URL Shortener implementation          |
| `feature/rate-limiter`  | Rate Limiter implementation           |

> More implementations will be added over time.

## How to Explore an Implementation

List all available branches:

```bash
git branch -a
```

Switch to a specific implementation:

```bash
git checkout feature/rate-limiter
```

Or clone and directly checkout a branch:

```bash
git clone <repository-url>
git checkout <branch-name>
```

## Topics Covered

This repository aims to cover implementations of:

- Rate Limiting
- URL Shortening
- Distributed Caching
- Message Queues
- Notification Systems
- Search Systems
- API Gateways
- Distributed Locks
- Load Balancing
- Service Discovery
- Event-Driven Architectures
- Real-Time Analytics
- And many more...

## Goals

- Understand system design beyond interview diagrams.
- Learn trade-offs between different architectural choices.
- Explore scalability, reliability, and performance challenges.
- Gain hands-on experience building distributed systems.
- Document implementation decisions and lessons learned.

## Disclaimer

These implementations are primarily educational and may not be production-ready. The focus is on understanding concepts, experimenting with ideas, and demonstrating architectural patterns.

## Contributing

This is currently a personal learning repository, but suggestions, discussions, and improvements are always welcome.

## License

This project is licensed under the MIT License.
