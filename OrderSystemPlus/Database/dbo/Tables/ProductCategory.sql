CREATE TABLE [dbo].[ProductCategory] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (100) NULL,
    [Description] NVARCHAR (500) NULL,
    [CreatedAt]   DATETIME2 (7)  CONSTRAINT [DF_ProductCategory_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]   DATETIME2 (7)  CONSTRAINT [DF_ProductCategory_UpdatedAt] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]   BIT            NULL,
    CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

