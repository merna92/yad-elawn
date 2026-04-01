-- أولاً: مسح الجداول لو كانت موجودة (بالترتيب العكسي عشان الـ Foreign Keys)
IF OBJECT_ID('Evaluates', 'U') IS NOT NULL DROP TABLE Evaluates;
IF OBJECT_ID('Audit_Log', 'U') IS NOT NULL DROP TABLE Audit_Log;
IF OBJECT_ID('Notification', 'U') IS NOT NULL DROP TABLE Notification;
IF OBJECT_ID('Message', 'U') IS NOT NULL DROP TABLE Message;
IF OBJECT_ID('Matches', 'U') IS NOT NULL DROP TABLE Matches;
IF OBJECT_ID('Status_History', 'U') IS NOT NULL DROP TABLE Status_History;
IF OBJECT_ID('Medical_Supplies', 'U') IS NOT NULL DROP TABLE Medical_Supplies;
IF OBJECT_ID('Medicine', 'U') IS NOT NULL DROP TABLE Medicine;
IF OBJECT_ID('Clothes', 'U') IS NOT NULL DROP TABLE Clothes;
IF OBJECT_ID('Food', 'U') IS NOT NULL DROP TABLE Food;
IF OBJECT_ID('Donation', 'U') IS NOT NULL DROP TABLE Donation;
IF OBJECT_ID('Admin', 'U') IS NOT NULL DROP TABLE Admin;
IF OBJECT_ID('Beneficiary', 'U') IS NOT NULL DROP TABLE Beneficiary;
IF OBJECT_ID('Charity', 'U') IS NOT NULL DROP TABLE Charity;
IF OBJECT_ID('Donor', 'U') IS NOT NULL DROP TABLE Donor;
IF OBJECT_ID('[User]', 'U') IS NOT NULL DROP TABLE [User];
IF OBJECT_ID('Location', 'U') IS NOT NULL DROP TABLE Location;

-- ثانياً: كريات الجداول من جديد
-- 1. جدول الموقع (Location)
CREATE TABLE Location (
    LocationID INT PRIMARY KEY IDENTITY(1,1),
    City_Area VARCHAR(100) NOT NULL,
    GPS_Coordinates VARCHAR(100)
);

-- 2. جدول المستخدم الأساسي (User)
CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    FName VARCHAR(50) NOT NULL,
    LName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Address NVARCHAR(MAX),
    IsVerified BIT DEFAULT 0
);

-- 3. جداول أنواع المستخدمين [cite: 1]
CREATE TABLE Donor (
    DonorID INT PRIMARY KEY,
    Donation_Count INT DEFAULT 0,
    CONSTRAINT FK_Donor_User FOREIGN KEY (DonorID) REFERENCES [User](UserID) ON DELETE CASCADE
);

CREATE TABLE Charity (
    CharityID INT PRIMARY KEY,
    Capacity INT,
    License_Number VARCHAR(50) UNIQUE,
    CoverageArea VARCHAR(100),
    Needs NVARCHAR(MAX),
    LocationID INT,
    CONSTRAINT FK_Charity_User FOREIGN KEY (CharityID) REFERENCES [User](UserID) ON DELETE CASCADE,
    CONSTRAINT FK_Charity_Location FOREIGN KEY (LocationID) REFERENCES Location(LocationID)
);

CREATE TABLE Beneficiary (
    BeneficiaryID INT PRIMARY KEY,
    LocationID INT,
    CONSTRAINT FK_Beneficiary_User FOREIGN KEY (BeneficiaryID) REFERENCES [User](UserID) ON DELETE CASCADE,
    CONSTRAINT FK_Beneficiary_Location FOREIGN KEY (LocationID) REFERENCES Location(LocationID)
);

CREATE TABLE Admin (
    AdminID INT PRIMARY KEY,
    CONSTRAINT FK_Admin_User FOREIGN KEY (AdminID) REFERENCES [User](UserID) ON DELETE CASCADE
);

-- 4. جدول التبرع الأساسي 
CREATE TABLE Donation (
    DonationID INT PRIMARY KEY IDENTITY(1,1),
    Status VARCHAR(50) CHECK (Status IN ('Pending', 'Under Review', 'Available', 'Reserved', 'Delivered', 'Cancelled')),
    Image VARCHAR(255),
    DonorID INT NOT NULL,
    LocationID INT,
    CONSTRAINT FK_Donation_Donor FOREIGN KEY (DonorID) REFERENCES Donor(DonorID),
    CONSTRAINT FK_Donation_Location FOREIGN KEY (LocationID) REFERENCES Location(LocationID)
);

