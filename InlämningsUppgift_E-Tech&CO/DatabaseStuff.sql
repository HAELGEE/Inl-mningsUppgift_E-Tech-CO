Insert into ProductCategory([ProductCategoryName], ShopId)
values
('Computer', 1),
('Phone', 1),
('Screen', 1)
go

Insert into ProductSubcategory([ProductSubcategoryName], ProductCategoryId)
values
('Computer Components', 1),
('Desktop Computers', 1),
('Laptops', 1),
('Apple', 2),
('Nothing', 2),
('Samsung Phone', 2),
('MSI', 3),
('ASUS', 3),
('Samsung', 3)
go


Insert into Shop (Category, [name], Quantity, ProductInformation, SubCategory, Price, Sold, RegularShipping, ExpressShipping)
values
('Computer', 'Samsung 9100 PRO M.2', 10, 'The latest hard drive on the market.', 'Computer Components', 2849, 0, 200, 500),
('Computer', 'Samsung 990 EVO Plus', 33, 'A more affordable yet solid SSD.', 'Computer Components', 1549, 0, 200, 500),
('Computer', 'Intel Core i9, 11', 1, 'This is a good computer for gaming, but it is best for programming.', 'Desktop Computers', 1199, 0, 200, 500),
('Computer', 'AMD Ryzen 2040', 2, 'The best desktop computer for gaming.', 'Desktop Computers', 5001, 0, 200, 500),
('Computer', 'Asus 3940 pro', 9, 'Great for playing Tetris.', 'Laptops', 1550, 0, 200, 500),
('Computer', 'Apple MacBook Air (2025)', 69, 'Has the latest CPU and fast RAM, ideal for drawing in Paint.', 'Laptops', 300, 0, 200, 500),

('Phone', 'iPhone 16e', 5, 'Recent model with promotional offer.', 'Apple', 800, 0, 200, 500),
('Phone', 'iPhone 15 Pro Max', 11, 'Great for calls and casual gaming.', 'Apple', 7801, 0, 200, 500),
('Phone', 'Nothing Phone (3a)', 99, 'A unique phone with a custom OS.', 'Nothing', 4290, 0, 200, 500),
('Phone', 'Nothing Phone (2a)', 20, 'Older variant with the same great OS.', 'Nothing', 3390, 0, 200, 500),
('Phone', 'Galaxy S25 Ultra', 4, 'High-end phone with premium features.', 'Samsung Phone', 12049, 0, 200, 500),
('Phone', 'Galaxy Z Fold 6', 10, 'Foldable phone with advanced functionality.', 'Samsung Phone', 16000, 0, 200, 500),

('Screen', 'MSI 27" Gaming Screen MAG 27CQ6F', 30, 'Refresh rate: 180Hz, response time: 0.5ms.', 'MSI', 2990, 0, 200, 500),
('Screen', 'MSI 40" Gaming Screen MAG401QR', 5, 'Ultra-wide monitor. 155Hz, 1ms response.', 'MSI', 5490, 0, 200, 500),
('Screen', 'ASUS 27" TUF VG27AQ3A', 50, '180Hz refresh rate, 0ms response.', 'ASUS', 3990, 0, 200, 500),
('Screen', 'ASUS 34" Curved TUF VG34VQL3A', 55, '200Hz refresh rate, 1ms response.', 'ASUS', 5490, 0, 200, 500),
('Screen', 'Samsung 34" Odyssey G8 34DG85', 10, '175Hz refresh rate, 1ms response.', 'Samsung', 9490, 0, 200, 500),
('Screen', 'Samsung 34" ViewFinity S6 34C654VA', 10, '100Hz refresh rate, 5ms response.', 'Samsung', 5490, 0, 200, 500);
go

