## Prerequisites

Before you begin, ensure you have the following installed:
- **Visual Studio 2022**
- **.NET Core 8 LTS**

You will also need to set up your database:
- **MSSQL** - Make sure it is installed and running on your system.

## Getting Started

1. **Clone the Repository**
   - Start by cloning the project to your local machine. 
   - Open the project using **Visual Studio 2022**.

2. **Configure the `appsettings.json` File**
   - Navigate to the `appsettings.json` file.
   - Edit the `ConnectionStrings` section to match your system database settings.
   - Feel free to name the database as you wish.

3. **Install Necessary Packages**
   - Go to **Nuget Package Manager Solution** in Visual Studio.
   - Install the following packages:
     - `Microsoft.EntityFrameworkCore` (for general EF Core functionality)
     - `Microsoft.EntityFrameworkCore.SqlServer` (for SQL Server support)
     - `Microsoft.EntityFrameworkCore.Tools` (for EF Core tools)

4. **Set Up the Database**
   - Open the **Nuget Package Manager Console**.
   - Run the following command to create the database and all relevant tables:
     ```
     update-database
     ```

## Running the Project

- After completing the setup, you can run the project by pressing `F5` or using the **Start** button in Visual Studio.
- This will launch the web application in your default web browser.

