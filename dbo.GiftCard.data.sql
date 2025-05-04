-- tuples for the table
INSERT INTO GiftCard (GiftCardNumber, CurrentBalance, IsActive, ExpirationDate)
VALUES 
('123456', 100.00, 1, '2025-12-31'),
('111111', 50.00, 1, '2022-01-01'), -- expired
('222222', 10.00, 0, '2025-12-31'), -- inactive
('333333', 0.00, 1, '2025-12-31'); -- no balance

-- UPDATE GiftCard SET CurrentBalance = 100 WHERE GiftCardNumber = '123456';