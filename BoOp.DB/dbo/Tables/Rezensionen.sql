-- Erstellt von Manuel Janzen
-- Bearbeitet von Manuel Janzen, Dominik von Michalkowsky
CREATE TABLE [dbo].[Rezensionen]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BuchID] INT NOT NULL,
    [Sterne] INT NOT NULL, 
    [Rezensionstext] NVARCHAR(MAX) NULL, 
    [PersonID] INT NULL
)
