# Backend Project with Docker and .NET

This project is a backend application built with .NET and runs using Docker. It includes a PostgreSQL database and a PgAdmin interface.

---

## Prerequisites

- [Docker](https://www.docker.com/get-started) installed on your machine  
- [Docker Compose](https://docs.docker.com/compose/install/) 
---


## Installation & Run

1. **Download the repository**
   ```bash
   git clone https://github.com/Icaro4fonso/IKT_BACKEND.git

2. **Navigate to the directory**
   ```bash
   cd IKT_BACKEND

3. **Environment Configuration**

    - Do not forget to use the provided `.env.example` file.

4. **Run the application using docker**
   ```bash
   docker compose up --build

5. **Access the aplication and database**
   
   - localhost:8080 (api) 
   - localhost:5051 (pgAdmin)

## Stacks 

1. **C# and .NET**

    - Perfect for API development with robust data validation libraries. Being a strongly typed language, C# ensures better readability and reduces runtime errors.

2. **PostgreSql**

    - Leveraging C# and Entity Framework with PostgreSQL enables seamless bulk inserts, effectively solving the project's primary data processing challenge.

2. **Docker**

    - Containerized setup: No need to install external tools or dependencies

