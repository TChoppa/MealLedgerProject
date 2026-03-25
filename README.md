# 🚀 MealLedger – Smart Lunch Management System

## 📌 Overview
**MealLedger** is a web-based internal tool designed to streamline and automate employee lunch registration, replacing traditional Excel-based tracking systems.

It ensures a smooth, efficient, and error-free process for managing daily meal participation within an organization.

---

## 🎯 Problem Statement
Many organizations rely on Excel sheets for lunch registration, which leads to:

- ❌ Manual errors and duplicate entries  
- ❌ No enforcement of cutoff time  
- ❌ Lack of real-time visibility  
- ❌ No proper tracking or reporting  
- ❌ Time-consuming management process  

---

## 💡 Solution
MealLedger solves these challenges by providing:

- ✅ Automated lunch registration system  
- ✅ Cutoff time enforcement (e.g., 4 PM for next day lunch)  
- ✅ Role-based access control (Admin & Users)  
- ✅ Centralized and structured data management  
- ✅ Export functionality for reporting  

---

## ⚙️ Features

### 👤 User Features
- Register for lunch easily  
- View registration status  
- Simple and intuitive UI  

### 🛠️ Admin Features
- View all registered users  
- Filter data (day-wise / week-wise)  
- Export reports to:
  - 📄 PDF  
  - 📊 Excel  
- Manage user data efficiently  

---

## 🔐 Authentication
- Login using **Workday ID**
- No password required (internal organizational tool)
- Pre-registered users stored in database

---

## 🧰 Tech Stack

- **Frontend:** HTML, CSS, JavaScript  
- **Backend:** ASP.NET MVC / ASP.NET Core  
- **Database:** SQL Server (MDF - Local DB)  
- **Architecture:** MVC Pattern  

---

## 🗄️ Database
- Uses **SQL Server LocalDB (.mdf file)**  
- Stores:
  - User details  
  - Lunch registrations  
- Easily scalable to cloud database (Azure SQL, etc.)

---

## 📊 Key Highlights
- 🔄 Replaced manual Excel workflow  
- ⏱️ Saves time for employees & admins  
- 📈 Improves accuracy and efficiency  
- 🧩 Modular and scalable design  

---

## 🚀 Future Enhancements
- Email notifications for reminders  
- Mobile-friendly UI  
- Cloud deployment (Azure)  
- Analytics dashboard  
- Multi-location support  

---

## ⚡ Getting Started

### Prerequisites
- Visual Studio  
- SQL Server / LocalDB  

### Setup Steps
1. Clone the repository
   ```bash
   https://github.com/TChoppa/MealLedgerProject.git
