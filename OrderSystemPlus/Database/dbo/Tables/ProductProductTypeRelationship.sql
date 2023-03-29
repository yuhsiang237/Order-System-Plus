CREATE TABLE [dbo].[ProductProductTypeRelationship] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [ProductId]         INT NULL,
    [ProductTypeId] INT NULL,
    CONSTRAINT [PK_ProductProductTypeRelationship] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產品ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductProductTypeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產品類別ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductProductTypeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'ProductTypeId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductProductTypeRelationship',
    @level2type = N'COLUMN',
    @level2name = N'Id'