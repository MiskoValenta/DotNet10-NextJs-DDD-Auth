# DotNet10-NextJs-DDD-Auth
A production-ready, full-stack authentication boilerplate built with .NET 10 (Domain-Driven Design) and Next.js. Features highly secure JWT auth via HTTP-Only cookies, silent automated token refresh, Strongly Typed IDs, and PostgreSQL.

---

# 🛡️ DotNet10-NextJs-DDD-Auth 🛡️

A full-stack, production-ready authentication boilerplate demonstrating **Domain-Driven Design (DDD)** principles, **Strongly Typed IDs**, and a highly secure **JWT + HTTP-Only Cookies** authorization flow. 

Built with **.NET 10**, **Entity Framework Core (PostgreSQL)**, and **Next.js (TypeScript)**.

## Features
* **Domain-Driven Design (DDD):** Clean, strictly decoupled architecture (Domain -> Application -> Infrastructure -> Presentation).
* **Strongly Typed IDs:** Prevents primitive obsession and entity misassociation.
* **Secure Authentication:** Uses short-lived Access Tokens and long-lived Refresh Tokens.
* **XSS & CSRF Protection:** Tokens are stored in `HttpOnly`, `Strict/Lax` cookies, completely invisible to client-side JavaScript.
* **Silent Token Refresh:** Next.js frontend interceptor automatically catches `401 Unauthorized` responses and seamlessly refreshes the token in the background.
* **Route Protection:** Next.js Middleware fully protects private routes (e.g., `/dashboard`).

---

## Architecture Explained

This project strictly follows the Dependency Inversion Principle. The core business logic is isolated and has no dependencies on external frameworks or databases.

1.  **Domain:** The heart of the software. Contains Entities, Value Objects (Strongly Typed IDs), and Repository Interfaces.
2.  **Application:** Contains use cases, DTOs, and Interfaces (e.g., `IJwtProvider`, `IPasswordHasher`). References *Domain*.
3.  **Infrastructure:** Implements the interfaces defined in the Application/Domain layers. Contains EF Core `AppDbContext`, BCrypt hashing, and JWT generation. References *Domain* and *Application*.
4.  **Presentation (Web API):** The ASP.NET Core API controllers. Handles HTTP requests, sets Cookies, and wires up Dependency Injection. References *Application* and *Infrastructure*.

---

## Getting Started

Follow these steps precisely to get the project running on your local machine.

## 1. Prerequisites
* Before you begin, ensure you have the following installed:
  * [Git](https://git-scm.com/downloads) - To clone the repository.
  * [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) - The backend framework.
  * [Node.js (LTS version)](https://nodejs.org/en/download/) & npm - The frontend runtime and package manager.
  * [PostgreSQL](https://www.postgresql.org/download/) - The relational database. Ensure it is running locally on the default port `5432`.

---

## 2. Clone the Repository
* Open your terminal and run:
```bash
git clone [https://github.com/YOUR_USERNAME/DotNet10-NextJs-DDD-Auth.git](https://github.com/YOUR_USERNAME/DotNet10-NextJs-DDD-Auth.git)
cd DotNet10-NextJs-DDD-Auth
```
`(Note: Replace YOUR_USERNAME with your actual GitHub username)`

---

## 3. Backend Setup (.NET 10)

### Step 3.1: Configure the Database Connection
* Navigate to the Presentation layer and open appsettings.json:
```bash
cd authproj-be
```
* Edit the appsettings.json file and update the PostgresConnection string with your local PostgreSQL password:
```json
"ConnectionStrings": {
  "PostgresConnection": "Host=localhost;Port=5432;Database=AuthDb;Username=postgres;Password=YOUR_POSTGRES_PASSWORD"
}
```
### Step 3.2: Install Entity Framework Core Tools
* You need the EF Core CLI tools to run database migrations. Run this globally:
```bash
dotnet tool install --global dotnet-ef
```
### Step 3.3: Apply Database Migrations
* Navigate back to the root of the backend folder (where your .sln file is, or one level above authproj-be) and run the migrations to generate your PostgreSQL tables:
### Ensure you are in the root directory containing both Infrastructure and authproj-be folders
```bash
# Ensure you are in the root directory containing both Infrastructure and authproj-be folders
dotnet ef database update --project Infrastructure --startup-project authproj-be
```
### Step 3.4: Run the API
* Start the backend server:
```bash
cd authproj-be
dotnet run
```
`The API will start running on http://localhost:5000. Keep this terminal window open.`

---

## 4. Frontend Setup (Next.js)
* Open a new terminal window and navigate to the frontend directory:
```bash
cd authproj-fe
```

### Step 4.1: Install Dependencies
* Install all required Node.js packages:
```bash
npm install
```
### Step 4.2: Configure Environment Variables
* Create a new file named .env.local in the root of the authproj-fe folder. Add the following line to point to your local .NET API:
```bash
NEXT_PUBLIC_API_URL=http://localhost:5000/api
```
### Step 4.3: Run the Development Server
* Start the Next.js application:
```bash
npm run dev
```
`The frontend will start running on http://localhost:3000.`

---

## Testing the Application
* Open your browser and navigate to http://localhost:3000.
* Click **"Create account"** and register a new user.
* Upon successful registration, the API will attach `HttpOnly` cookies to your browser and Next.js will redirect you to the protected `/dashboard`.
* **Test the Auto-Refresh:** The Access Token expires in 1 minute. Wait for 61 seconds on the dashboard, then refresh the page. You will see (in the Network tab) that the application seamlessly intercepts the `401` error, hits the `/refresh` endpoint, receives new cookies, and completes your request without logging you out.
* Click **"Log out"** to delete the cookies and be redirected back to the login screen. Try manually navigating to `/dashboard` via the URL bar to see the Next.js Middleware block you.

---

## Contributing
Contributions, issues, and feature requests are welcome! Feel free to check the issues page.

---

## License
This project is MIT licensed.
