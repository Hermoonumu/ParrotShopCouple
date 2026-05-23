# 🛍️ Parrot Shop with ASP.NET Core Web API

You can buy your own slave parrot right here!

## 🎯 Goal of this whatever I'm trying to build

- Practice C#
- Get gist of EF and Fluent API
- Mock Payments

## 🚀 Tech Stack

- **Framework:** ASP.NET Core 10 Web API
- **Database:** PostgreSQL (with Npgsql)
- **ORM:** Entity Framework Core (Code-First)
- **Docs:** Scalar
- **Auth:** JWT (JSON Web Tokens) with Refresh Tokens
- **Testing:** XUnit
- **API:** REST
- **Architecture:** Clean
- **Caching:** Redis
- **Automation:** Hangfire
- **RT updates:** SignalR (maybe)
- **Containerization:** Docker

## 🤡 Funny Code Dinguses

- **Bit packing:** The colours of a bird are stored as bit flags inside an int
- **Security:** JWT and password hashing (you may crack the table but frick you)

## ☑️ What's been achieved?

- Entites are established
- Set up database connection and migrations
- Registering
- Logging in (HttpCookie)
- DB Revoked Tokens Table cleanup with Hangfire

## 🚲 What's to do?

- User accounts (AUTH, managing, roles) **+**
- Shopping
  - As admin:
    - Manage store (add/remove prods, edit prods)
    - Edit images of the prods
    - Announce sales or whatever
  - As user:
    - Create a cart
    - Put items in cart
    - Checkout
    - Order tracking
    - Manage the account
- Some real endpoints **+**
- Logging
- Filtering of the products
- Mock Payments
- The "Select-a-parrot-by-answering-a-questionnaire" thingamajig
- Redis caching
- MAYBE SignalR (updates to a customer in real time)
- XUnit so that it doesn't break
- FluentValidation **+**
- Docker Containerization
- Scalar API docs **+**

## ¿ So ehhh

if you know c# and dis kinda stuff get me some halp
