CREATE TABLE [dbo].[ReturnShipmentOrderDetail] (
    [Id]              INT IDENTITY(1,1) NOT NULL,
    [ReturnShipmentOrderNumber]     VARCHAR (100)   NOT NULL,
    [ShipmentOrderDetailId]     INT   NOT NULL,
    [ProductId]       INT             NULL,
    [ProductQuantity] DECIMAL (18, 4) NULL,
    [Remarks]         NVARCHAR (500)  NULL,
    [CreatedOn]       DATETIME        NOT NULL,
    [UpdatedOn]       DATETIME        NOT NULL,
    [IsValid]         BIT             NOT NULL, 
    CONSTRAINT [PK_ReturnShipmentOrderDetail] PRIMARY KEY ([Id])
);




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'退貨單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ReturnShipmentOrderNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'流水號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'產品ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductId'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'退貨產品數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ProductQuantity'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'Remarks'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'建立時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'CreatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'更新時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否生效',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'IsValid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'進貨單明細ID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ReturnShipmentOrderDetail',
    @level2type = N'COLUMN',
    @level2name = N'ShipmentOrderDetailId'