# DotCreate CLI Tool

DotCreate is a CLI tool designed to streamline the process of setting up ASP.NET Core projects. Inspired by tools like create-react-app, DotCreate simplifies the project setup process by providing easy-to-use commands and options.

## Installation

To install DotCreate globally on your system, use the following command:

```bash
dotnet tool install -g DotCreate

Usage
Create a New Project
To create a new ASP.NET Core project, run:

bash
Copy code
dc create-app
Follow the prompts to select options such as project type (MVC or WebAPI), and additional features like jQuery, Bootstrap, Dapper ORM, and Entity Framework ORM.

Features
Project Types: Choose between ASP.NET Core MVC or WebAPI.
Optional Features:
jQuery
Bootstrap
Dapper ORM
Entity Framework ORM
Automatic Setup:
Automatically adds connection strings to appsettings.json.
Adds IDbConnection service to the DI container in Program.cs.
Class Libraries:
Follows Single Responsibility Principle by creating class libraries for Models, Repositories, Managers, and Common utilities.
Auto-adds references of these libraries to the main project.
Contributing
Contributions are welcome! If you have any suggestions, feature requests, or bug reports, please open an issue.

License
This project is licensed under the MIT License.

sql
Copy code

Copy and paste this content into your README.md file, and it should display correctly with Markdown formatting when viewed in platforms like GitHub. Let me know if you need any further assistance!





