CREATE TABLE [dbo].[BuchSchlagwoerter]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [BuchID] INT NOT NULL, 
    [SchlagwortID] INT NOT NULL
)
