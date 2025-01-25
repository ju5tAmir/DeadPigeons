# DeadPigeons  
![DeadPigeons Banner](https://github.com/user-attachments/assets/b89b40cf-25a1-4e2d-8952-573d943c1d06)

**DeadPigeons** is a web-based solution for managing and scaling the “Dead Pigeons” game, a fundraising activity for the local sports club, Jerne IF. The platform digitizes the game, enabling seamless user participation and management while maintaining support for offline components.  

---

## Stack & Technologies  

The **DeadPigeons** platform is built using a modern tech stack designed for scalability, maintainability, and flexibility:  

- **.NET & Entity Framework**: The backend is built using **ASP.NET Core** for the web API, with **Entity Framework Core** for managing the database and simplifying data access.  
- **ASP.NET Core Identity**: User authentication and authorization are managed with **ASP.NET Core Identity**, providing secure login, registration, and role-based access control.  
- **React & TypeScript**: The frontend is built with **React**, utilizing **TypeScript** for type safety and improved developer experience. The UI is styled with **Tailwind CSS** for a modern, responsive design.  
- **Google Cloud**: The application is deployed on **Google Cloud**, providing a reliable and scalable infrastructure for handling user requests and transactions.

This stack ensures that both the frontend and backend are highly performant and secure, allowing for a smooth and responsive user experience.

---
## User Panel  

### Landing Page & Games Overview  
Users can view the available games for a selected year through a dropdown:  
![Games Overview](https://github.com/user-attachments/assets/116307ef-fc6d-4675-9341-25e6b30c5e2e)  

### Play Game  
Players can participate in active games by selecting numbers to play. The total price adjusts dynamically based on the number of chosen numbers and package prices:  
![Play Game](https://github.com/user-attachments/assets/c453ec05-2b95-4e68-a907-3c47c96e41d3)  
**Increased numbers view:**  
![Increased Numbers](https://github.com/user-attachments/assets/6ebc2a98-0ad7-4ccd-b6ca-eb045ac8da8e)  

### Transactions  
Players can track their transactions:  
![Transactions](https://github.com/user-attachments/assets/9c724549-65c9-4712-ae21-d66e90f66232)  

### Transaction Tickets  
Detailed transaction receipts are also available:  
![Transaction Ticket](https://github.com/user-attachments/assets/f4696952-6f3a-4579-8f8f-ec875370a8d2)  

### Profile  
Users can update their profile and view their activity:  
![Profile](https://github.com/user-attachments/assets/6620cbfe-1ea9-4db7-afbc-e13f4fe66dc3)  

---

## Admin Panel  

### Landing Page & Games Overview  
Admins can view an overview of games for each year:  
![Admin Games Overview](https://github.com/user-attachments/assets/b441e2df-e980-4d8a-b4f2-aaaf34e80498)  

By clicking on a specific game, admins can access detailed information:  
![Admin Game Details](https://github.com/user-attachments/assets/e9bd4858-c46f-4ff3-9f05-accebe7dcdd5)  

Admins can also add offline data for a more accurate representation:  
![Offline Data](https://github.com/user-attachments/assets/922f7ed1-40c8-4229-8807-4a772c0606dd)  

### User Management  
Admins have full CRUD functionality for managing users:  
![User Management](https://github.com/user-attachments/assets/2ee5e997-75be-4580-b7b0-7b6eb54e495e)  

By clicking on a user, admins can view detailed information about their activities and transactions:  
![User Details](https://github.com/user-attachments/assets/33cf732f-451e-4555-993f-99bf34edeaa8)  

Admins can also update user statuses, make them active/inactive, and edit other details:  
![Edit User](https://github.com/user-attachments/assets/866b2d81-2451-40b0-98da-bedadbf60ee8)  

### Transactions  
Admins can view and manage all transactions made by users:  
![Transactions](https://github.com/user-attachments/assets/1e5ad2a7-57af-45e2-9d26-0cfa059bcd71)  

Transactions can be approved or declined. For approval, admins can select the appropriate amount based on the transaction image:  
![Transaction Approval](https://github.com/user-attachments/assets/ead55da4-3dde-47b6-afc1-e3b331106ea7)  

Admins can also send or reduce funds directly from user accounts:  
![Send/Reduce Funds](https://github.com/user-attachments/assets/1592f859-2777-430a-934d-43d0f1eb7be5)  

By entering user details (ID, email, or phone), admins can target specific accounts:  
![Target Account Info](https://github.com/user-attachments/assets/5dfe0176-b703-4073-8d5a-7be90b2f18b5)  

### Transaction Tickets  
Admins can generate detailed transaction tickets:  
![Transaction Ticket](https://github.com/user-attachments/assets/f4696952-6f3a-4579-8f8f-ec875370a8d2)  

### Profile  
Admins can view and update their profiles:  
![Profile](https://github.com/user-attachments/assets/6620cbfe-1ea9-4db7-afbc-e13f4fe66dc3)  

---

