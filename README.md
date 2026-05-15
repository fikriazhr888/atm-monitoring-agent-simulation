# ATM Monitoring Agent Simulation

ATM Monitoring Agent Simulation is a simple .NET Worker Service project that simulates an ATM monitoring agent.

The project focuses on:

* background worker/service flow
* ATM status collection
* monitoring status delivery
* retry mechanism
* local pending storage
* graceful shutdown
* dependency injection
* unit testing

---

# Features

* ATM status simulation
* Monitoring client simulation
* Retry pending mechanism
* Local JSON pending storage
* Duplicate pending prevention
* Structured logging
* Graceful shutdown using CancellationToken
* Unit testing with xUnit

---

# Tech Stack

* .NET 8 Worker Service
* C#
* Dependency Injection
* ILogger
* xUnit
* FluentAssertions
* JSON local storage

---

# Project Structure

```text id="1a2b3c"
AtmMonitoring.Agent
│
├── Application
│   └── Interfaces
│
├── Domain
│   ├── Models
│   └── Enums
│
├── Infrastructure
│   ├── Monitoring
│   └── Storage
│
├── Device
│   └── Providers
│
├── Worker.cs
└── Program.cs
```

---

# Architecture Decisions

This project uses a simple responsibility-based architecture:

* Domain → models and enums
* Application → interfaces/contracts
* Infrastructure → monitoring and storage implementations
* Device → ATM status provider simulation
* Worker → orchestration flow

Dependency Injection is used so implementations can be replaced without modifying Worker logic.

The architecture is intentionally kept lightweight to avoid unnecessary complexity for the coding test while still maintaining clean separation of concerns.

---

# Application Flow

```text id="4d5e6f"
START CYCLE

1. Retry pending statuses
2. Remove pending if retry succeeds
3. Collect new ATM status
4. Send status to monitoring service
5. Save pending if sending fails
6. Delay next cycle

END CYCLE
```

---

# Collect Status Flow

The Worker periodically calls `IAtmStatusProvider` to collect the latest ATM status.

The mock provider simulates:

* HEALTHY
* DEGRADED
* OFFLINE

Each status contains:

* unique StatusId
* terminal information
* timestamp
* ATM component statuses

`Guid.NewGuid()` is used to generate unique Status IDs and prevent duplicate monitoring data.

---

# Send Status Flow

After collecting ATM status, the Worker sends the status to the monitoring client.

Flow:

1. Collect ATM status
2. Send status to monitoring service
3. If failed → save to pending storage
4. If success → continue to next cycle

Simulation rules:

* new OFFLINE statuses fail during sending
* retry pending statuses may succeed if network connectivity becomes available again

This hybrid approach prevents pending OFFLINE data from remaining stuck forever.

---

# Pending Storage Flow

If sending fails, the status will be stored locally in:

```text id="7g8h9i"
storage/pending-status.json
```

Local storage is used to:

* prevent monitoring data loss
* support retry mechanism
* handle unstable network conditions

Duplicate pending statuses are prevented using StatusId validation before saving.

---

# Retry Flow

At the beginning of every Worker cycle:

1. Retry pending statuses first
2. Remove pending data if retry succeeds
3. Continue normal status collection

Retry is executed before collecting new statuses to prevent old pending data from being left behind or continuously accumulating.

---

# Monitoring Client Simulation

`MockMonitoringClient` simulates backend monitoring communication.

Simulation includes:

* network failure
* random connectivity issues
* send success/failure responses

This allows retry and pending storage behavior to be tested realistically.

---

# Graceful Shutdown

The Worker uses `CancellationToken` to support graceful shutdown.

Benefits:

* stop background tasks safely
* cancel delays immediately
* prevent hanging processes during application shutdown

---

# Error Handling

The project includes basic error handling for:

* failed ATM status collection
* failed monitoring delivery
* failed pending storage read/write
* duplicate pending status prevention
* graceful cancellation/shutdown

---

# Running The Project

## Using Visual Studio

1. Open the solution
2. Build the solution
3. Run the project

---

## Using CLI

```bash id="0j1k2l"
dotnet build
dotnet run --project AtmMonitoring.Agent
```

---

# Running Unit Tests

```bash id="3m4n5o"
dotnet test
```

---

# Sample Logs

```text id="6p7q8r"
info: ATM Monitoring Agent Started

info: Retry pending statuses started

info: No pending statuses found

info: Status Collected:
StatusId: STAT-a12bc34d
TerminalId: ATM-0001
OverallStatus: HEALTHY

info: Send Success: Status sent successfully
```

---

```text id="9s0t1u"
warn: Send Failed: Failed because ATM offline

warn: Status saved to pending storage:
STAT-b45cd67e
```

---

```text id="2v3w4x"
info: Retry pending statuses started

info: Retry pending success:
STAT-b45cd67e
```

---

# Replacing Mock Monitoring Client With Real Backend API

Currently the project uses `MockMonitoringClient`.

To integrate with a real backend API:

* replace `IMonitoringClient` implementation
* use `HttpClient`
* call monitoring API endpoints using HTTP POST
* add authentication/token handling

Because the project uses interfaces and Dependency Injection, the monitoring client implementation can be replaced without changing Worker logic.

---

# Considerations For Multiple ATM Machines

If the agent is deployed across many ATM machines, several additional considerations are important:

* centralized logging
* monitoring dashboard
* distributed retry strategy
* message broker (RabbitMQ/Kafka)
* database persistence
* deployment automation
* observability and metrics
* configuration management
* service health monitoring
* security and authentication

---

# Future Improvements

Possible future improvements:

* SQLite pending storage
* RabbitMQ/Kafka integration
* Docker deployment
* Windows Service deployment
* centralized logging platform
* monitoring dashboard
* distributed monitoring support
