CREATE TABLE [dbo].[ShipmentOrderDetails] (
    [Id]            INT             IDENTITY (1, 1) NOT NULL,
    [OrderId]       INT             NULL,
    [ProductId]     INT             NULL,
    [ProductNumber] NVARCHAR (100)  NULL,
    [ProductName]   NVARCHAR (100)  NULL,
    [ProductPrice]  DECIMAL (18, 4) NULL,
    [ProductUnit]   DECIMAL (18, 4) NULL,
    [Remarks]       NVARCHAR (500)  NULL,
    [CreatedAt]     DATETIME2 (7)   CONSTRAINT [DF_OrderDetails_CreatedAt_1] DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]     DATETIME2 (7)   CONSTRAINT [DF_OrderDetails_UpdateAt] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]     BIT             NULL,
    CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

