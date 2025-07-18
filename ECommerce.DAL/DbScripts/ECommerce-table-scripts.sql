 CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100),
    Email NVARCHAR(200) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    PasswordHash NVARCHAR(500) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    IsActive BIT DEFAULT 1
);

CREATE TABLE UserRoles (
    UserId INT,
    RoleId INT,
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

 CREATE TABLE Categories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    ParentCategoryId INT NULL,
    FOREIGN KEY (ParentCategoryId) REFERENCES Categories(Id)
);

CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    SKU NVARCHAR(100),
    StockQuantity INT NOT NULL,
    CategoryId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

CREATE TABLE ProductImages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    ImageUrl NVARCHAR(500),
    IsPrimary BIT DEFAULT 0,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE ProductReviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ProductId INT,
    Rating INT NOT NULL,
    Comment NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE ProductTags (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    Tag NVARCHAR(100),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

 CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    OrderStatus NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2),
    ShippingAddressId INT,
    BillingAddressId INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ShippingAddressId) REFERENCES Addresses(Id),
    FOREIGN KEY (BillingAddressId) REFERENCES Addresses(Id)
);

CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    UnitPrice DECIMAL(18,2),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Payments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    PaymentMethod NVARCHAR(50),
    PaymentStatus NVARCHAR(50),
    PaymentDate DATETIME,
    TransactionId NVARCHAR(200),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);

 CREATE TABLE Addresses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    AddressLine1 NVARCHAR(200),
    AddressLine2 NVARCHAR(200),
    City NVARCHAR(100),
    State NVARCHAR(100),
    Country NVARCHAR(100),
    PostalCode NVARCHAR(20),
    IsBilling BIT DEFAULT 0,
    IsShipping BIT DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

 CREATE TABLE CartItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ProductId INT,
    Quantity INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

CREATE TABLE Wishlists (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ProductId INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

 CREATE TABLE AuditLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    Action NVARCHAR(100),
    EntityName NVARCHAR(100),
    EntityId INT,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    Timestamp DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Warehouses (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Location NVARCHAR(500)
);


CREATE TABLE StockInventory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    WarehouseId INT,
    Quantity INT,
    FOREIGN KEY (ProductId) REFERENCES Products(Id),
    FOREIGN KEY (WarehouseId) REFERENCES Warehouses(Id)
);


CREATE TABLE SalesDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT,
    ProductId INT,
    Quantity INT,
    Price DECIMAL(18,2),
    Discount DECIMAL(18,2),
    TotalAmount DECIMAL(18,2),
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);


CREATE TABLE Tags (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE
);



ALTER TABLE ProductTags
ADD TagId INT;

ALTER TABLE ProductTags
ADD CONSTRAINT FK_ProductTags_Tags FOREIGN KEY (TagId) REFERENCES Tags(Id);