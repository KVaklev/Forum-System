# Forum Management System

## Project Description

The Forum Management System is a web application designed to facilitate discussions and interactions among users on a specific topic. The system allows users to create posts, add comments, upvote/downvote content, and perform various other actions related to forum management. The project focuses on providing essential features such as user registration, authentication, role-based access control, post management, comment management, and administrative capabilities.

## Features

User registration and authentication
Role-based access control (user and admin roles)
Create, read, update, and delete posts
Add/Edit/Delete comments to posts
Upvote/downvote posts and comments
Sorting and filtering of posts, users, comments
User profile management - edit personal information, upload profile picture
Admin capabilities for user management and content moderation

## Technologies Used

Front-end: HTML, CSS, JavaScript
Back-end: ASP.NET Core 
IDE: Visual Studio 2022
Database: Microsoft SQL Server Management Studio
REST API: ASP.NET Core Web API
Unit Testing: MSTest
Documentation: Swagger

## How to Set Up and Run the Project Locally - follow these steps:

1. Clone the GitLab Repository
2. Install Dependencies and Packages
3. Ensure that you have Microsoft SQL Server Management Studio installed and running on your local machine. 
4. Configure the Connection String - locate the configuration file where the database connection string is stored. It is usually found in a configuration file like appsettings.json. 
	Update the connection string with the appropriate details for your SQL Server database. 
	Provide the server name, database name, connection.=> "DefaultConnection":"Server=....;Database=....;Trusted_Connection=True;"
4. Build and Run the Project - use the appropriate commands to build and run the project.
5. Access the Application - once the project is running locally, you can access the application by opening a web browser and entering the appropriate URL.

## Database Relations

![Diagram](https://gitlab.com/forum-system-group-8/forum-system/-/blob/main/Diagram.jpg)

# About the Project 

## Project Hierarchy and Entity Descriptions

| Layer 	  | Class Libraries  | Description                                                                                                |
|-----------------|------------------|------------------------------------------------------------------------------------------------------------|
| 1. Business     | Dto              | Contains Data Transfer Objects used to transfer data between different layers of the application.          |
|       	  | Exceptions       | Includes custom exception classes for handling specific business logic errors or exceptional situations.   |                                                                    
|       	  | Mapper           | Provides mapping functionality to map objects between different layers or models.                          |
|       	  | QueryParameters  | Defines query parameters for filtering, sorting, and pagination in data retrieval operations.              |
|       	  | Services         | Implements business logic and handles the interaction between the presentation layer and data access layer.|                                                        
|      		  | ViewModels       | Contains view models used for presenting data to the user interface.                                       |
|                 |                  |                                                                                                            |
| 2. DataAccess   | Models	     | Contains data models representing the entities stored in the database - User, Category, Post, Comment, etc.|
|		  | Repositories     | Implements the repository pattern to encapsulate data access logic for each model.                         |
|		  |	-Contracts   | - Defines interfaces for the repositories, specifying the available operations.                            |
|		  |	-Context     | - Represents the database context and provides access to the database using an ORM  tool.                  |
|		  |	-Models      | - Contains entity models mapped to database tables.                                                        |
|                 |                  |                                                                                                            |
| 3. Presentation |wwwroot	     | Stores static files such as CSS, JavaScript, and image files used by the presentation layer.               |     
|		  |Controllers       | Handles requests from the client-side and coordinates the flow of data between the layers.                 |
|                 |     -Api         | - Contains controllers that implement the RESTful API endpoints for the application.                       |
|                 |     -MVC         | - Contains controllers that handle server-side rendering of views.                                         |
|		  |Views             | Contains the views responsible for rendering the user interface and presenting data to the end-user.       |
|		  |Helpers           | Provides helper classes or methods that assist in rendering views or performing other related tasks.       |

## Credits

This project was created with the assistance of:

- Marina Toncheva
- Kristian Vaklev
- Evelina Hristova 

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.