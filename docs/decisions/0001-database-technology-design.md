---
nav_order: 2
---
# MADR (Architectural Decision Record) for CustardRM

Title: Database Technology Design Record for CustardRM

2025-03-17 00:35:00

## Context and Problem Statement

The system must be capable of keeping permanent records of stock, users, and logs. Due to this, a database will be utilised to handle persistant data storage and handling.

The key decision involved is choosing:

select the database technology stack for persistant data handling

### Database Development Technologies

## Considered Options

* Microsoft SQL Server
* MySQL
* PostgreSQL

## Decision Outcome

Chosen Technologies:

Microsoft SQL Server as the database technology.

### Consequences

* Good, SQL Server's built-in security features ensure secure handling of sensitive user data and transactions.
* Good, SQL Server offers enterprise-level performance, scalability, and support, allowing the system to serve the projected number of users, as well as accomodate the estimated growth.
* Bad, SQL Server's enterprise edition will introduce higher costs due to the large-scale deployment, though this is justified by its performance and security features.

## Pros and Cons of the Options

### Microsoft SQL Server

* Good, excellent support for integration with ASP.NET Core, Entity Framework, and other .NET tools.
* Good, high-performance capabilities for handling complex queries, large datasets, and transactions.
* Good, scales well from small projects to large enterprise solutions.
* Good, advanced security features such as encryption and strong authentication protocols.
* Good, provides robust management tools like SQL Server Management Studio (SSMS).
* Bad, the enterprise edition can be expensive, which would likely be required due to the scale of the project.

### MySQL

* Good, free and open-source, making it attractive from a cost perspective.
* Good, well-established with a large user base and strong community support.
* Bad, less natural integration with .NET and Entity Framework, requiring third-party connectors that may increase complexity.
* Bad, compared to SQL Server, MySQL has fewer advanced enterprise-level features for scalability and security. Examples of this would be partition switching or sliding window partitioning, advanced query optimisation, and row-level security.

### PostgreSQL

* Good, free and highly scalable.
* Good, supports complex queries, JSON data types, and extensions, making it highly versatile.
* Developer familiarity: The team is more comfortable working with SQL Server, which is fully supported within the .NET stack.
* Good, a robust community with extensive resources and documentation.
* Bad, like MySQL, PostgreSQL lacks seamless integration with the .NET ecosystem, requiring additional workarounds.
