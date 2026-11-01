SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- Customers (6)
SET IDENTITY_INSERT dbo.Customers ON;
INSERT INTO dbo.Customers (CustomerId, Name, Email, Phone, Tier, CreatedAt) VALUES
    (1, N'Aarav Sharma', N'aarav.sharma@example.com', N'+91-98100-00001', N'Gold', DATEADD(HOUR, -9600, SYSUTCDATETIME())),
    (2, N'Priya Nair', N'priya.nair@example.com', N'+91-98100-00002', N'Silver', DATEADD(HOUR, -6000, SYSUTCDATETIME())),
    (3, N'Rohan Mehta', N'rohan.mehta@example.com', N'+91-98100-00003', N'Standard', DATEADD(HOUR, -2880, SYSUTCDATETIME())),
    (4, N'Sneha Iyer', N'sneha.iyer@example.com', N'+91-98100-00004', N'Gold', DATEADD(HOUR, -7200, SYSUTCDATETIME())),
    (5, N'Vikram Singh', N'vikram.singh@example.com', NULL, N'Standard', DATEADD(HOUR, -1440, SYSUTCDATETIME())),
    (6, N'Ananya Gupta', N'ananya.gupta@example.com', N'+91-98100-00006', N'Silver', DATEADD(HOUR, -4320, SYSUTCDATETIME()));
SET IDENTITY_INSERT dbo.Customers OFF;

-- Addresses (7)
SET IDENTITY_INSERT dbo.Addresses ON;
INSERT INTO dbo.Addresses (AddressId, CustomerId, Line1, Line2, City, State, PostalCode, Country, IsDefault) VALUES
    (1, 1, N'12 MG Road', N'Flat 4B', N'New Delhi', N'Delhi', N'110001', N'India', 1),
    (2, 1, N'88 Lakeview Apartments', NULL, N'Gurugram', N'Haryana', N'122001', N'India', 0),
    (3, 2, N'45 Residency Road', NULL, N'Bengaluru', N'Karnataka', N'560025', N'India', 1),
    (4, 3, N'7 Park Street', NULL, N'Mumbai', N'Maharashtra', N'400001', N'India', 1),
    (5, 4, N'21 Anna Salai', NULL, N'Chennai', N'Tamil Nadu', N'600002', N'India', 1),
    (6, 5, N'3 Banjara Hills Road', NULL, N'Hyderabad', N'Telangana', N'500034', N'India', 1),
    (7, 6, N'19 FC Road', NULL, N'Pune', N'Maharashtra', N'411004', N'India', 1);
SET IDENTITY_INSERT dbo.Addresses OFF;

-- Products (10)
SET IDENTITY_INSERT dbo.Products ON;
INSERT INTO dbo.Products (ProductId, Sku, Name, Category, Price, StockQty, IsReturnable, ReturnWindowDays) VALUES
    (1, N'HP-100', N'Wireless Headphones', N'Audio', 99.99, 40, 1, 30),
    (2, N'CASE-1', N'Carry Case', N'Accessories', 29.99, 120, 1, 30),
    (3, N'KB-220', N'Mechanical Keyboard', N'Peripherals', 49.50, 60, 1, 30),
    (4, N'MP-001', N'Mouse Pad', N'Accessories', 19.99, 200, 1, 30),
    (5, N'MON-27', N'27-inch Monitor', N'Displays', 249.00, 15, 1, 15),
    (6, N'WC-720', N'HD Webcam', N'Peripherals', 59.00, 0, 1, 30),
    (7, N'SSD-1T', N'1TB Portable SSD', N'Storage', 129.00, 25, 1, 30),
    (8, N'EB-300', N'Earbuds', N'Audio', 39.00, 80, 0, 0),
    (9, N'LAP-14', N'14-inch Laptop', N'Computers', 899.00, 8, 1, 15),
    (10, N'CHG-65', N'65W USB-C Charger', N'Accessories', 24.99, 150, 1, 30);
SET IDENTITY_INSERT dbo.Products OFF;

