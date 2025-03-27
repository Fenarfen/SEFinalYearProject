---
nav_order: 2
---
# MADR (Architectural Decision Record) for CustardRM

Title: Frontend Technology Design Record for CustardRM

2025-03-17 00:35:00

## Context and Problem Statement

The system must be capable of allowing non-technical users to interact with the system. To do this, the business logic must be connected to an easy to understand and use interface.

The key decision involved is choosing:

Select the technology stack for the frontend

### Frontend Development Technologies

## Considered Options

* Blazor
* React
* Angular

## Decision Outcome

Chosen Technologies:

Blazor as the frontend technology.

### Consequences

* Good, Native .NET support for building interactive web user interfaces.
* Good, Allows full-stack C# development, allowing for the sharing of models or business logic.
* Bad, smaller ecosystem compared to more established JavaScript frameworks, unlikely to be an issue however, due to the size and scope of the intended project.

## Pros and Cons of the Options

### Blazor

* Good, allows for a unified .NET technology stack, reducing context switching and simplifying full-stack development.
* Good, has integrated state management and a component-based design.
* Good, Reduces the time for me to familiarise myself with the technology as I am already familiar with Blazor.
* Bad, has a smaller ecosystem compared to other, more established frameworks, due to being a newer technology.

### React

* Good, has a large ecosystem, with a large set of third-party libraries.
* Good, good performance for more complex UIs and applications.
* Bad, would require context switching between JS and C#, potentially introducing a longer development time.
* Bad, requires additional setup and implementation for full-stack development.
* Bad, introduces a more complex build and deployment process in comparison to Blazor.

### Angular

* Good, Comprehensive framework that provides built-in solutions for routing, forms, HTTP services, and more
* Good, Backed by Google with plenty of documentation and third party libraries
* Bad, Steep learning curve due to its opinionated structure
* Bad, Similar to React, will result in having a Js/TypeScript stack, requiring context switching and separate build pipelines if integrating into an ASP.NET backend
