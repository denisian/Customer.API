# Customer.Api

![Swagger](images/API%20endpoints.png)

## This API exposes the following endpoints
  - Contact information
    - Get a specific contact: GET /api/v{version}/customers/{customerId}/contacts/{contactId}
    - Get all contacts: GET /api/v{version}/customers/{customerId}/contacts
  - Customer Information
    - Get a specific customer: GET /api/v{version}/customers/{customerId}
    - Get all customers: GET /api/v{version}/customers
    - Update an exising customer: PUT /api/v{version}/customers/{customerId}
  - Note information
    - Get a specific note: GET /api/v{version}/customers/{customerId}/notes/{noteId}
    - Get all notes: /api/v{version}/customers/{customerId}/notes
    - Update an existing note: PUT /api/v{version}/customers/{customerId}/notes/{noteId}
    - Delete and exising note: DELETE /api/v{version}/customers/{customerId}/notes/{noteId}

The default APi version is 1.
All identifiers are numbers.

## How to run application
  - Open solution in Visual Studio 2019
  - Update connection string in appsettings.json by setting password that sent by email
  - Run the project
  - Use Swagger UI or Postman to send requests
  
## Database
  - I use SQL Server database hosted on Azure
  - Currently the database server connection is public and you can access to the server **sqltstauckl.database.windows.net** using SSMS or Azure Data Studio with user/password from the connection string
  
  ### Tables structure
  
  ![ER diagram](images/ER%20diagram.png)
  
  ``` sql 
  CREATE TABLE Status
(
	StatusID TINYINT IDENTITY(1,1) NOT NULL
	,Description VARCHAR(30) NOT NULL
	,CreatedDateTimeUTC DATETIME NOT NULL
	CONSTRAINT DF_Status_CreateDateTimeUTC DEFAULT (SYSUTCDATETIME())
	,UpdatedDateTimeUTC DATETIME NULL
	,CONSTRAINT PK_Status PRIMARY KEY CLUSTERED (StatusID ASC)
)

CREATE TABLE Customer
(
	CustomerID INT IDENTITY(1,1) NOT NULL
	,StatusID TINYINT NOT NULL
	,FirstName VARCHAR(30) NULL
	,LastName VARCHAR(60) NOT NULL
	,CreatedDateTimeUTC DATETIME NOT NULL
	CONSTRAINT DF_Customer_CreateDateTimeUTC DEFAULT (SYSUTCDATETIME())
	,UpdatedDateTimeUTC DATETIME NULL
	,CONSTRAINT PK_Customer PRIMARY KEY CLUSTERED (CustomerID ASC)
	,CONSTRAINT FK_Customer_Status_StatusID FOREIGN KEY (StatusID) REFERENCES Status(StatusID)
)

CREATE TABLE Contact
(
	ContactID INT IDENTITY(1,1) NOT NULL
	,CustomerID INT
	,Type CHAR(2) NOT NULL
	,Detail VARCHAR(60) NOT NULL
	,CreatedDateTimeUTC DATETIME NOT NULL
	CONSTRAINT DF_Contact_CreateDateTimeUTC DEFAULT (SYSUTCDATETIME())
	,UpdatedDateTimeUTC DATETIME NULL
	,CONSTRAINT FK_Contact_Customer_CustomerID FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)

CREATE TABLE Note
(
	NoteID INT IDENTITY(1,1) NOT NULL
	,CustomerID INT
	,Detail VARCHAR(200) NOT NULL
	,CreatedDateTimeUTC DATETIME NOT NULL
	CONSTRAINT DF_Note_CreateDateTimeUTC DEFAULT (SYSUTCDATETIME())
	,UpdatedDateTimeUTC DATETIME NULL
	,CONSTRAINT FK_Note_Customer_CustomerID FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
)
 ```

## Notes
I spent on this project about 6-7 hours including
  - Design a data model
  - Build an app
  - Set up and configuration SQL db on Azure
  - Testing all endpoints with mix of data
  - Writing this description :)
  
 What I missed/would add
  - Logging. We should add Serilog and logging middleware to store all requests/responses in log files/Elasticsearch. I would also send a message to TEams/Slack channel if any production issues occured.
  - Validation. Currently, there is almost no a proper request validation. I would go with some custom implementation or use FluentValidation.
  - There is no API authentication and authorisation. Ideally we shoud have an IdentityServer and probably database permissions to limit the access scope for the client.
  - I created only a single Unit test however we need to cover all possible scenarios with all endpoints. Integration tests wouls be also an advantage.
  - Thinking about possible performance issues, we will need to add non-clustered indexes on some heavily used columns.
  
  
