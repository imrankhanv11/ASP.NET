-- 1. Roles Table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

-- 2. Users Table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId INT NOT NULL,
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- 3. Categories Table
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL
);

-- 4. Products Table
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT NOT NULL,
    CONSTRAINT FK_Products_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- 5. Carts Table
CREATE TABLE Carts (
    CartId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL UNIQUE, -- One cart per user
    CONSTRAINT FK_Carts_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- 6. CartItems Table
CREATE TABLE CartItems (
    CartItemId INT PRIMARY KEY IDENTITY(1,1),
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY (CartId) REFERENCES Carts(CartId),
    CONSTRAINT FK_CartItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

-- 7. Orders Table
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    CONSTRAINT FK_Orders_Users FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

-- 8. OrderItems Table
CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);

Insert Into Roles (RoleName) Values ('SPAdmin'),('Admin'),('User');


INSERT INTO Categories (CategoryName)
VALUES 
('Electronics'),
('Clothing'),
('Books'),
('Home Appliances'),
('Sports');

INSERT INTO Products (Name, Description, Price, Stock, CategoryId)
VALUES
-- Electronics
('Smartphone', 'Latest 5G smartphone with 128GB storage', 35000.00, 50, 1),
('Laptop', '15-inch laptop with Intel i7 and 16GB RAM', 75000.00, 30, 1),
('Headphones', 'Noise cancelling wireless headphones', 5000.00, 100, 1),

-- Clothing
('T-Shirt', 'Cotton round-neck T-shirt', 500.00, 200, 2),
('Jeans', 'Slim fit denim jeans', 1500.00, 120, 2),

-- Books
('ASP.NET Core Guide', 'Learn ASP.NET Core step by step', 800.00, 75, 3),
('C# Programming', 'Complete reference for C# developers', 950.00, 60, 3),

-- Home Appliances
('Microwave Oven', '800W digital microwave oven', 9000.00, 40, 4),
('Refrigerator', 'Double-door frost free refrigerator', 25000.00, 25, 4),

-- Sports
('Cricket Bat', 'English willow cricket bat', 4500.00, 80, 5),
('Football', 'FIFA approved professional football', 1200.00, 150, 5);