-- 5. جداول أنواع التبرعات (المطابقة للـ ERD) [cite: 1]
CREATE TABLE Food (
    DonationID INT PRIMARY KEY,
    Product_Name VARCHAR(100),
    Food_Type VARCHAR(50) CHECK (Food_Type IN ('Fresh', 'Canned', 'Dried')),
    Category VARCHAR(50) CHECK (Category IN ('Fruits', 'Vegetables', 'Dry goods')),
    Expiry_Date DATE,
    Quantity VARCHAR(50),
    Storage_Condition VARCHAR(100),
    Shelf_Life VARCHAR(50),
    CONSTRAINT FK_Food_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID) ON DELETE CASCADE
);

CREATE TABLE Clothes (
    DonationID INT PRIMARY KEY,
    Season VARCHAR(50) CHECK (Season IN ('Summer', 'Winter', 'All-season')),
    Gender VARCHAR(50) CHECK (Gender IN ('Men', 'Women', 'Children')),
    Size VARCHAR(20),
    Condition VARCHAR(50) CHECK (Condition IN ('New', 'Very Good', 'Good')),
    Cleaning_Status VARCHAR(100),
    CONSTRAINT FK_Clothes_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID) ON DELETE CASCADE
);

CREATE TABLE Medicine (
    DonationID INT PRIMARY KEY,
    Medicine_Name VARCHAR(100),
    Medicine_Type VARCHAR(100),
    Expiry_Date DATE,
    Quantity VARCHAR(50),
    Safety_Conditions NVARCHAR(MAX),
    CONSTRAINT FK_Medicine_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID) ON DELETE CASCADE
);

CREATE TABLE Medical_Supplies (
    DonationID INT PRIMARY KEY,
    Supply_Name VARCHAR(100),
    Condition VARCHAR(100),
    Quantity VARCHAR(50),
    CONSTRAINT FK_MedicalSupplies_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID) ON DELETE CASCADE
);

-- 6. جداول العمليات والتواصل 
CREATE TABLE Status_History (
    HistoryID INT PRIMARY KEY IDENTITY(1,1),
    DonationID INT,
    Old_Status VARCHAR(50),
    New_Status VARCHAR(50),
    Change_Date DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_StatusHistory_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID)
);

CREATE TABLE Matches (
    MatchID INT PRIMARY KEY IDENTITY(1,1),
    DonationID INT,
    CharityID INT,
    BeneficiaryID INT,
    Distance DECIMAL(10,2),
    Urgency_Level VARCHAR(50),
    Match_Date DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Matches_Donation FOREIGN KEY (DonationID) REFERENCES Donation(DonationID),
    CONSTRAINT FK_Matches_Charity FOREIGN KEY (CharityID) REFERENCES Charity(CharityID),
    CONSTRAINT FK_Matches_Beneficiary FOREIGN KEY (BeneficiaryID) REFERENCES Beneficiary(BeneficiaryID)
);

CREATE TABLE Message (
    MessageID INT PRIMARY KEY IDENTITY(1,1),
    SenderID INT,
    ReceiverID INT,
    Content NVARCHAR(MAX),
    Sent_Date_Time DATETIME DEFAULT GETDATE(),
    Is_Read BIT DEFAULT 0,
    CONSTRAINT FK_Message_Sender FOREIGN KEY (SenderID) REFERENCES [User](UserID),
    CONSTRAINT FK_Message_Receiver FOREIGN KEY (ReceiverID) REFERENCES [User](UserID)
);

CREATE TABLE Notification (
    NotifID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT,
    Content NVARCHAR(MAX),
    Timestamp DATETIME DEFAULT GETDATE(),
    Is_Read BIT DEFAULT 0,
    CONSTRAINT FK_Notification_User FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

CREATE TABLE Audit_Log (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    AdminID INT,
    Action_Taken NVARCHAR(MAX),
    Action_Date DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_AuditLog_Admin FOREIGN KEY (AdminID) REFERENCES Admin(AdminID)
);

CREATE TABLE Evaluates (
    CharityID INT,
    DonorID INT,
    Score INT,
    Comment NVARCHAR(MAX),
    PRIMARY KEY (CharityID, DonorID),
    CONSTRAINT FK_Evaluates_Charity FOREIGN KEY (CharityID) REFERENCES Charity(CharityID),
    CONSTRAINT FK_Evaluates_Donor FOREIGN KEY (DonorID) REFERENCES Donor(DonorID)
);