CREATE TABLE [dbo].[ProductProductCategoryRelationships] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [ProductId]         INT NULL,
    [ProductCategoryId] INT NULL,
    CONSTRAINT [PK_ProductProductCategoryRelationships] PRIMARY KEY CLUSTERED ([Id] ASC)
);

