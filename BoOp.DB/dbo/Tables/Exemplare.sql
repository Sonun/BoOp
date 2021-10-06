CREATE TABLE [dbo].[Exemplare]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BuchID] INT NOT NULL, 
    [Barcode] NVARCHAR(50) NOT NULL, 
    [AusleiherID] INT NULL, 
    [AusleihDatum] DATETIME2 NULL
)