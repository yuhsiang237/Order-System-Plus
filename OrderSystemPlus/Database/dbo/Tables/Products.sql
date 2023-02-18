CREATE TABLE [dbo].[Products] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Number]      NVARCHAR (100)  NULL,
    [Name]        NVARCHAR (100)  NULL,
    [Price]       DECIMAL (18, 4) NULL,
    [Description] VARCHAR (500)   NULL,
    [CurrentUnit] DECIMAL (18, 4) NULL,
    [CreatedAt]   DATETIME2 (7)   CONSTRAINT [DF_Products_CreatedAt] DEFAULT (getdate()) NULL,
    [UpdatedAt]   DATETIME2 (7)   CONSTRAINT [DF_Products_UpdatedAt] DEFAULT (getdate()) NULL,
    [IsDeleted]   BIT             NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);

