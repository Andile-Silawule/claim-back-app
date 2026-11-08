# ClaimBack – Warranty & Refund Recovery Platform

**A business-focused platform for managing purchases, warranties and refund claims.**

## Overview

ClaimBack is a web-based application designed to help consumers and small organisations keep track of their purchases, monitor warranty periods and manage refund or warranty claims.

The platform addresses a common problem: people often lose track of purchase records, forget warranty expiration dates and struggle to organise information when requesting refunds or warranty-related assistance.

ClaimBack aims to bring this information together in one centralised platform.

## The Problem

Managing purchases and warranty claims can become difficult when information is scattered across receipts, emails and paper documents.

Common challenges include:

- Losing proof of purchase.
- Forgetting warranty expiration dates.
- Missing retailer return deadlines.
- Struggling to track the progress of claims.
- Having no central record of previous purchase and claim activity.

## Proposed Solution

ClaimBack provides a centralised system where users can:

- Register and manage their purchases.
- Record purchase prices, retailers and purchase dates.
- Calculate warranty expiration dates.
- Store receipts and supporting documents.
- Create and track refund or warranty claims.
- Monitor claim progress and outcomes.
- View purchase and claim information through a dashboard.

## Technology Stack

| Component | Technology |
|---|---|
| Backend | C# / ASP.NET Core Web API |
| Frontend | Angular |
| Programming Language | TypeScript |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| API Architecture | RESTful APIs |
| Testing | xUnit / Angular Testing |
| Version Control | Git & GitHub |
| Cloud | Planned cloud deployment |

## Architecture

ClaimBack follows a layered backend architecture.

```text
ClaimBack
│
├── Frontend
│   └── Angular + TypeScript
│
└── Backend
    └── ClaimBack.API
        ├── Controllers
        ├── Services
        ├── DTOs
        ├── Models
        └── Data
            └── ClaimBackDbContext
```

### Backend responsibilities

- **Controllers:** Handle incoming HTTP requests and return API responses.
- **Services:** Contain business logic and application operations.
- **DTOs:** Define the data accepted and returned by API operations.
- **Models:** Represent application entities.
- **Data:** Manages database access through Entity Framework Core.

## Current Development Progress

The backend development is underway.

Implemented so far:

- ASP.NET Core Web API project setup.
- SQL Server database configuration.
- Entity Framework Core integration.
- Database migrations.
- Purchase entity and database persistence.
- Purchase creation endpoint.
- Retrieve all purchases endpoint.
- Retrieve an individual purchase endpoint.
- Initial purchase update functionality under development.

## Planned Features

- User registration and authentication.
- Purchase management.
- Warranty expiration tracking.
- Separate retailer return deadlines.
- Receipt and document uploads.
- Warranty and refund claim management.
- Claim status tracking and activity history.
- Dashboard and reporting.
- Search, filtering and sorting.
- In-app notifications and reminders.
- Cloud deployment.

## Project Goals

The primary goals of ClaimBack are to:

1. Build a practical business application that solves a real-world problem.
2. Apply clean backend architecture and REST API development principles.
3. Develop full-stack skills using C#, ASP.NET Core, Angular and SQL Server.
4. Implement database relationships and business rules.
5. Gain practical experience with application security, testing and cloud deployment.

## Important Scope

ClaimBack is a purchase and claim management platform. It helps users organise and track their claims but does not automatically submit claims to retailers or manufacturers, guarantee refunds or determine legal eligibility.

## Project Status

**Status:** In Development

This project is actively being developed, with backend functionality being implemented incrementally before frontend integration.

---

*Built as a full-stack software engineering portfolio project.*