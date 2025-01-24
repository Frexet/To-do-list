# 📝 To-Do List API with .NET 9 & SQLite 🚀

A powerful **.NET 9 Web API** for managing a simple **To-Do List**, built with **Entity Framework Core** and **SQLite**.  
This API supports **CRUD operations**, tracks tasks marked as completed, and integrates advanced features like project pausing and tag management.

---

## 📌 **Features**
### 🔥 **Task Management**
- **CRUD operations** for tasks.
- **`PATCH /api/Tasks/{id}/complete`**: Mark a task as completed.
- Tracks deadlines and completion status (`IsCompleted`).

### 🔥 **Project Management**
- **CRUD operations** for projects.
- Manage related tasks and tags for each project.
- Pause and activate projects with the `IsPaused` property.

### 🔥 **Tag Management**
- **CRUD operations** for tags.
- Associate tags with tasks and projects.

### 🔥 **Database Integration**
- SQLite database with migrations for managing schema.
- Automated schema updates with Entity Framework Core.
- Tables include `Projects`, `TaskItems`, and `Tags` with relationships.

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