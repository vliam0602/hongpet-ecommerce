# HongPet E-commerce Platform

HongPet is an e-commerce platform dedicated to pet products. This project implements a full-featured online store with product catalog, shopping cart, user authentication, and order management. Built with a modern tech stack including React for admin frontend and ASP.NET MVC for customer frontend, with a .NET backend following clean architecture principles.

## 📑 Table of Contents
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Technology Stack](#️-technology-stack)
- [Testing](#-testing)
- [License](#-license)

## ✨ Features

| Feature Category       | Features                                                                 |
|-------------------------|-------------------------------------------------------------------------|
| **User & Authentication** | ✅ User registration and account management<br>✅ JWT token-based authentication with refresh tokens<br>✅ Role-based authorization (Admin/Customer)<br>✅ User profile management with avatar support<br>✅ Secure password handling and reset functionality |
| **Product Management** | ✅ Comprehensive product catalog with categories and subcategories<br>✅ Product variants with customizable attributes (size, color, etc.)<br>✅ Multiple product images support<br>✅ Category navigation and hierarchical browsing |
| **Shopping Experience** | ✅ Shopping cart functionality<br>✅ Order creation and checkout process<br>✅ Multiple payment method options<br>✅ Order history<br>❌ Payment process<br>❌ Shipping address management |
| **Review System**       | ✅ View reviews and ratings of products<br>❌ Customer add product reviews and ratings<br>❌ Review moderation capabilities for administrators |
| **Admin Dashboard**     | ✅ User management interface<br>✅ Product and category management<br>✅ Order processing and status management<br>❌ Analytics and reporting capabilities |

## 🏗️ Project Structure

| Project | Component | Description |
|---|---|---|
| [HongPet.Backend](./HongPet.Backend/) |  [HongPet.Domain](./HongPet.Backend/src/HongPet.Domain/) | Contains core domain entities, DTOs, enums, and repository abstractions. |
|  | [HongPet.Application](./HongPet.Backend/src//HongPet.Application/) | Contains business logic and service implementations. |
|  | [HongPet.Infrastructure](./HongPet.Backend/src/HongPet.Infrastructure/)    | Handles database access using Entity Framework Core. |
| | [HongPet.SharedViewModels](./HongPet.Backend/HongPet.SharedViewModels/)  | Shared models for communication between layers. |
|  | [HongPet.Migrators.MSSQL](./HongPet.Backend/src/HongPet.Migrators.MSSQL/)   | Database migrations for SQL Server. |
|   | [HongPet.WebApi](./HongPet.Backend/src/HongPet.WebApi/) | Provides APIs for the customer and admin sites, handles business logic, and manages data persistence. |
|  | [Testing](HongPet.Backend/test/) | Unit tests using xUnit, Moq, and AutoFixture to ensure backend reliability. |
| [HongPet.Customer Site](./HongPet.CustomerSite/HongPet.CustomerMVC/) | [ASP.NET MVC Controllers](./HongPet.CustomerSite/HongPet.CustomerMVC/Controllers/) | Handles user requests and redirects views for the customer site.                                 |
|  | [Views](./HongPet.CustomerSite/HongPet.CustomerMVC/Views/) | Razor views for displaying product catalog, shopping cart, and user profile. |
|  | [Models](./HongPet.CustomerSite/HongPet.CustomerMVC/Models/) | Models for data binding between the views and MVC controllers |
| [HongPet.AdminSite](./HongPet.AdminSite/) | [React Components](./HongPet.AdminSite/src/components/) | UI components for managing users, products, categories, and orders. |
|   | [React Pages](./HongPet.AdminSite/src/pages/)  | Contains individual pages for the admin site, such as `Dashboard`, `Login`, and feature-specific pages like `Products`, `Categories`, `Orders`, and `Customers`. Each page handles its respective functionality and integrates with backend APIs. |
|   | [API Integration](./HongPet.AdminSite/src/services/)  | Communicates with backend APIs for data retrieval and updates. |

## 🛠️ Technology Stack

### Backend
- .NET 9 Web API
- Entity Framework Core with SQL Server
- Clean Architecture design
- AutoMapper for object mapping
- JWT Authentication
- xUnit, Moq, and AutoFixture for testing

### Frontend
- **Customer Site:** ASP.NET MVC, Bootstrap 5
- **Admin Dashboard:** ReactJS, TailwindCSS 4

## 🧪 Testing

The backend project are thoroughly unit tested to ensure reliability.


View the [test directory](./HongPet.Backend/test) for detailed information about test coverage and quality metrics.

## 📄 License

This project is under a [custom license](./LICENSE).

🔒 You are **not allowed to use, copy, or redistribute** this source code without permission.  
📬 Contact for usage permissions: v.trclam@gmail.com