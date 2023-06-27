CREATE TABLE [dbo].[ShipmentOrderDetail] (
    [Id]              INT IDENTITY(1,1) NOT NULL,
    [OrderNumber]     VARCHAR (100)   NOT NULL,
    [ProductId]       INT             NULL,
    [ProductNumber]   NVARCHAR (100)  NULL,
    [ProductName]     NVARCHAR (100)  NULL,
    [ProductPrice]    DECIMAL (18, 4) NULL,
    [ProductQuantity] DECIMAL (18, 4) NULL,
    [Remarks]         NVARCHAR (500)  NULL,
    [CreatedOn]       DATETIME        NOT NULL,
    [UpdatedOn]       DATETIME        NOT NULL,
    [IsValid]         BIT             NOT NULL, 
    CONSTRAINT [PK_ShipmentOrderDetail] PRIMARY KEY ([Id])
);




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'OrderNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品價錢',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductPrice'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'商品單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = 'ProductQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'Remarks'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'系統建立時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'CreatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'系統更新時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否生效',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'IsValid'
GO
CREATE NONCLUSTERED INDEX [IX_ShipmentOrderDetail]
    ON [dbo].[ShipmentOrderDetail]([OrderNumber] ASC);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'Id'