# 🛍️ CuaHangVHT - ASP.NET Core MVC Fashion E-commerce Website

This is a complete, server-rendered e-commerce web application built with **ASP.NET Core 8 MVC**. It demonstrates a robust, traditional approach to building web applications where the user interface is generated directly on the server using Razor Views.

![CuaHangVHT Demo](https://res.cloudinary.com/dqwxudyzu/image/upload/v1717866579/z5536413444498_e9f55a15a0c841c2d28a38fd37c86a4e_hpg4u6.gif)

---

## 🌟 Key Features

* **For Users:**
    * User registration, login, and profile management with ASP.NET Core Identity.
    * Browse, search, and filter products by category.
    * Shopping cart management (add, remove, update quantity).
    * Place orders and view personal order history.
* **For Administrators (Admin):**
    * Overview dashboard.
    * Product management (CRUD operations).
    * Category management.
    * Manage customer orders.
    * Manage user accounts.

---

## 🛠️ Technology Stack

| Component         | Technology                                                                                                |
| :---------------- | :-------------------------------------------------------------------------------------------------------- |
| **Framework** | **ASP.NET Core 8 MVC** |
| **UI** | **Razor Views**, HTML, CSS, Bootstrap, JavaScript/jQuery                                                  |
| **Data Access** | **Entity Framework Core 8** |
| **Architecture** | **Model-View-Controller (MVC)**, Clean Architecture Principles, Repository Pattern                        |
| **Database** | **Microsoft SQL Server** |
| **Authentication**| ASP.NET Core Identity, Cookie-based authentication                                                        |
| **Tools** | Visual Studio 2022, Git                                                                                   |

---

## 🏛️ Architecture

The project is built using the classic **Model-View-Controller (MVC)** architectural pattern, which provides a clear separation of concerns:

* **Model:** Represents the application's data and business logic, managed by Entity Framework Core.
* **View:** Renders the user interface for the application using **Razor Views**, combining HTML with C# code.
* **Controller:** Acts as an interface between Model and View, handling user input, interacting with the data, and rendering the appropriate response.

Principles of **Clean Architecture** are applied to further separate business logic from infrastructure concerns, making the application more maintainable and testable.

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Setup and Installation

```bash
# 1. Clone the repository
git clone [https://github.com/Vohoangtuanhdhtdm/CuaHangVHT.git](https://github.com/Vohoangtuanhdhtdm/CuaHangVHT.git)

# 2. Navigate to the project directory
cd CuaHangVHT

# 3. Configure the database connection string
#    Open `appsettings.json` and update the `DefaultConnection` string
#    to match your SQL Server configuration.

# 4. Apply migrations to create the database
#    (Run this command from the directory containing the .sln file)
dotnet ef database update

# 5. Run the application
dotnet run
```

---

## 📸 Screenshots

Homepage                                                                                                                                                                                                          

 ![Homepage](https://res.cloudinary.com/dqwxudyzu/image/upload/v1749447614/cuaHangVHT_MVC_fm4hoj.png) 
