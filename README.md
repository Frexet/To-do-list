# 📝 To-Do List API with .NET 9 & SQLite 🚀

A **.NET 9 Web API** for managing a simple **To-Do List**, built with **Entity Framework Core** and **SQLite**.  
This API allows users to **create, update, retrieve, delete, and complete tasks**.

---

## 📌 **New Features**
### 🔥 **Task Completion**
- Added a **`PATCH /api/Tasks/{id}/complete`** endpoint to mark a task as completed.
- The **`TaskItem` model now includes an `IsCompleted` property**.
- This enables users to track whether a task is done.

### 🔥 **Database Improvements**
- Updated **SQLite schema** to include the `IsCompleted` field.
- **Migrations were updated** to reflect the new database structure.

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