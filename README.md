# ATM Monitoring Agent Simulation

ATM Monitoring Agent Simulation adalah simulasi sederhana agent monitoring ATM menggunakan C# .NET Worker Service.

Project ini dibuat untuk memenuhi coding test .NET Developer dengan fokus pada:

* asynchronous worker/service flow
* retry mechanism
* offline pending storage
* monitoring simulation
* clean code structure
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
* Graceful shutdown
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

```text
AtmMonitoring.Agent
│
├── Application
│   └──Interfaces
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
```

---

# Application Flow

```text
START CYCLE

1. Retry pending statuses
2. Remove pending if retry success
3. Collect ATM status
4. Send status to monitoring service
5. Save to pending storage if failed
6. Delay next cycle

END CYCLE
```

---

# Pending Storage

Failed status will be saved into:

```text
storage/pending-status.json
```

The pending storage is persistent and survives application restart.

---

# Monitoring Client Simulation

Monitoring client simulates network communication to backend monitoring service.

Current implementation:

* random network availability simulation
* retry capable flow
* offline buffer support

---

# Sequence Diagram

```text
ATM Provider
    ↓
Worker Service
    ↓
Monitoring Client
    ↓
Success / Failed
    ↓
Pending Storage
    ↓
Retry Next Cycle
```

---

# How To Run

1. Open solution in Visual Studio
2. Build solution
3. Run project

Or using CLI:

```bash
dotnet build
dotnet run --project AtmMonitoring.Agent
```

---

# Running Unit Tests

```bash
dotnet test
```

---

# Future Improvements

Possible future improvements:

* SQLite pending storage
* HTTP API integration
* Windows Service deployment
* RabbitMQ/Kafka integration
* Docker support
* Centralized logging
* Metrics and observability


* # Sample Logs

See:

* docs/sample-log.png

