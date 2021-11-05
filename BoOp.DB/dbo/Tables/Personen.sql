CREATE TABLE [dbo].[Personen]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Vorname] NVARCHAR(50) NOT NULL, 
    [Nachname] NVARCHAR(50) NOT NULL, 
    [Geburtsdatum] DATETIME2 NOT NULL, 
    [Telefonnummer] VARCHAR(20) NULL, 
    [Rechte] INT NOT NULL, 
    [EMail] NVARCHAR(150) NULL, 
    [AusweisID] NVARCHAR(150) NULL, 
    [PasswortHASH] NVARCHAR(150) NULL
)
