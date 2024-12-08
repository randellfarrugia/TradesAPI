
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TradingDb')
BEGIN
    CREATE DATABASE TradingDb;
    WAITFOR DELAY '00:00:01';
END;

DECLARE @sql NVARCHAR(MAX);

SET @sql = '
USE TradingDb;

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = ''Trades'' AND xtype = ''U'')
BEGIN
    CREATE TABLE Trades (
        [Id] uniqueidentifier PRIMARY KEY,
        [User] NVARCHAR(50) NULL,
        [CurrencyCode] NVARCHAR(50) NULL,
        [Amount] DECIMAL(18, 0) NULL,
        [Fee] DECIMAL(18, 0) NULL,
        [TradeDate] DATETIME NULL DEFAULT GETDATE()
    );
END;';

EXEC sp_executesql @sql;
