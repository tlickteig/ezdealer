USE master
GO

IF EXISTS(SELECT 'EZ_Dealer' FROM master.dbo.sysdatabases
			WHERE name = 'EZ_Dealer')
BEGIN
	DROP DATABASE [EZ_Dealer]
	print('Database Dropped')
END
GO

CREATE DATABASE [EZ_Dealer]
print('Database Created')
GO

USE [EZ_Dealer]
GO

print '' print '*** Creating Trade-In Destinations Table***'
GO
CREATE TABLE [dbo].[TradeInDestinations] (

	[DestinationID] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	CONSTRAINT [pk_DestinationID] PRIMARY KEY([DestinationID])

)

print '' print '*** Creating Part Types Table***'
GO
CREATE TABLE [dbo].[PartTypes] (

	[PartType] [nvarchar](50) NOT NULL,
	CONSTRAINT [pk_PartType] PRIMARY KEY([PartType])

)

print '' print '*** Creating Makes Table***'
GO
CREATE TABLE [dbo].[Makes] (

	[Make] [nvarchar](20) NOT NULL,
	CONSTRAINT [pk_Make] PRIMARY KEY([Make])

)

print '' print '*** Creating Employee Roles Table***'
GO
CREATE TABLE [dbo].[EmployeeRoles] (

	[EmployeeRole] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL
	CONSTRAINT [pk_EmployeeRole] PRIMARY KEY([EmployeeRole])

)

print '' print '*** Creating Car Types Table***'
GO
CREATE TABLE [dbo].[CarTypes] (

	[CarType] [nvarchar](20) NOT NULL,
	CONSTRAINT [pk_CarType] PRIMARY KEY([CarType])

)

print '' print '*** Creating Employees Table***'
GO
CREATE TABLE [dbo].[Employees] (

	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmployeeID] [int] IDENTITY(1000000, 1) NOT NULL,
	[EmployeeRole] [nvarchar](50) NOT NULL,
	[BirthDate] [date] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date],
	[Password] [nvarchar](100) DEFAULT '9C9064C59F1FFA2E174EE754D2979BE80DD30DB552EC03E7E327E9B1A4BD594E' NOT NULL
	CONSTRAINT [pk_EmployeeID] PRIMARY KEY([EmployeeID]),
	CONSTRAINT [fk_Employees_EmployeeRole] FOREIGN KEY([EmployeeRole])
		REFERENCES [EmployeeRoles]([EmployeeRole])

)

print '' print '*** Creating Customers Table***'
GO
CREATE TABLE [dbo].[Customers] (

	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](150) NOT NULL,
	[CustomerID] [int] IDENTITY(1000000, 1) NOT NULL,
	[EmailAddress] [nvarchar](100),
	[PhoneNumber] [nvarchar](20) NOT NULL,
	CONSTRAINT [pk_CustomerID] PRIMARY KEY([CustomerID]),

)

print '' print '*** Creating Trade-Ins Table***'
GO
CREATE TABLE [dbo].[TradeIns] (

	[DestinationID] [nvarchar](50) NOT NULL,
	[TradeInID] [int] IDENTITY(1000000, 1) NOT NULL,
	[Date] [date] NOT NULL,
	[EmployeeID] [int] NOT NULL
	CONSTRAINT [pk_TradeInID] PRIMARY KEY([TradeInID]),
	CONSTRAINT [fk_TradeIns_TradeInID] FOREIGN KEY([DestinationID])
		REFERENCES [TradeInDestinations]([DestinationID]),
	CONSTRAINT [fk_Employees_EmployeeID] FOREIGN KEY([EmployeeID])
		REFERENCES [Employees]([EmployeeID])

)

