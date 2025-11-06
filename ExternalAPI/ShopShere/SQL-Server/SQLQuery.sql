Create Database ShopSphere;

-- 1. Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- 2. Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId INT NOT NULL,
    CreatedAt Date Not NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- 3. Products Table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL
);

-- 4. Carts Table
CREATE TABLE Carts (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE, 
    CONSTRAINT FK_Carts_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- 5. CartItems Table
CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartId) REFERENCES Carts(CartId),
    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);


-- 6. Add Roles in Roll Table
INSERT INTO Roles (RoleName) VALUES ('Admin'), ('User');