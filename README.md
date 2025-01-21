# 📝 To-Do List API with .NET 9 & SQLite 🚀

A powerful **.NET 9 Web API** for managing a simple **To-Do List**, built with **Entity Framework Core** and **SQLite**.  
This API supports **CRUD operations** and tracks tasks marked as completed.

---

## 📌 **New Features**
### 🔥 **Task Completion**
- **`PATCH /api/Tasks/{id}/complete`**: A new endpoint to mark a task as completed.
- **`TaskItem` model updated** with an `IsCompleted` property to track task completion.

### 🔥 **Database Improvements**
- Updated SQLite schema to include the `IsCompleted` field.
- **Migrations adjusted** to reflect the new database structure.

---

## 🚀 **Getting Started**

### 🔧 **Prerequisites**
- .NET 9 SDK ([Download Here](https://dotnet.microsoft.com/download/dotnet/9.0))
- Visual Studio / VS Code
- Git

### 🏗 **Setup**
1. **Clone this repository:**
   ```sh
   git clone https://github.com/Frexet/To-do-list.git
   cd To-do-list