-- Orders (11)
SET IDENTITY_INSERT dbo.Orders ON;
INSERT INTO dbo.Orders (OrderId, CustomerId, OrderDate, Status, TotalAmount, ShippingAddressId) VALUES
    (1031, 1, DATEADD(HOUR, -768, SYSUTCDATETIME()), N'Delivered', 19.99, 1),
    (1038, 1, DATEADD(HOUR, -216, SYSUTCDATETIME()), N'Delivered', 49.50, 1),
    (1042, 1, DATEADD(HOUR, -48, SYSUTCDATETIME()), N'Shipped', 129.98, 1),
    (2001, 2, DATEADD(HOUR, -24, SYSUTCDATETIME()), N'Processing', 249.00, 3),
    (3001, 3, DATEADD(HOUR, -432, SYSUTCDATETIME()), N'Delivered', 899.00, 4),
    (3002, 3, DATEADD(HOUR, -528, SYSUTCDATETIME()), N'Delivered', 249.00, 4),
    (4001, 4, DATEADD(HOUR, -72, SYSUTCDATETIME()), N'Shipped', 188.00, 5),
    (5001, 5, DATEADD(HOUR, -240, SYSUTCDATETIME()), N'Cancelled', 59.00, 6),
    (5002, 5, DATEADD(HOUR, -24, SYSUTCDATETIME()), N'Placed', 24.99, 6),
    (6001, 6, DATEADD(HOUR, -288, SYSUTCDATETIME()), N'Shipped', 899.00, 7),
    (6002, 6, DATEADD(HOUR, -480, SYSUTCDATETIME()), N'Delivered', 39.00, 7);
SET IDENTITY_INSERT dbo.Orders OFF;

-- OrderItems (13)
SET IDENTITY_INSERT dbo.OrderItems ON;
INSERT INTO dbo.OrderItems (OrderItemId, OrderId, ProductId, Quantity, UnitPrice, ItemStatus) VALUES
    (1, 1031, 4, 1, 19.99, N'Delivered'),
    (2, 1038, 3, 1, 49.50, N'ReturnRequested'),
    (3, 1042, 1, 1, 99.99, N'Shipped'),
    (4, 1042, 2, 1, 29.99, N'Shipped'),
    (5, 2001, 5, 1, 249.00, N'Placed'),
    (6, 3001, 9, 1, 899.00, N'ReturnRequested'),
    (7, 3002, 5, 1, 249.00, N'Delivered'),
    (8, 4001, 7, 1, 129.00, N'Shipped'),
    (9, 4001, 6, 1, 59.00, N'Placed'),
    (10, 5001, 6, 1, 59.00, N'Cancelled'),
    (11, 5002, 10, 1, 24.99, N'Placed'),
    (12, 6001, 9, 1, 899.00, N'Shipped'),
    (13, 6002, 8, 1, 39.00, N'Delivered');
SET IDENTITY_INSERT dbo.OrderItems OFF;


-- Shipments (8)
SET IDENTITY_INSERT dbo.Shipments ON;
INSERT INTO dbo.Shipments (ShipmentId, OrderId, Carrier, TrackingNumber, ShippedAt, EstimatedDelivery, DeliveredAt, Status) VALUES
    (1, 1031, N'DHL', N'DH555000111', DATEADD(HOUR, -744, SYSUTCDATETIME()), DATEADD(HOUR, -648, SYSUTCDATETIME()), DATEADD(HOUR, -672, SYSUTCDATETIME()), N'Delivered'),
    (2, 1038, N'DHL', N'DH987654321', DATEADD(HOUR, -192, SYSUTCDATETIME()), DATEADD(HOUR, -96, SYSUTCDATETIME()), DATEADD(HOUR, -120, SYSUTCDATETIME()), N'Delivered'),
    (3, 1042, N'BlueDart', N'BD123456789', DATEADD(HOUR, -24, SYSUTCDATETIME()), DATEADD(HOUR, 48, SYSUTCDATETIME()), NULL, N'InTransit'),
    (4, 3001, N'BlueDart', N'BD300100001', DATEADD(HOUR, -408, SYSUTCDATETIME()), DATEADD(HOUR, -336, SYSUTCDATETIME()), DATEADD(HOUR, -336, SYSUTCDATETIME()), N'Delivered'),
    (5, 3002, N'Delhivery', N'DL300200002', DATEADD(HOUR, -504, SYSUTCDATETIME()), DATEADD(HOUR, -408, SYSUTCDATETIME()), DATEADD(HOUR, -408, SYSUTCDATETIME()), N'Delivered'),
    (6, 4001, N'BlueDart', N'BD400100001', DATEADD(HOUR, -24, SYSUTCDATETIME()), DATEADD(HOUR, 72, SYSUTCDATETIME()), NULL, N'InTransit'),
    (7, 6001, N'DHL', N'DH600100001', DATEADD(HOUR, -264, SYSUTCDATETIME()), DATEADD(HOUR, -72, SYSUTCDATETIME()), NULL, N'Delayed'),
    (8, 6002, N'Delhivery', N'DL600200002', DATEADD(HOUR, -456, SYSUTCDATETIME()), DATEADD(HOUR, -384, SYSUTCDATETIME()), DATEADD(HOUR, -384, SYSUTCDATETIME()), N'Delivered');
