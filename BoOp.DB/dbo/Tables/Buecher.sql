-- Erstellt von Manuel Janzen
-- Bearbeitet von Manuel Janzen, Dominik von Michalkowsky
CREATE TABLE [dbo].[Buecher]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Titel] NVARCHAR(50) NOT NULL, 
    [Author] NVARCHAR(50) NOT NULL, 
    [Verlag] NVARCHAR(50) NULL,
    [Auflage] INT NULL, 
    [ISBN] VARCHAR(50) NOT NULL, 
    [Altersvorschlag] VARCHAR(20) NULL, 
    [Regal] VARCHAR(50) NULL
)
