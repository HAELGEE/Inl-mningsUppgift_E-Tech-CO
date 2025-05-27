Insert into ProductCategory([ProductCategoryName])
values
('Computer'),
('Phone'),
('Screen')
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


Insert into Shop (ProductCategoryId, [name], Quantity, ProductInformation, ProductSubcategoryId, Price, Sold, RegularShipping, ExpressShipping)
values
(1, 'Samsung 9100 PRO M.2', 10, 'The latest hard drive on the market.', 1, 2849, 0, 200, 500),
(1, 'Samsung 990 EVO Plus', 33, 'A more affordable yet solid SSD.', 1, 1549, 0, 200, 500),
(1, 'Intel Core i9, 11', 1, 'This is a good computer for gaming, but it is best for programming.', 2, 1199, 0, 200, 500),
(1, 'AMD Ryzen 2040', 2, 'The best desktop computer for gaming.', 2, 5001, 0, 200, 500),
(1, 'Asus 3940 pro', 9, 'Great for playing Tetris.', 3, 1550, 0, 200, 500),
(1, 'Apple MacBook Air (2025)', 69, 'Has the latest CPU and fast RAM, ideal for drawing in Paint.', 3, 300, 0, 200, 500),

(2, 'iPhone 16e', 5, 'Recent model with promotional offer.', 4, 800, 0, 200, 500),
(2, 'iPhone 15 Pro Max', 11, 'Great for calls and casual gaming.', 4, 7801, 0, 200, 500),
(2, 'Nothing Phone (3a)', 99, 'A unique phone with a custom OS.', 5, 4290, 0, 200, 500),
(2, 'Nothing Phone (2a)', 20, 'Older variant with the same great OS.', 5, 3390, 0, 200, 500),
(2, 'Galaxy S25 Ultra', 4, 'High-end phone with premium features.', 6, 12049, 0, 200, 500),
(2, 'Galaxy Z Fold 6', 10, 'Foldable phone with advanced functionality.', 6, 16000, 0, 200, 500),

(3, 'MSI 27" Gaming Screen MAG 27CQ6F', 30, 'Refresh rate: 180Hz, response time: 0.5ms.', 7, 2990, 0, 200, 500),
(3, 'MSI 40" Gaming Screen MAG401QR', 5, 'Ultra-wide monitor. 155Hz, 1ms response.', 7, 5490, 0, 200, 500),
(3, 'ASUS 27" TUF VG27AQ3A', 50, '180Hz refresh rate, 0ms response.', 8, 3990, 0, 200, 500),
(3, 'ASUS 34" Curved TUF VG34VQL3A', 55, '200Hz refresh rate, 1ms response.', 8, 5490, 0, 200, 500),
(3, 'Samsung 34" Odyssey G8 34DG85', 10, '175Hz refresh rate, 1ms response.', 9, 9490, 0, 200, 500),
(3, 'Samsung 34" ViewFinity S6 34C654VA', 10, '100Hz refresh rate, 5ms response.', 9, 5490, 0, 200, 500);
go


