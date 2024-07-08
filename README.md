# Task 1
-Design a database schema for a simple e-commerce platform with the following requirements:
- Users (UserId, Username, Password, Email)
- Products (ProductId, ProductName, Description, Price)
- Orders (OrderId, UserId, OrderDate, TotalAmount)
- OrderItems (OrderItemId, OrderId, ProductId, Quantity, Price)

```sh
   CREATE TABLE Users (
    UserId int IDENTITY PRIMARY KEY,
    UserName nvarchar(100) NOT NULL,
    Password varchar(20) NOT NULL,
    Email varchar(100) NOT NULL
);

CREATE TABLE Products (
    ProductId int IDENTITY PRIMARY KEY,
    ProductName nvarchar(200) NOT NULL,
    Description nvarchar(500) NOT NULL,
    Price decimal
);

CREATE TABLE Orders (
    OrderId int IDENTITY PRIMARY KEY,
    UserId int,
    OrderDate date,
    TotalAmount decimal,
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

CREATE TABLE OrderItems (
    OrderItemId int IDENTITY PRIMARY KEY,
    OrderId int,
    ProductId int,
    Quantity int NOT NULL,
    Price decimal NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId), 
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) 
);
```
