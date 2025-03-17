---
nav_order: 1
---
# MADR (Architectural Decision Record) for CustardRM

Title: Backend Technology Stack Design Record for CustardRM

2025-03-17 00:22:00

## Context and Problem Statement

I am developing a CRM service, capable of offering online CRM services to multiple companies and external services, which is augmented with AI for the purposes of testing the viability and usability of AI in modern CRM systems.

This system must allow users of the subscribed companies to view inventory, sales, purchases, internal finances of the subscribed company, and use the AI tools for those CRM features.

The key decision involved is choosing:

Select the technology stack for back-end development.

## Considered Options

Backend Technology Stack:

* ASP.NET Core
* Node.js

## Decision Outcome

Chosen Technologies:

ASP.NET Core.

### Consequences

* Good, given my experience in .NET, this will give me a good headstart by reducing learning time.
* Good, ASP.NET offers enterprise-level performance, scalability, and support, allowing the system to serve a large number of users.
* Bad, resource cost of the fully scaled application may be significantly more costly than a more minimalist backend framework

## Pros and Cons of the Options

### Backend Development technologies

#### ASP.NET Core

* Good, the team has previous experience with the .NET stack.
* Good, has excellent performance in web applications, especially with concurrent users.
* Good, provides robust, built-in security mechanisms such as identity management and authentication.
* Good, integrates smoothly with Visual Studio and other .NET-specific tools, enhancing productivity.
* Bad, can be steep for developers unfamiliar with the .NET stack.
* Bad, can be resource intensive compared to more minimalist backend frameworks.

#### Node.js

* Good, allows for JS everywhere, if going for a JS approach over C# .NET.
* Good, has a large ecosystem of packages accessible through NPM.
* Good, allows for scalable networked applications, making it a good contender for the implementaiton of microservices which require real time capabilities.
* Bad, possible performance bottleneck when performing computational tasks, due to the single-threaded event loop model.

### More Information

ASP.NET Core is the most efficient choice, allowing for the utilisation of the current capabilties of the team, and offers good scalability as well as robust performance.
