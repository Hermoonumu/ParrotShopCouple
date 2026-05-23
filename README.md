# 🦜 Parrot Shop Couple - Features & Technology Stack

Based on the source code provided in the **ParrotShopCouple** repository, here is a detailed breakdown of the project's functionality and the technology stack used.

## 🚀 Core Functionality

The application is a full-stack e-commerce platform specifically designed for an online parrot shop. It includes both customer-facing features and administrative tools.

### Customer Features:
* **Parrot & Item Catalog:** Users can browse through a catalog of parrots and related shop items. Parrots have detailed traits (e.g., noise level, trainability, size, sociability).
* **Interactive Questionnaire (Quiz):** A unique feature where users can take a quiz to find the perfect parrot breed that matches their lifestyle and preferences. 
* **Shopping Cart & Checkout:** Users can add parrots and items to their cart, manage cart contents, and proceed through a secure checkout process.
* **User Accounts & Authentication:** Customers can register, log in, and manage their accounts. The system uses secure JWT authentication with refresh tokens to keep users logged in securely.
* **Order History:** Users can view their past orders and track their status.

### Admin/Management Features:
* **Admin Dashboard:** Role-based access control (using Admin Guards) ensures that only authorized administrators can access management areas.
* **Inventory Management:** Admins can add, update, or delete parrots and shop items (categories, traits, etc.).
* **Order Management:** Admins can view and process customer orders.
* **User Management:** Admins have the ability to view and manage registered user accounts.

---

## 🛠️ Technology Stack

The project follows a modern, decoupled client-server architecture.

### Backend (ParrotShopBackend)
The server-side is built using **Microsoft's .NET ecosystem** and follows Clean Architecture principles (separating API, Application, Domain, and Infrastructure layers).

* **Framework:** ASP.NET Core Web API (.NET C#)
* **Architecture:** Clean Architecture / N-Tier (Domain-Driven Design concepts)
* **Authentication & Security:** JWT (JSON Web Tokens) with Refresh Token rotation for secure, stateless API authentication.
* **Caching:** **Redis** (Distributed Cache) is integrated to improve performance (e.g., caching frequent queries or session data).
* **Data Access:** Entity Framework Core (inferred via `ShopContext` and repository patterns like `IUserRepository`, `IOrderRepository`).
* **Patterns Used:** Repository Pattern, Data Transfer Objects (DTOs), Global Exception Handling, and custom Validators.

### Frontend (ParrotShopFrontend)
The client-side is a Single Page Application (SPA) built with **Google's Angular framework**.

* **Framework:** Angular
* **Language:** TypeScript
* **Styling & Markup:** CSS3 and HTML5
* **Routing & Guards:** Angular Router with custom AuthGuards (`auth-guard`, `adminguard`, `checkout-guard`) to protect private and admin routes.
* **State Management/API Communication:** RxJS and Angular's `HttpClient` (managed via services like `auth-service`, `parrot-service`, and `checkout-service` with HTTP Interceptors).
* **Development Tools:** Node.js, Angular CLI, Prettier, and ESLint.
