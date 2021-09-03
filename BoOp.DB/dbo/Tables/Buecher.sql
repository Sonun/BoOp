CREATE TABLE [dbo].[Buecher]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [BuchGenre_ID] INT NULL, 
    [BuchSchlagwoerter_ID] INT NULL, 
    [Person_ID] INT NULL, 
    [Titel] NVARCHAR(50) NOT NULL, 
    [Author] NVARCHAR(50) NOT NULL, 
    [Verlag] NVARCHAR(50) NOT NULL, 
    [Auflage] INT NULL, 
    [ISBN] VARCHAR(50) NULL, 
    [Altersvorschlag] VARCHAR(20) NULL, 
    [Regal] VARCHAR(50) NULL
)
