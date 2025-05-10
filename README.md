# HongPet E-commerce Platform

## 📑 Table of Contents
- [About](#-about)
- [Features](#-features)
  - [User & Authentication](#user--authentication)
  - [Product Management](#product-management)
  - [Shopping Experience](#shopping-experience)
  - [Review System](#review-system)
  - [Admin Dashboard](#admin-dashboard)
- [Technology Stack](#️-technology-stack)

- [Testing](#-testing)
- [License](#-license)

## 📋 About

HongPet is an e-commerce platform dedicated to pet products. This project implements a full-featured online store with product catalog, shopping cart, user authentication, and order management. Built with a modern tech stack including React for admin frontend and ASP.NET MVC for customer frontend, with a .NET backend following clean architecture principles.

## ✨ Features

### User & Authentication
- ✅ User registration and account management
- ✅ JWT token-based authentication with refresh tokens
- ✅ Role-based authorization (Admin/Customer)
- ✅ User profile management with avatar support
- ✅ Secure password handling and reset functionality

### Product Management
- ✅ Comprehensive product catalog with categories and subcategories
- ✅ Product variants with customizable attributes (size, color, etc.)
- ✅ Multiple product images support
- ✅ Category navigation and hierarchical browsing

### Shopping Experience
- ✅ Shopping cart functionality
- ✅ Order creation and checkout process
- ✅ Multiple payment method options
- ✅ Order history
- ❌ Payment process
- ❌ Shipping address management

### Review System
- ✅ View reviews and ratings of product
- ❌ Customer add product reviews and ratings
- ❌ Review moderation capabilities for administrators

### Admin Dashboard
- ✅ User management interface
- ✅ Product and category management
- ✅ Order processing and status management
- ❌ Analytics and reporting capabilities

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

The backend services are thoroughly unit tested to ensure reliability.


View the [test coverage report](https://github.com/vliam0602/hongpet-ecommerce/tree/main/HongPet.Backend/test) for detailed information about test coverage and quality metrics.

## 📄 License

This project is under a [custom license](./LICENSE).

🔒 You are **not allowed to use, copy, or redistribute** this source code without permission.  
📬 Contact for usage permissions: v.trclam@gmail.com