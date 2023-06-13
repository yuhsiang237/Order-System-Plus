CREATE TABLE [dbo].[ProductTypeRelationship] (
    [ProductId]     INT NOT NULL,
    [ProductTypeId] INT NOT NULL
);




GO
CREATE NONCLUSTERED INDEX [IX_ProductTypeRelationship]
    ON [dbo].[ProductTypeRelationship]([ProductId] ASC);

