# SampleApi

# Overview
SampleApi is a simple CRUD API for managing users.

# Prerequisites
Docker Desktop installed on your machine.

1. Clone the repository to your local machine.
2. Navigate to the src directory.
3. Run the following command in your terminal: docker-compose up --build
4. Once the containers are up and running, access the API Swagger documentation at: http://localhost:5000/swagger/index.html

# Features
1. CRUD Operations for Users
2. Simple Exception Handling
3. Simple validation for email addresses, assuming they are unique in the database.
4. MongoDB is used as the primary database to showcase a modern stack.
5. Unit tests are implemented only for the UserService.
