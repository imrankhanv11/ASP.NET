create Database BookManagementSystem;

-- Roles table
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

-- Users table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL, -- store hashed password
    RoleId INT NOT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- Categories table
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(100) NOT NULL UNIQUE
);

-- Books table
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Author NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL,
    CategoryId INT NOT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Orders table
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    BookId INT NOT NULL,
    Quantity INT NOT NULL,
    OrderDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
);



-- Insert roles
INSERT INTO Roles (RoleName) VALUES 
('Admin'),
('User');

-- Insert users
INSERT INTO Users (Username, PasswordHash, RoleId) VALUES
('admin1', 'admin123hash', 1), -- Admin
('john_doe', 'user123hash', 2), -- User
('emma_watson', 'pass123hash', 2); -- User

-- Insert categories
INSERT INTO Categories (CategoryName) VALUES
('Fiction'),
('Science'),
('History'),
('Technology');

-- Insert books
INSERT INTO Books (Title, Author, Price, Stock, CategoryId) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', 350.00, 12, 1),
('To Kill a Mockingbird', 'Harper Lee', 299.00, 8, 1),
('A Brief History of Time', 'Stephen Hawking', 500.00, 5, 2),
('Sapiens: A Brief History of Humankind', 'Yuval Noah Harari', 450.00, 10, 3),
('Clean Code', 'Robert C. Martin', 600.00, 6, 4),
('The Pragmatic Programmer', 'Andrew Hunt', 550.00, 4, 4);

-- Insert orders
INSERT INTO Orders (UserId, BookId, Quantity) VALUES
(2, 1, 1), -- john_doe ordered The Great Gatsby
(2, 3, 1), -- john_doe ordered A Brief History of Time
(3, 4, 2), -- emma_watson ordered Sapiens
(3, 5, 1); -- emma_watson ordered Clean Code
