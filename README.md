# ArribaEats - C# Food Delivery Management System

![Project Status](https://img.shields.io/badge/Status-Finished-brightgreen)
![Language](https://img.shields.io/badge/Language-C%23-blue)
![Platform](https://img.shields.io/badge/.NET-9.0-purple)

A robust, console-based food delivery ecosystem designed to handle real-time interactions between **Customers**, **Restaurants (Clients)**, and **Deliverers**. This project demonstrates core Object-Oriented Programming (OOP) principles and efficient state management in C#.

## 🚀 System Overview

ArribaEats acts as a three-sided marketplace, managing the complex lifecycle of a food order from placement to doorstep.

### Key Features:
* **Role-Based Access:** Dedicated menus and logic for Customers, Clients, and Deliverers.
* **Dynamic Menu Management:** Restaurants can add items and update prices in real-time.
* **Order Lifecycle Tracking:** A state-driven system that monitors if an order is "Cooking," "Ready," "Picked Up," or "Delivered."
* **Logistics Engine:** Calculates travel distances (Manhattan Distance) to help Deliverers choose the most efficient jobs.
* **Singleton Managers:** Uses a centralized `UserManager` and `OrderManager` to ensure data consistency across all menus.

---

## 🏗️ Technical Architecture

### The Order State Machine
The core of the application is the `Order` model, which uses boolean flags to track the physical progress of the food.



1.  **Ordered**: Order is created and visible to the Restaurant.
2.  **Cooking**: Restaurant sets `StartedCooking = true`.
3.  **Ready**: Restaurant sets `IsReadyForPickup = true`.
4.  **Arrived**: Deliverer signals arrival via `DelivererArrived = true`.
5.  **Delivered**: Final state reached when `Delivered = true`.

### Smart Distance Calculation
To simulate a real-world grid, the system uses the **Manhattan Distance** formula to calculate the effort required for a delivery:
$$Distance = |x_1 - x_2| + |y_1 - y_2|$$



---

## 🛠️ Tech Stack & Concepts
* **C# / .NET 9.0**
* **LINQ:** For high-performance filtering of orders and user records.
* **Singleton Pattern:** To maintain a global state without passing objects through every constructor.
* **Encapsulation:** Properties and private backing fields to protect data integrity.

---

## 💻 How to Run
1. Ensure you have the **.NET 9.0 SDK** installed.
2. Clone this repository.
3. Open a terminal in the project root.
4. Run the following command:
   ```bash
   dotnet run
