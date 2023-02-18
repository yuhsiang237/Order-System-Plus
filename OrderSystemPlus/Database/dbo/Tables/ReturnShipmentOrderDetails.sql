CREATE TABLE [dbo].[ReturnShipmentOrderDetails] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [ReturnShipmentOrderId] INT             NULL,
    [ShipmentOrderDetailId] INT             NULL,
    [Unit]                  DECIMAL (18, 4) NULL,
    [IsDeleted]             BIT             NULL,
    [CreatedAt]             DATETIME2 (7)   NOT NULL,
    [UpdateAt]              DATETIME2 (7)   NOT NULL,
    [Remarks]               VARCHAR (500)   NULL,
    CONSTRAINT [PK_ReturnShipmentOrderDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

