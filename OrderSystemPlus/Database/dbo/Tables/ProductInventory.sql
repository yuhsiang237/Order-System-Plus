CREATE TABLE [dbo].[ProductInventory] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [ProductId]   INT             NULL,
    [AdjustQuantity]       DECIMAL (18, 4) NULL,
    [PrevTotalQuantity]       DECIMAL (18, 4) NULL,
    [TotalQuantity]        DECIMAL (18, 4) NULL,
    [AdjustProductInventoryType] INT NULL, 
    [Description] VARCHAR (500)   NULL,
    [CreatedOn]  DATETIME   NOT NULL,
    [UpdatedOn] DATETIME   NOT NULL,
    [IsValid] BIT            NOT NULL,
    CONSTRAINT [PK_productInventory] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = 'AdjustQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'異動型別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'AdjustProductInventoryType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'描述',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'建立時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'CreatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'更新時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否生效',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = N'IsValid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整前總數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = 'PrevTotalQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'調整後總數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ProductInventory',
    @level2type = N'COLUMN',
    @level2name = 'TotalQuantity'