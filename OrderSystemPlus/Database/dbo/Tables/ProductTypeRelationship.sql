CREATE TABLE [dbo].[ProductTypeRelationship] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [ProductId]         INT NULL,
    [ProductTypeId] INT NULL,
    CONSTRAINT [PK_ProductTypeRelationship] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO