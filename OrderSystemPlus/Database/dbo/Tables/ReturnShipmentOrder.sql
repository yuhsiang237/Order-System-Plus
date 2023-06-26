CREATE TABLE [dbo].[ReturnShipmentOrder] (
    [ReturnShipmentOrderNumber] VARCHAR (100)   NOT NULL,
    [ShipmentOrderNumber]       VARCHAR (100)   NOT NULL,
    [TotalReturnAmount]         DECIMAL (18, 4) NULL,
    [ReturnDate]                DATETIME        NULL,
    [Remark]                    NVARCHAR (500)  NULL,
    [IsValid]                   BIT             NOT NULL,
    [CreatedOn]                 DATETIME        NOT NULL,
    [UpdatedOn]                 DATETIME        NOT NULL,
    CONSTRAINT [PK_ReturnShipmentOrder] PRIMARY KEY CLUSTERED ([ReturnShipmentOrderNumber] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'UpdatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'CreatedOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否生效', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'IsValid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退貨日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'ReturnDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退貨總金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'TotalReturnAmount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進貨單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'ShipmentOrderNumber';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退貨單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ReturnShipmentOrder', @level2type = N'COLUMN', @level2name = N'ReturnShipmentOrderNumber';