SET IDENTITY_INSERT dbo.Shipments OFF;

-- ShipmentEvents (23)
SET IDENTITY_INSERT dbo.ShipmentEvents ON;
INSERT INTO dbo.ShipmentEvents (EventId, ShipmentId, EventTime, Location, Description) VALUES
    (1, 1, DATEADD(HOUR, -744, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (2, 1, DATEADD(HOUR, -720, SYSUTCDATETIME()), N'Mumbai Hub', N'Departed origin hub'),
    (3, 1, DATEADD(HOUR, -672, SYSUTCDATETIME()), N'New Delhi', N'Delivered'),
    (4, 2, DATEADD(HOUR, -192, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (5, 2, DATEADD(HOUR, -144, SYSUTCDATETIME()), N'Delhi Hub', N'Arrived at destination hub'),
    (6, 2, DATEADD(HOUR, -120, SYSUTCDATETIME()), N'New Delhi', N'Delivered'),
    (7, 3, DATEADD(HOUR, -24, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (8, 3, DATEADD(HOUR, -20, SYSUTCDATETIME()), N'Mumbai Hub', N'Departed origin hub'),
    (9, 3, DATEADD(HOUR, -6, SYSUTCDATETIME()), N'Delhi Hub', N'Arrived at destination hub'),
    (10, 4, DATEADD(HOUR, -408, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (11, 4, DATEADD(HOUR, -360, SYSUTCDATETIME()), N'Mumbai Hub', N'Out for delivery'),
    (12, 4, DATEADD(HOUR, -336, SYSUTCDATETIME()), N'Mumbai', N'Delivered'),
    (13, 5, DATEADD(HOUR, -504, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (14, 5, DATEADD(HOUR, -432, SYSUTCDATETIME()), N'Mumbai Hub', N'Out for delivery'),
    (15, 5, DATEADD(HOUR, -408, SYSUTCDATETIME()), N'Mumbai', N'Delivered'),
    (16, 6, DATEADD(HOUR, -24, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (17, 6, DATEADD(HOUR, -10, SYSUTCDATETIME()), N'Mumbai Hub', N'Departed origin hub'),
    (18, 7, DATEADD(HOUR, -264, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (19, 7, DATEADD(HOUR, -216, SYSUTCDATETIME()), N'Nagpur Hub', N'Arrived at transit hub'),
    (20, 7, DATEADD(HOUR, -144, SYSUTCDATETIME()), N'Nagpur Hub', N'Delayed: awaiting onward transport'),
    (21, 8, DATEADD(HOUR, -456, SYSUTCDATETIME()), N'Bhiwandi Warehouse', N'Picked up by carrier'),
    (22, 8, DATEADD(HOUR, -408, SYSUTCDATETIME()), N'Pune Hub', N'Out for delivery'),
    (23, 8, DATEADD(HOUR, -384, SYSUTCDATETIME()), N'Pune', N'Delivered');
SET IDENTITY_INSERT dbo.ShipmentEvents OFF;
