CREATE TABLE [dbo].[ShipmentOrders] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Number]       VARCHAR (100)   NOT NULL,
    [Type]         INT             NULL,
    [Total]        DECIMAL (18, 4) NULL,
    [SignName]     VARCHAR (100)   NULL,
    [Status]       INT             NULL,
    [FinishDate]   DATETIME2 (7)   NULL,
    [DeliveryDate] DATETIME2 (7)   NULL,
    [Address]      VARCHAR (500)   NULL,
    [Remarks]      VARCHAR (500)   NULL,
    [UpdateAt]     DATETIME2 (7)   CONSTRAINT [DF_Orders_UpdateTime] DEFAULT (getdate()) NOT NULL,
    [CreatedAt]    DATETIME2 (7)   CONSTRAINT [DF_Orders_CreateTime] DEFAULT (getdate()) NOT NULL,
    [IsDeleted]    BIT             NULL,
    CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED ([Id] ASC)
);

