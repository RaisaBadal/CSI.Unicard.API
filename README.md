# Task 1
- Design a database schema for a simple e-commerce platform with the following requirements:
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
#  Write stored procedures for the following:
- Retrieve all users who have placed an order.
- Retrieve the top 5 products based on the total quantity sold.
- Calculate the total revenue generated from orders in the last month.
- Retrieve the list of orders along with the user and product details for each order item.
  
```sh
  GO

CREATE PROCEDURE Totalrevenue
AS
BEGIN
    SELECT SUM(Orders.TotalAmount) AS TotalRevenue
    FROM Orders
    WHERE Orders.OrderDate <= GETDATE()
    AND Orders.OrderDate >= DATEADD(MONTH, -1, GETDATE());
END;
GO



CREATE PROCEDURE Top5Products
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 5 
        pr.ProductId,
        pr.ProductName,
        pr.Price,
        pr.Description,
        SUM(it.Quantity) AS TotalQuantitySold
    FROM OrderItems it
    JOIN Products pr ON it.ProductId = pr.ProductId
    GROUP BY pr.ProductId, pr.ProductName, pr.Price, pr.Description
    ORDER BY TotalQuantitySold DESC;
END;

go
create procedure orderItemWithDetailed
as
begin

select ord.*,us.*,pr.* from OrderItems ot
join Orders ord on ot.OrderId=ord.OrderId
join Users us on ord.UserId=us.UserId
join Products pr on ot.ProductId=pr.ProductId

end

go
CREATE PROCEDURE UsersWithOrders
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DISTINCT u.UserId, u.UserName, u.Email
    FROM Users u
    INNER JOIN Orders o ON u.UserId = o.UserId;
END;
```
