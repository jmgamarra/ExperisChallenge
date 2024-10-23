-- Verificar y crear la base de datos si no existe
IF DB_ID('ProductManager') IS NULL
BEGIN
    CREATE DATABASE ProductManager;
END;

-- Usar la base de datos ProductManager
USE ProductManager;

-- Verificar y crear la tabla Users
IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY(1,1),
        UserName NVARCHAR(100) NOT NULL UNIQUE
    );
END;

-- Verificar y crear la tabla UserSecurity
IF OBJECT_ID('UserSecurity', 'U') IS NULL
BEGIN
    CREATE TABLE UserSecurity (
        SecurityId INT PRIMARY KEY IDENTITY(1,1),
        UserId INT NOT NULL,
        PasswordHash NVARCHAR(256) NOT NULL,
        IsActive BIT DEFAULT 1,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );
END;

-- Verificar y crear la tabla Products
IF OBJECT_ID('Products', 'U') IS NULL
BEGIN
    CREATE TABLE Products (
        ProductId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(18,2) NOT NULL,
        Quantity INT NOT NULL,
        UserId INT NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );
END;

-- Insertar un usuario por defecto si no existe
IF NOT EXISTS (SELECT * FROM Users WHERE UserName = 'admin')
BEGIN
    INSERT INTO Users (UserName) VALUES ('admin');
    DECLARE @UserId INT = SCOPE_IDENTITY();
    INSERT INTO UserSecurity (UserId, PasswordHash, IsActive) 
    VALUES (@UserId, HASHBYTES('SHA2_256', 'adminpassword'), 1);
END;
go
--Product 
-- Procedimiento almacenado para crear un producto
CREATE OR ALTER PROCEDURE CreateProduct
    @Name NVARCHAR(100),
    @Price DECIMAL(18,2),
    @Quantity INT,
    @UserId INT
AS
BEGIN
    INSERT INTO Products (Name, Price, Quantity, UserId)
    VALUES (@Name, @Price, @Quantity, @UserId);
END;
GO

-- Procedimiento almacenado para obtener todos los productos de un usuario
CREATE OR ALTER PROCEDURE GetAllProductsByUser
    @UserId INT
AS
BEGIN
    SELECT ProductId AS Id, Name, Price, Quantity, UserId
    FROM Products
    WHERE UserId = @UserId;
END;
GO

-- Procedimiento almacenado para obtener un producto por ID
CREATE OR ALTER PROCEDURE GetProductById
    @ProductId INT
AS
BEGIN
    SELECT ProductId AS Id, Name, Price, Quantity, UserId
    FROM Products
    WHERE ProductId = @ProductId;
END;
GO

-- Procedimiento almacenado para actualizar un producto
CREATE OR ALTER PROCEDURE UpdateProduct
    @ProductId INT,
    @Name NVARCHAR(100),
    @Price DECIMAL(18,2),
    @Quantity INT
AS
BEGIN
    UPDATE Products
    SET Name = @Name, Price = @Price, Quantity = @Quantity
    WHERE ProductId = @ProductId;
END;
GO

-- Procedimiento almacenado para eliminar un producto
CREATE OR ALTER PROCEDURE DeleteProduct
    @ProductId INT
AS
BEGIN
    DELETE FROM Products WHERE ProductId = @ProductId;
END;
GO

---User

-- Procedimiento almacenado para crear un usuario y su seguridad en una transacción
CREATE OR ALTER PROCEDURE CreateUser
    @UserName NVARCHAR(100),
    @PasswordHash NVARCHAR(256),
    @IsActive BIT
AS
BEGIN
    SET XACT_ABORT ON;
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Insertar usuario en la tabla Users
       -- DECLARE @UserId INT;
        INSERT INTO Users (UserName) 
       -- OUTPUT INSERTED.UserId INTO @UserId
        VALUES (@UserName);
		  DECLARE @UserId INT = SCOPE_IDENTITY();
        -- Insertar seguridad del usuario en UserSecurity
        INSERT INTO UserSecurity (UserId, PasswordHash, IsActive)
        VALUES (@UserId, @PasswordHash, @IsActive);

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- Procedimiento almacenado para obtener un usuario por nombre
CREATE OR ALTER PROCEDURE GetUserByName
    @UserName NVARCHAR(100)
AS
BEGIN
    SELECT UserId AS Id, UserName 
    FROM Users 
    WHERE UserName = @UserName;
END;
GO

-- Procedimiento almacenado para obtener la seguridad del usuario por UserId
CREATE OR ALTER PROCEDURE GetUserSecurityByUserId
    @UserId INT
AS
BEGIN
    SELECT SecurityId, UserId, PasswordHash, IsActive 
    FROM UserSecurity 
    WHERE UserId = @UserId;
END;
GO
--user security 
-- Procedimiento almacenado para crear una entrada en UserSecurity
CREATE OR ALTER PROCEDURE CreateUserSecurity
    @UserId INT,
    @PasswordHash NVARCHAR(256),
    @IsActive BIT
AS
BEGIN
    INSERT INTO UserSecurity (UserId, PasswordHash, IsActive)
    VALUES (@UserId, @PasswordHash, @IsActive);
END;
GO

-- Procedimiento almacenado para obtener la seguridad del usuario por UserId
CREATE OR ALTER PROCEDURE GetUserSecurityByUserId
    @UserId INT
AS
BEGIN
    SELECT SecurityId, UserId, PasswordHash, IsActive 
    FROM UserSecurity 
    WHERE UserId = @UserId;
END;
GO

select * from Users