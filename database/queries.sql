CREATE TRIGGER trg_DonationStatusHistory
ON Donation
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Status)
    BEGIN
        INSERT INTO Status_History (DonationID, Old_Status, New_Status, Change_Date)
        SELECT d.DonationID, d.Status, i.Status, GETDATE()
        FROM deleted d
        INNER JOIN inserted i ON d.DonationID = i.DonationID
    END
END;



CREATE PROCEDURE sp_RegisterDonor
    @FName VARCHAR(50), @LName VARCHAR(50), @Email VARCHAR(100), 
    @Password VARCHAR(255), @Phone VARCHAR(20), @Address NVARCHAR(MAX)
AS
BEGIN
    BEGIN TRANSACTION
    BEGIN TRY
        INSERT INTO [User] (FName, LName, Email, Password, Phone, Address, UserType, IsVerified)
        VALUES (@FName, @LName, @Email, @Password, @Phone, @Address, 'Donor', 1);
        
        DECLARE @NewUserID INT = SCOPE_IDENTITY();
        
        INSERT INTO Donor (DonorID, Donation_Count) VALUES (@NewUserID, 0);
        
        COMMIT TRANSACTION
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION
        THROW;
    END CATCH
END;

CREATE VIEW vw_AvailableFoodDonations AS
SELECT 
    D.DonationID, F.Product_Name, F.Quantity, F.Expiry_Date, 
    U.FName + ' ' + U.LName AS DonorName, L.City_Area
FROM Donation D
JOIN Food F ON D.DonationID = F.DonationID
JOIN Donor DR ON D.DonorID = DR.DonorID
JOIN [User] U ON DR.DonorID = U.UserID
JOIN Location L ON D.LocationID = L.LocationID
WHERE D.Status = 'Available';




CREATE FUNCTION fn_GetDonorCount (@DonorID INT)
RETURNS INT
AS
BEGIN
    DECLARE @Count INT;
    SELECT @Count = COUNT(*) FROM Donation WHERE DonorID = @DonorID;
    RETURN @Count;
END;



SELECT 
    M.MatchID,
    U_Charity.FName AS CharityName,
    U_Beneficiary.FName AS BeneficiaryName,
    F.Product_Name AS ItemDonated,
    M.Match_Date
FROM Matches M
JOIN Charity C ON M.CharityID = C.CharityID
JOIN [User] U_Charity ON C.CharityID = U_Charity.UserID
JOIN Beneficiary B ON M.BeneficiaryID = B.BeneficiaryID
JOIN [User] U_Beneficiary ON B.BeneficiaryID = U_Beneficiary.UserID
JOIN Donation D ON M.DonationID = D.DonationID
JOIN Food F ON D.DonationID = F.DonationID;




-- 1. التأكد من وجود جمعية (Charity)
IF NOT EXISTS (SELECT 1 FROM Charity WHERE CharityID = 1001)
BEGIN
    -- إذا مكنش موجود يوزر برقم 1001، نكريت واحد مؤقت للتجربة
    IF NOT EXISTS (SELECT 1 FROM [User] WHERE UserID = 1001)
    BEGIN
        SET IDENTITY_INSERT [User] ON;
        INSERT INTO [User] (UserID, FName, LName, Email, Password, UserType) 
        VALUES (1001, 'Test', 'Charity', 'testcharity@mail.com', '123', 'Charity');
        SET IDENTITY_INSERT [User] OFF;
    END
    INSERT INTO Charity (CharityID, License_Number, Capacity) VALUES (1001, 'LIC-TEST-1', 500);
END

-- 2. التأكد من وجود مستفيد (Beneficiary)
IF NOT EXISTS (SELECT 1 FROM Beneficiary WHERE BeneficiaryID = 1002)
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [User] WHERE UserID = 1002)
    BEGIN
        SET IDENTITY_INSERT [User] ON;
        INSERT INTO [User] (UserID, FName, LName, Email, Password, UserType) 
        VALUES (1002, 'Test', 'Beneficiary', 'testben@mail.com', '123', 'Beneficiary');
        SET IDENTITY_INSERT [User] OFF;
    END
    INSERT INTO Beneficiary (BeneficiaryID) VALUES (1002);
END

-- 3. إضافة عملية مطابقة (Match) باستخدام أول DonationID متاح فعلياً
DECLARE @AvailableDonationID INT = (SELECT TOP 1 DonationID FROM Donation ORDER BY DonationID DESC);
DECLARE @CharityID INT = (SELECT TOP 1 CharityID FROM Charity);
DECLARE @BeneficiaryID INT = (SELECT TOP 1 BeneficiaryID FROM Beneficiary);