print '' print '*** Creating Billing Table***'
GO
CREATE TABLE [dbo].[Billing] (

	[BillingID] [int] IDENTITY(1000000, 1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[AmountDue] [money] NOT NULL,
	[AmountPaid] [money] NOT NULL,
	[IssueDate] [date] NOT NULL,
	[DueDate] [date] NOT NULL,
	CONSTRAINT [pk_BillingID] PRIMARY KEY([BillingID]),
	CONSTRAINT [fk_CustomerID_Billing] FOREIGN KEY([CustomerID])
		REFERENCES [Customers]([CustomerID])

)

print '' print '*** Creating Shipment Table***'
GO
CREATE TABLE [dbo].[Shipment] (

	[ShipmentID] [int] IDENTITY(1000000, 1) NOT NULL,
	[ShipmentAmount] [money] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	CONSTRAINT [pk_ShipmentID] PRIMARY KEY([ShipmentID]),
	CONSTRAINT [fk_Shipment_EmployeeID] FOREIGN KEY([EmployeeID])
		REFERENCES [Employees]([EmployeeID])

)

print '' print '*** Creating Billing Line Item Table***'
GO
CREATE TABLE [dbo].[BillingLineItem] (

	[BillingLineItemID] [int] IDENTITY(1000000, 1) NOT NULL,
	[BillingID] [int] NOT NULL,
	[Amount] [money] NOT NULL,
	CONSTRAINT [pk_BillingLineItemID] PRIMARY KEY([BillingLineItemID]),
	CONSTRAINT [fk_BillingLineItem_BillingID] FOREIGN KEY([BillingID])
		REFERENCES [Billing]([BillingID])

)

print '' print '*** Creating Sales Table***'
GO
CREATE TABLE [dbo].[Sales] (

	[SaleID] [int] IDENTITY(1000000, 1) NOT NULL,
	[SaleAmount] [money] NOT NULL,
	[SaleDate] [date] NOT NULL,
	[BillingLineItemID] [int] NOT NULL,
	[CustomerID] [int] NOT NULL,
	[EmployeeID] [int] NOT NULL,
	CONSTRAINT [pk_SaleID] PRIMARY KEY([SaleID]),
	CONSTRAINT [fk_Sales_BillingLineItemID] FOREIGN KEY([BillingLineItemID])
		REFERENCES [BillingLineItem]([BillingLineItemID])

)

print '' print '*** Creating Cars Table***'
GO
CREATE TABLE [dbo].[Cars] (

	[VIN] [nvarchar](20) NOT NULL,
	[Make] [nvarchar](20) NOT NULL,
	[Model] [nvarchar](20) NOT NULL,
	[Year] [int] NOT NULL,
	[Color] [nvarchar](20) NOT NULL,
	[MSRP] [money],
	[SaleID] [int],
	[ShipmentID] [int],
	[CarType] [nvarchar](20) NOT NULL,
	[IsUsed] [bit] DEFAULT 0 NOT NULL,
	[TradeInID] [int]
	CONSTRAINT [pk_VIN] PRIMARY KEY([VIN]),
	CONSTRAINT [fk_Cars_Make] FOREIGN KEY([Make])
		REFERENCES [Makes]([Make]),
	CONSTRAINT [fk_Cars_SaleID] FOREIGN KEY([SaleID])
		REFERENCES [Sales]([SaleID]),
	CONSTRAINT [fk_Cars_ShipmentID] FOREIGN KEY([ShipmentID])
		REFERENCES [Shipment]([ShipmentID]),
	CONSTRAINT [fk_Cars_TradeInID] FOREIGN KEY([TradeInID])
		REFERENCES [TradeIns]([TradeInID])
)

print '' print '*** Creating Parts Table***'
GO
CREATE TABLE [dbo].[Parts] (

	[PartType] [nvarchar](50) NOT NULL,
	[SerialNumber] [nvarchar](100) NOT NULL,
	[Cost] [money] NOT NULL,
	CONSTRAINT [pk_serialNumber] PRIMARY KEY([SerialNumber]),
	CONSTRAINT [fk_Parts_Type] FOREIGN KEY([PartType])
		REFERENCES [PartTypes]([PartType])

)

print '' print '*** Creating Repairs Table***'
GO
CREATE TABLE [dbo].[Repairs] (

	[RepairID] [int] IDENTITY(1000000, 1) NOT NULL,
	[RepairDescription] [nvarchar](500) NOT NULL,
	[EmployeeID] [int] NOT NULL,
	[VIN] [nvarchar](20) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[Date] [date] NOT NULL,
	[BillingLineItemID] [int],
	[Amount] [money],
	CONSTRAINT [pk_RepairID] PRIMARY KEY([RepairID]),
	CONSTRAINT [fk_Repairs_EmployeeID] FOREIGN KEY([EmployeeID])
		REFERENCES [Employees]([EmployeeID]),
	CONSTRAINT [fk_Repairs_CustomerID] FOREIGN KEY([CustomerID])
		REFERENCES [Customers]([CustomerID]),
	CONSTRAINT [fk_BillingLineItem_BillingLineItemID] FOREIGN KEY([BillingLineItemID])
		REFERENCES [BillingLineItem]([BillingLineItemID])

)

print '' print '*** Creating Repair Line Item Table***'
GO
CREATE TABLE [dbo].[RepairLineItem] (

	[RepairID] [int]  NOT NULL,
	[RepairLineItemID] [int] IDENTITY(1000000, 1) NOT NULL,
	[SerialNumber] [nvarchar](100) NOT NULL,
	[Amount] [money] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	CONSTRAINT [pk_RepairLineItemID] PRIMARY KEY([RepairLineItemID]),
	CONSTRAINT [fk_RepairLineItem_RepairID] FOREIGN KEY([RepairID])
		REFERENCES [Repairs]([RepairID]),
	CONSTRAINT [fk_RepairLineItem_SerialNumber] FOREIGN KEY([SerialNumber])
		REFERENCES [Parts]([SerialNumber]),
	CONSTRAINT [fk_RepairLineItem_Type] FOREIGN KEY([Type])
		REFERENCES [PartTypes]([PartType])

)

print '' print '*** Inserting data into TradeInDestinations***'
GO
INSERT INTO [dbo].[TradeInDestinations]
	([DestinationID], [Description])
	VALUES
	('WHOLESALE', 'To be sold at wholesale auction'),
	('RESOLD', 'To be resold'),
	('SCRAPYARD', 'To be sent to the scrapyard')

print '' print '*** Inserting data into PartTypes***'
GO
INSERT INTO [dbo].[PartTypes]
	([PartType])
	VALUES
	('CLUTCH'),
	('SHOCK_ABSORBER'),
	('FUEL_PUMP'),
	('DISTRIBUTOR'),
	('BRAKE_FLUID'),
	('BRAKE_PAD'),
	('BATTERY'),
	('CONTROL_ARM'),
	('FLYWHEEL'),
	('CATALYTIC_CONVERTER'),
	('ALTERNATOR'),
	('RADIATOR'),
	('MUFFLER'),
	('SPARK_PLUG'),
	('MOTOR_OIL'),
	('POWER_STEERING_FLUID'),
	('COOLANT'),
	('WINDSHIELD_WASHER_FLUID'),
	('OIL_FILTER'),
	('TAIL_LIGHT'),
	('HEAD_LIGHT'),
	('STARTER_MOTOR'),
	('ENGINE_CONTROL_MODULE'),
	('AIRBAG_CONTROL_MODULE'),
	('TRANSMISSION_CONTROL_MODULE'),
	('RADIATOR_FAN'),
	('WATER_PUMP'),
	('EXHAUST_PIPE')

print '' print '*** Inserting data into Makes***'
GO
INSERT INTO [dbo].[Makes]
	([Make])
	VALUES
	('ACURA'),
	('ALFA_ROMEO'),
	('ASTON_MARTIN'),
	('AUDI'),
	('BENTLEY'),
	('BMW'),
	('BUGATTI'),
	('BUICK'),
	('CADILLAC'),
	('CHEVROLET'),
	('CHRYSLER'),
	('CITROEN'),
	('DODGE'),
	('FERRARI'),
	('FIAT'),
	('FORD'),
	('GMC'),
	('HONDA'),
	('HYUNDAI'),
	('INFINITI'),
	('JAGUAR'),
	('JEEP'),
	('KIA'),
	('KOENIGSEGG'),
	('LAMBORGHINI'),
	('LAND_ROVER'),
	('LEXUS'),
	('MASERATI'),
	('MAZDA'),
	('MCLAREN'),
	('MERCEDES'),
	('MINI'),
	('MITSUBISHI'),
	('NISSAN'),
	('PAGANI'),
	('PEUGEOT'),
	('PORSCHE'),
	('RAM'),
	('RENAULT'),
	('ROLLS_ROYCE'),
	('SAAB'),
	('SUBARU'),
	('SUZUKI'),
	('TATA'),
	('TESLA'),
	('TOYOTA'),
	('VOLKSWAGEN'),
	('VOLVO'),
	('PONTIAC')

print '' print '*** Inserting data into EmployeeRoles***'
GO
INSERT INTO [dbo].[EmployeeRoles]
	([EmployeeRole], [Description])
	VALUES
	('MANAGER', 'Manager of the facilities'),
	('SALESMAN', 'Salesman of cars'),
	('TECHNICIAN', 'Performs repair work on cars'),
	('ADMIN', 'System administrator')

print '' print '*** Inserting data into CarTypes***'
GO
INSERT INTO [dbo].[CarTypes]
	([CarType])
	VALUES
	('SUV'),
	('SEDAN'),
	('SPORTS'),
	('HATCHBACK'),
	('COUPE'),
	('COMPACT'),
	('CONVERTIBLE'),
	('LUXURY'),
	('MINIVAN'),
	('HYBRID'),
	('TRUCK'),
	('ROADSTER'),
	('TOURING'),
	('ELECTRIC')

print '' print '*** Inserting data into Employees ***'
GO
INSERT INTO [dbo].[Employees]
	([FirstName], [LastName], [EmployeeRole], [BirthDate], [StartDate], [EndDate])
	VALUES
	('System', 'Admin', 'ADMIN', '2000-01-01', '2000-01-01', NULL),
	('Timothy', 'Lickteig', 'SALESMAN', '2000-08-02', '2019-01-01', NULL)

print '' print '*** Inserting data into Shipment ***'
GO
INSERT INTO [dbo].[Shipment]
	([ShipmentAmount], [EmployeeID], [Date])
	VALUES
	(3000, 1000001, '2019-05-21'),
	(1000, 1000001, '2019-03-16'),
	(5000, 1000001, '2019-04-05'),
	(10000, 1000001, '2019-10-23'),
	(13000, 1000001, '2019-08-06'),
	(18000, 1000001, '2019-04-16')

print '' print '*** Inserting data into TradeIns ***'
GO
INSERT INTO [dbo].[TradeIns]
	([DestinationID], [Date], [EmployeeID])
	VALUES
	('RESOLD', '2019-03-20', 1000001),
	('SCRAPYARD', '2019-06-15', 1000001),
	('WHOLESALE', '2019-08-10', 1000001)

print '' print '*** Inserting data into Cars ***'
GO
INSERT INTO [dbo].[Cars]
	([VIN], [Make], [Model], 
	[Year], [Color], [MSRP], 
	[SaleID], [ShipmentID], 
	[CarType], [IsUsed], [TradeInID])
	VALUES
	('1', 'TOYOTA', '4Runner', 2004, 'Silver', 30000, NULL, 1000000, 'SUV', 1, NULL),
	('2', 'FORD', 'Escort', 2001, 'Purple', 15000, NULL, 1000001, 'HATCHBACK', 1, NULL),
	('3', 'NISSAN', 'Altima', 2007, 'White', 20000, NULL, 1000002, 'SEDAN', 1, NULL),
	('4', 'HONDA', 'Accord', 2018, 'Black', 18000, NULL, 1000003, 'COUPE', 0, NULL),
	('5', 'TESLA', 'Model 3', 2020, 'Red', 72000, NULL, 1000003, 'ELECTRIC', 0, NULL),
	('6', 'JEEP', 'Gladiator', 2020, 'Silver', 40000, NULL, 1000003, 'SUV', 0, NULL),
	('7', 'CHRYSLER', 'Sebring', 2001, 'Green', 18000, NULL, NULL, 'SEDAN', 1, 1000000),
	('8', 'BUICK', 'Regal', 1987, 'Black', 23000, NULL, NULL, 'COUPE', 1, 1000001),
	('9', 'LEXUS', 'LFA', 2001, 'Yellow', 102000, NULL, NULL, 'SPORTS', 1, 1000002)

print '' print '*** Inserting data into Cars ***'
GO
INSERT INTO [dbo].[Customers]
	([FirstName], [LastName], [Address], 
	[EmailAddress], [PhoneNumber])
	VALUES
	('Bob', 'Marley', '913 Roehampton St. Pompano Beach, FL 33060', 'bobmarley@gmail.com', '(401) 689-9649'),
	('Bill', 'Gates', '95 Mayfield Drive Olympia, WA 98512', 'billgates@gmail.com', '(432) 301-3608'),
	('Tom', 'Cruise', '844 Rocky River Rd. Stone Mountain, GA 30083', 'tomcruise@gmail.com', '(301) 794-3959'),
	('Rodney', 'Alcala', '9750 St Paul Street Burnsville, MN 55337', 'rodneyalcala@gmail.com', '(768) 706-0356'),
	('Mavis', 'Bacon', '21 Fairway Ave. Bayonne, NJ 07002', 'mavisbacon@gmail.com', '(749) 270-6108'),
	('Kazoo', 'Kid', '9543 East Joy Ridge Street Garland, TX 75043', 'kazookid@gmail.com', '(461) 350-0647')

print '' print '*** Creating sp_authenticate_employee ***'
GO
CREATE PROCEDURE [sp_authenticate_employee] (

	@EmployeeID [int],
	@Password [nvarchar](100)

)
AS
BEGIN

	SELECT COUNT([EmployeeID])
	FROM [dbo].[Employees]
	WHERE [EmployeeID] = @EmployeeID
		AND [Password] = @Password
		AND [EndDate] IS NULL

END
GO

print '' print '*** Creating sp_return_employee_by_employeeID ***'
GO
CREATE PROCEDURE [sp_return_employee_by_employeeID] (

	@EmployeeID [int]

)
AS
BEGIN

	SELECT [FirstName], [LastName], [EmployeeID], [EmployeeRole], [BirthDate], [StartDate], [EndDate], [Password]
	FROM [Employees]
	WHERE [EmployeeID] = @EmployeeID

END
GO

print '' print '*** Creating sp_update_employee ***'
GO
CREATE PROCEDURE [sp_update_employee] (
	
	@EmployeeID [int],
	@FirstName [nvarchar](50),
	@LastName [nvarchar](50),
	@EmployeeRole [nvarchar](50),
	@BirthDate [date],
	@StartDate [date],
	@EndDate [date],
	@Password [nvarchar](100)

)
AS
BEGIN

	UPDATE [dbo].[Employees]
	SET
		[FirstName] = @FirstName,
		[LastName] = @LastName,
		[EmployeeRole] = @EmployeeRole,
		[BirthDate] = @BirthDate,
		[StartDate] = @StartDate,
		[EndDate] = @EndDate,
		[Password] = @Password
	WHERE [EmployeeID] = @EmployeeID

END
GO

print '' print '*** Creating sp_return_all_employees ***'
GO
CREATE PROCEDURE [sp_return_all_employees] 
AS
BEGIN
	
	SELECT
		[EmployeeID],
		[FirstName],
		[LastName],
		[EmployeeRole],
		[BirthDate],
		[StartDate],
		[EndDate],
		[Password]
	FROM [dbo].[Employees]

END
GO

print '' print '*** Creating sp_deactivate_employee ***'
GO
CREATE PROCEDURE [sp_deactivate_employee] (

	@EmployeeID [int]

)
AS
BEGIN

	UPDATE [dbo].[Employees]
	SET [EndDate] = GETDATE()
	WHERE [EmployeeID] = @EmployeeID

END
GO

print '' print '*** Creating sp_new_employee ***'
GO
CREATE PROCEDURE [sp_new_employee] (

	@FirstName [nvarchar](50),
	@LastName [nvarchar](50),
	@EmployeeRole [nvarchar](50),
	@BirthDate [date],
	@Password [nvarchar](100)

)
AS
BEGIN

	INSERT INTO [dbo].[Employees]
	([FirstName], [LastName], 
	[EmployeeRole], [BirthDate], [StartDate], 
	[EndDate], [Password])
	VALUES
	(@FirstName, @LastName,
	@EmployeeRole, @BirthDate, GETDATE(), 
	NULL, @Password)

END
GO

print '' print '*** Creating sp_return_car_types ***'
GO
CREATE PROCEDURE [sp_return_car_types]
AS
BEGIN

	SELECT [CarType]
	FROM [dbo].[CarTypes]

END
GO

print '' print '*** Creating sp_return_makes ***'
GO
CREATE PROCEDURE [sp_return_makes]
AS
BEGIN

	SELECT [Make]
	FROM [dbo].[Makes]

END
GO

print '' print '*** Creating sp_new_car ***'
GO
CREATE PROCEDURE [sp_new_car] (

	@VIN [nvarchar](20),
	@Make [nvarchar](20),
	@Model [nvarchar](20),
	@Year [int],
	@Color [nvarchar](20),
	@MSRP [money],
	@SaleID [int],
	@ShipmentID [int],
	@CarType [nvarchar](20),
	@IsUsed [bit],
	@TradeInID [int]

)
AS
BEGIN

	INSERT INTO [dbo].[Cars]
	([VIN], [Make], [Model], 
	[Year], [Color], [MSRP], 
	[SaleID], [ShipmentID], 
	[CarType], [IsUsed], [TradeInID])
	VALUES
	(@VIN, @Make, @Model, 
	@Year, @Color, @MSRP, 
	@SaleID, @ShipmentID, 
	@CarType, @IsUsed, @TradeInID)

END
GO

print '' print '*** Creating sp_update_car_status ***'
GO
CREATE PROCEDURE [sp_update_car_status] (

	@VIN [nvarchar](20),
	@SaleID [int],
	@ShipmentID [int],
	@TradeInID [int]
	
)
AS
BEGIN

	UPDATE [dbo].[Cars]
	SET [SaleID] = @SaleID,
		[ShipmentID] = @ShipmentID,
		[TradeInID] = @TradeInID
	WHERE [VIN] = @VIN

END
GO

print '' print '*** Creating sp_add_shipment_to_car ***'
GO
CREATE PROCEDURE [sp_add_shipment_to_car] (

	@VIN [nvarchar](20),
	@ShipmentAmount [money],
	@EmployeeID [int],
	@Date [date]
	
)
AS
BEGIN

	DECLARE @ShipmentID [int]
	SET @ShipmentID = 0

	INSERT INTO [dbo].[Shipment]
	([ShipmentAmount], [EmployeeID], [Date])
	VALUES
	(@ShipmentAmount, @EmployeeID, @Date)
	
	SELECT @ShipmentID = MAX([ShipmentID])
	FROM [dbo].[Shipment]
	
	UPDATE [dbo].[Cars]
	SET [ShipmentID] = @ShipmentID
	WHERE [VIN] = @VIN
	
END
GO

print '' print '*** Creating sp_add_tradeIn_to_car ***'
GO
CREATE PROCEDURE [sp_add_tradeIn_to_car] (

	@VIN [nvarchar](20),
	@TradeInDestination [nvarchar](50),
	@EmployeeID [int],
	@Date [date]
	
)
AS
BEGIN

	DECLARE @TradeInID [nvarchar](50)
	SET @TradeInID = 0

	INSERT INTO [dbo].[TradeIns]
	([DestinationID], [EmployeeID], [Date])
	VALUES
	(@TradeInDestination, @EmployeeID, @Date)
	
	SELECT @TradeInID = MAX([TradeInID])
	FROM [dbo].[TradeIns]
	
	UPDATE [dbo].[Cars]
	SET [TradeInID] = @TradeInID
	WHERE [VIN] = @VIN
	
END
GO

print '' print '*** Creating sp_return_all_cars ***'
GO
CREATE PROCEDURE [sp_return_all_cars]
AS
BEGIN
	
	SELECT
		[VIN],
		[Make],
		[Model],
		[Year],
		[Color],
		[MSRP],
		[SaleID],
		[ShipmentID],
		[CarType],
		[IsUsed],
		[TradeInID]
	FROM [dbo].[Cars]
	
END
GO

print '' print '*** Creating sp_return_car_by_vin ***'
GO
CREATE PROCEDURE [sp_return_car_by_vin]
(
	@VIN [nvarchar](20)
)
AS
BEGIN

	SELECT
		[Make],
		[Model],
		[Year],
		[Color],
		[MSRP],
		[SaleID],
		[ShipmentID],
		[CarType],
		[IsUsed],
		[TradeinID]
	FROM [dbo].[Cars]
	WHERE [VIN] = @VIN

END
GO

print '' print '*** Creating sp_return_shipment_amount ***'
GO
CREATE PROCEDURE [sp_return_shipment_amount]
(
	@ShipmentID [int]
)
AS
BEGIN

	SELECT 
	[ShipmentAmount]
	FROM [dbo].[Shipment]
	WHERE [ShipmentID] = @ShipmentID

END
GO

print '' print '*** Creating sp_return_tradeIn_destination ***'
GO
CREATE PROCEDURE [sp_return_tradeIn_destination]
(
	@TradeInID [int]
)
AS
BEGIN

	SELECT 
	[DestinationID]
	FROM [dbo].[TradeIns]
	WHERE [TradeInID] = @TradeInID

END
GO

print '' print '*** Creating sp_new_customer ***'
GO
CREATE PROCEDURE [sp_new_customer]
(
	@FirstName [nvarchar](50),
	@LastName [nvarchar](50),
	@Address [nvarchar](75),
	@EmailAddress [nvarchar](100),
	@PhoneNumber [nvarchar](11)
)
AS
BEGIN
	
	INSERT INTO [dbo].[Customers]
	([FirstName], [LastName], [Address],
	[EmailAddress], [PhoneNumber])
	VALUES
	(@FirstName, @LastName, @Address, 
	@EmailAddress, @PhoneNumber)
	
END
GO

print '' print '*** Creating sp_return_all_customers ***'
GO
CREATE PROCEDURE [sp_return_all_customers]
AS
BEGIN
	
	SELECT 
		[FirstName],
		[LastName],
		[Address],
		[CustomerID],
		[EmailAddress],
		[PhoneNumber]
	FROM [dbo].[Customers]

END
GO

print '' print '*** Creating sp_new_sale ***'
GO
CREATE PROCEDURE [sp_new_sale]
(
	@SaleAmount [money],
	@SaleDate [date],
	@VIN [nvarchar](20),
	@BillingLineItemID [int],
	@CustomerID [int],
	@EmployeeID [int]
)
AS
BEGIN

	INSERT INTO [dbo].[Sales]
	([SaleAmount], [SaleDate], [BillingLineItemID], [CustomerID], [EmployeeID])
	VALUES
	(@SaleAmount, @SaleDate, @BillingLineItemID, @CustomerID, @EmployeeID)

	DECLARE @SaleID [int]
	SELECT @SaleID = MAX([SaleID])
	FROM [dbo].[Sales]

	UPDATE [dbo].[Cars]
	SET [SaleID] = @SaleID
	WHERE [VIN] = @VIN

END
GO

print '' print '*** Creating sp_return_all_sales ***'
GO
CREATE PROCEDURE [sp_return_all_sales]
AS
BEGIN
	
	SELECT 
		[SaleID],
		[SaleAmount],
		[SaleDate],
		[BillingLineItemID],
		[CustomerID],
		[EmployeeID]
	FROM [dbo].[Sales]

END
GO

print '' print '*** Creating sp_return_sale_by_id ***'
GO
CREATE PROCEDURE [sp_return_sale_by_id]
(
	@SaleID [int]
)
AS
BEGIN
	
	SELECT		
		[SaleAmount],
		[SaleDate],
		[BillingLineItemID],
		[CustomerID],
		[EmployeeID]
	FROM [dbo].[Sales]
	WHERE @SaleID = [SaleID]

END
GO

print '' print '*** Creating sp_new_bill ***'
GO
CREATE PROCEDURE [sp_new_bill]
(
	@AmountDue [money],
	@AmountPaid [money],
	@IssueDate [date],
	@DueDate [date],
	@CustomerID [int]
)
AS
BEGIN

	INSERT INTO [dbo].[Billing]
	([AmountDue], [AmountPaid], [IssueDate], [DueDate], [CustomerID])
	VALUES
	(@AmountDue, @AmountPaid, @IssueDate, @DueDate, @CustomerID)

END
GO

print '' print '*** Creating sp_new_billing_line_item ***'
GO
CREATE PROCEDURE [sp_new_billing_line_item]
(
	@BillingID [int],
	@Amount [money]
)
AS
BEGIN
	
	INSERT INTO [dbo].[BillingLineItem]
	([BillingID], [Amount])
	VALUES
	(@BillingID, @Amount)

END
GO

print '' print '*** Creating sp_add_line_item_to_most_recent_bill ***'
GO
CREATE PROCEDURE [sp_add_line_item_to_most_recent_bill] (

	@Amount [money]
	
)
AS
BEGIN

	DECLARE @BillingLineItemID [nvarchar](50)
	SET @BillingLineItemID = 0

	SELECT @BillingLineItemID = MAX([BillingID])
	FROM [dbo].[Billing]

	INSERT INTO [dbo].[BillingLineItem]
	([BillingID], [Amount])
	VALUES
	(@BillingLineItemID, @Amount)
		
END
GO

print '' print '*** Creating sp_return_latest_billing_line_item_id ***'
GO
CREATE PROCEDURE [sp_return_latest_billing_line_item_id]
AS
BEGIN

	SELECT MAX([BillingLineItemID])
	FROM [dbo].[BillingLineItem]

END
GO

print '' print '*** Creating sp_return_customer_id_by_name ***'
GO
CREATE PROCEDURE [sp_return_customer_id_by_name]
(
	@FirstName [nvarchar](50),
	@LastName [nvarchar](50)
)
AS
BEGIN
	
	SELECT [CustomerID]
	FROM [Customers]
	WHERE @FirstName = [FirstName]
		AND @LastName = [LastName]

END
GO

print '' print '*** Creating sp_return_customer_by_id ***'
GO
CREATE PROCEDURE [sp_return_customer_by_id]
(
	@CustomerID [int]
)
AS
BEGIN

	SELECT [FirstName], [LastName], [Address], [EmailAddress], [PhoneNumber]
	FROM [dbo].[Customers]
	WHERE [CustomerID] = @CustomerID

END
GO

print '' print '*** Creating sp_return_all_part_types ***'
GO
CREATE PROCEDURE [sp_return_all_part_types]
AS
BEGIN
	SELECT [PartType]
	FROM [dbo].[PartTypes]
END
GO

print '' print '*** Creating sp_add_new_part ***'
GO
CREATE PROCEDURE [sp_add_new_part]
(
	@PartType [nvarchar](50),
	@SerialNumber [nvarchar](100),
	@Cost [money]
)
AS
BEGIN
	
	INSERT INTO [dbo].[Parts]
	([PartType], [SerialNumber], [Cost])
	VALUES
	(@PartType, @SerialNumber, @Cost)

END
GO

print '' print '*** Creating sp_return_all_parts ***'
GO
CREATE PROCEDURE [sp_return_all_parts]
AS
BEGIN 

	SELECT [Parts].[PartType], 
		[Parts].[SerialNumber], 
		[Parts].[Cost], 
		[RepairLineItem].[RepairLineItemID]
	FROM [dbo].[Parts] LEFT JOIN [dbo].[RepairLineItem] ON
		[Parts].[SerialNumber] = [RepairLineItem].[SerialNumber]

END
GO

print '' print '*** Creating sp_delete_part ***'
GO
CREATE PROCEDURE [sp_delete_part]
(
	@SerialNumber [nvarchar](100)
)
AS
BEGIN

	DELETE FROM [dbo].[Parts]
	WHERE [SerialNumber] = @SerialNumber

END
GO

print '' print '*** Creating sp_is_part_available ***'
GO
CREATE PROCEDURE [sp_is_part_available]
(
	@SerialNumber [nvarchar](100),
	@Type [nvarchar](50)
)
AS
BEGIN
	
	SELECT COUNT([SerialNumber])
	FROM [dbo].[Parts]
	WHERE [SerialNumber] = @SerialNumber
		AND [PartType] = @Type

END
GO

print '' print '*** Creating sp_new_repair ***'
GO
CREATE PROCEDURE [sp_new_repair]
(
	@RepairDescription [nvarchar](500),
	@EmployeeID [int],
	@VIN [nvarchar](20),
	@CustomerID [int],
	@Date [date],
	@BillingLineItemID [int],
	@Amount [money]
)
AS
BEGIN

	INSERT INTO [dbo].[Repairs]
	([RepairDescription], [EmployeeID], [VIN], 
	[CustomerID], [Date], [BillingLineItemID], [Amount])
	VALUES
	(@RepairDescription, @EmployeeID, @VIN, 
	@CustomerID, @Date, @billingLineItemID, @Amount)

	SELECT MAX([RepairID])
	FROM [dbo].[Repairs]

END
GO

print '' print '*** Creating sp_return_all_repairs ***'
GO
CREATE PROCEDURE [sp_return_all_repairs]
AS
BEGIN

	SELECT [RepairID], [RepairDescription], [EmployeeID], 
		[VIN], [CustomerID], [Amount], [Date], [BillingLineItemID]
	FROM [dbo].[Repairs]

END
GO

print '' print '*** Creating sp_new_repair_line_item ***'
GO
CREATE PROCEDURE [sp_new_repair_line_item]
(
	@RepairID [int],
	@Amount [money],
	@PartType [nvarchar](50),
	@SerialNumber [nvarchar](20)
)
AS
BEGIN
	
	INSERT INTO [dbo].[RepairLineItem]
	([RepairID], [Amount], [Type], [SerialNumber])
	VALUES
	(@RepairID, @Amount, @PartType, @SerialNumber)

END
GO

print '' print '*** Creating sp_get_repair_line_items ***'
GO
CREATE PROCEDURE [sp_get_repair_line_items]
(
	@RepairID [int]
)
AS
BEGIN
	
	SELECT [RepairID], [RepairLineItemID], 
		[SerialNumber], [Amount], [Type]
	FROM [dbo].[RepairLineItem]
	WHERE @RepairID = [RepairID]
	
END
GO

print '' print '*** Creating sp_return_all_bills ***'
GO
CREATE PROCEDURE [sp_return_all_bills]
AS
BEGIN

	SELECT [BillingID], [AmountDue], [AmountPaid], 
	[IssueDate], [DueDate], [CustomerID]
	FROM [dbo].[Billing]

END
GO

print '' print '*** Creating sp_select_bill ***'
GO
CREATE PROCEDURE [sp_select_bill]
(
	@BillingID [int]
)
AS
BEGIN

	SELECT [BillingID], [AmountDue], [AmountPaid], 
	[IssueDate], [DueDate], [CustomerID]
	FROM [dbo].[Billing]
	WHERE @BillingID = [BillingID]

END
GO

print '' print '*** Creating sp_return_all_bill_line_items ***'
GO
CREATE PROCEDURE [sp_return_all_bill_line_items]
(
	@BillingID [int]
)
AS
BEGIN

	SELECT [BillingLineItemID], [BillingID], [Amount]
	FROM [dbo].[BillingLineItem]
	WHERE [BillingID] = @BillingID

END
GO
