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

# Task 3: Performance Optimization
Objective: Test ability to optimize database queries and API performance.
Task:
1. Given the following query, identify potential performance issues and suggest improvements:
SELECT p.ProductName, SUM(oi.Quantity) AS TotalQuantity
FROM Products p
JOIN OrderItems oi ON p.ProductId = oi.ProductId
GROUP BY p.ProductName
ORDER BY TotalQuantity DESC;
2. Optimize the above query for better performance.

```sh
CREATE INDEX idx_orderitems_productid ON OrderItems(ProductId);

SELECT p.ProductName, SUM(oi.Quantity) AS TotalQuantity
FROM Products p
JOIN OrderItems oi ON p.ProductId = oi.ProductId
GROUP BY p.ProductId
ORDER BY TotalQuantity DESC;
```

# Task 4: Data Integrity and Transactions
Objective: Evaluate understanding of data integrity and transaction management in SQL.
Task:
1. Write a stored procedure(s) to create a new order with multiple order items. Ensure that the
procedure handles transactions and rolls back in case of any errors.

```sh

CREATE PROCEDURE InsertOrderItem
    @OrderId int,
    @ProductId int,
    @Quantity int,
    @Price decimal
AS
BEGIN
    INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
    VALUES (@OrderId, @ProductId, @Quantity, @Price);
END;
GO



CREATE PROCEDURE CreateNewOrder
    @UserId int,
    @OrderDate date,
    @TotalAmount decimal OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @OrderId int;

    BEGIN TRY
        BEGIN TRANSACTION;
        INSERT INTO Orders (UserId, OrderDate, TotalAmount)
        VALUES (@UserId, @OrderDate, 0);

        SET @OrderId = SCOPE_IDENTITY();

        DECLARE @ProductId int;
        DECLARE @Quantity int;
        DECLARE @Price decimal;

        SET @ProductId = 1;
        SET @Quantity = 2;
        SET @Price = 10.00;
        EXEC InsertOrderItem @OrderId, @ProductId, @Quantity, @Price;

        SET @ProductId = 2;
        SET @Quantity = 1;
        SET @Price = 20.00;
        EXEC InsertOrderItem @OrderId, @ProductId, @Quantity, @Price;

        SELECT @TotalAmount = SUM(Quantity * Price)
        FROM OrderItems
        WHERE OrderId = @OrderId;

        UPDATE Orders
        SET TotalAmount = @TotalAmount
        WHERE OrderId = @OrderId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
		throw
		
		end catch

end
```
2. Describe how you would ensure data integrity in the database, particularly for the `Orders` and
`OrderItems` tables.
```sh
Foreign Key Constraints:

Foreign key constraints ensure that each OrderId in the OrderItems table corresponds to a valid OrderId in the Orders table, and each ProductId in the OrderItems table corresponds to a valid ProductId in the Products table.
```
```sh
Transactions:

Use transactions to ensure that operations affecting multiple rows or tables are atomic. If any part of the transaction fails, the entire transaction can be rolled back, ensuring that the database remains in a consistent state.
```
```sh
Stored Procedures:

Use stored procedures to encapsulate business logic. Stored procedures can ensure that operations are performed in a controlled manner, validating input data and maintaining consistency.
```