IF @AvailableDonationID IS NOT NULL
BEGIN
    INSERT INTO Matches (DonationID, CharityID, BeneficiaryID, Distance, Urgency_Level, Match_Date)
    VALUES (@AvailableDonationID, @CharityID, @BeneficiaryID, 10.5, 'Medium', GETDATE());
    
    PRINT 'Match Added Successfully for DonationID: ' + CAST(@AvailableDonationID AS VARCHAR);
END
ELSE
BEGIN
    PRINT 'No Donations found to create a match.';
END


-- 1. إضافة داتا لجدول الملابس (Clothes)
-- هنربطهم بآخر تبرعات ضفناها
INSERT INTO Clothes (DonationID, Season, Gender, Size, Condition, Cleaning_Status)
SELECT TOP 50 
    DonationID, 
    'Winter', 'Women', 'M', 'Very Good', 'Dry Cleaned'
FROM Donation 
WHERE DonationID NOT IN (SELECT DonationID FROM Food) -- نختار تبرعات مش متسجلة أكل
ORDER BY DonationID DESC;

-- 2. إضافة داتا لجدول الأدوية (Medicine)
INSERT INTO Medicine (DonationID, Medicine_Name, Medicine_Type, Expiry_Date, Quantity, Safety_Conditions)
SELECT TOP 50 
    DonationID, 
    'Panadol Extra', 'Painkiller', DATEADD(year, 2, GETDATE()), '5 Boxes', 'Keep in cool place'
FROM Donation 
WHERE DonationID NOT IN (SELECT DonationID FROM Food) 
AND DonationID NOT IN (SELECT DonationID FROM Clothes)
ORDER BY DonationID ASC;

-- 3. إضافة داتا لجدول المستلزمات الطبية (Medical_Supplies)
INSERT INTO Medical_Supplies (DonationID, Supply_Name, Condition, Quantity)
SELECT TOP 20 
    DonationID, 
    'First Aid Kit', 'New', '2 Units'
FROM Donation 
WHERE DonationID NOT IN (SELECT DonationID FROM Food) 
AND DonationID NOT IN (SELECT DonationID FROM Clothes)
AND DonationID NOT IN (SELECT DonationID FROM Medicine)
ORDER BY DonationID DESC;

-- 4. إضافة رسائل وهمية (Messages) بين اليوزرز
INSERT INTO Message (SenderID, ReceiverID, Content, Sent_Date_Time, Is_Read)
SELECT TOP 100
    U1.UserID, U2.UserID, 'Hello, is this donation still available?', GETDATE(), 0
FROM [User] U1 CROSS JOIN [User] U2
WHERE U1.UserID != U2.UserID AND U1.UserType = 'Charity' AND U2.UserType = 'Donor';

-- 5. إضافة تقييمات (Evaluates)
INSERT INTO Evaluates (CharityID, DonorID, Score, Comment)
SELECT TOP 50
    C.CharityID, D.DonorID, 5, 'Great donor, very fast response!'
FROM Charity C CROSS JOIN Donor D;

-- 6. إضافة سجلات في الـ Audit Log (عشان الأدمين)
INSERT INTO Audit_Log (AdminID, Action_Taken, Action_Date)
SELECT TOP 10 
    AdminID, 'Approved a new donation entry', GETDATE()
FROM Admin;








SELECT 
    D.DonationID,
    D.Status,
    CASE 
        WHEN F.DonationID IS NOT NULL THEN 'Food: ' + F.Product_Name
        WHEN C.DonationID IS NOT NULL THEN 'Clothes: ' + C.Gender + ' ' + C.Season
        WHEN M.DonationID IS NOT NULL THEN 'Medicine: ' + M.Medicine_Name
        ELSE 'Other'
    END AS ItemDetails,
    L.City_Area AS PickupLocation
FROM Donation D
LEFT JOIN Food F ON D.DonationID = F.DonationID
LEFT JOIN Clothes C ON D.DonationID = C.DonationID
LEFT JOIN Medicine M ON D.DonationID = M.DonationID
JOIN Location L ON D.LocationID = L.LocationID
WHERE D.DonorID = 1; -- جربي تغيري الرقم ده لأي ID عندك




SELECT TOP 10
    D.DonationID,
    U.FName + ' ' + U.LName AS DonorName,
    D.Status,
    F.Product_Name AS FoodItem,
    C.Season AS ClothingSeason,
    M.Medicine_Name AS MedicineName,
    L.City_Area
FROM Donation D
JOIN [User] U ON D.DonorID = U.UserID
JOIN Location L ON D.LocationID = L.LocationID
LEFT JOIN Food F ON D.DonationID = F.DonationID
LEFT JOIN Clothes C ON D.DonationID = C.DonationID
LEFT JOIN Medicine M ON D.DonationID = M.DonationID;