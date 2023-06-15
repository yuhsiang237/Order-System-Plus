CREATE TABLE [dbo].[ShipmentOrder] (
    [OrderNumber]    VARCHAR (100)   NOT NULL,
    [OrderType]      INT             NULL,
    [TotalAmount]    DECIMAL (18, 4) NULL,
    [RecipientName]  NVARCHAR (100)  NULL,
    [OperatorUserId] NVARCHAR (100)  NULL,
    [Status]         INT             NULL,
    [FinishDate]     DATETIME        NULL,
    [DeliveryDate]   DATETIME        NULL,
    [Address]        NVARCHAR (500)  NULL,
    [Remark]         NVARCHAR (500)  NULL,
    [CreatedOn]      DATETIME        NOT NULL,
    [UpdatedOn]      DATETIME        NOT NULL,
    [IsValid]        BIT             NOT NULL
);




GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否為生效資料',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'IsValid'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'系統建立時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'CreatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'系統更新時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'UpdatedOn'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨地址',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'Address'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'出貨日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'DeliveryDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'完成日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'FinishDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'Status'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'簽收人名稱',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = 'RecipientName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單總額',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'TotalAmount'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'訂單類別',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = 'OrderType'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'貨單編號',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = 'OrderNumber'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'操作人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ShipmentOrder',
    @level2type = N'COLUMN',
    @level2name = N'OperatorUserId'
GO
CREATE NONCLUSTERED INDEX [IX_ShipmentOrder]
    ON [dbo].[ShipmentOrder]([OrderNumber] ASC);

