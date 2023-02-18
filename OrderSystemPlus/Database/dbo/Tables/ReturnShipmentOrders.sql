CREATE TABLE [dbo].[ReturnShipmentOrders] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Number]          VARCHAR (100)   NULL,
    [ShipmentOrderId] INT             NULL,
    [Price]           DECIMAL (18, 4) NULL,
    [ReturnDate]      DATETIME2 (7)   NULL,
    [Remarks]         NVARCHAR (500)  NULL,
    [IsDeleted]       BIT             NULL,
    [UpdatedAt]       DATETIME2 (7)   NOT NULL,
    [CreatedAt]       DATETIME2 (7)   NOT NULL,
    [Total]           DECIMAL (18, 4) NULL,
    CONSTRAINT [PK_ReturnShipmentOrders] PRIMARY KEY CLUSTERED ([Id] ASC)
);

