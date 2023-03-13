CREATE TABLE [dbo].[ProductInventory] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [ProductId]   INT             NULL,
    [Quantity]        DECIMAL (18, 4) NULL,
    [Description] VARCHAR (500)   NULL,
    [CreatedOn]  DATETIME   NOT NULL,
    [UpdatedOn] DATETIME   NOT NULL,
    [IsValid] BIT            NOT NULL,
    CONSTRAINT [PK_productInventory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

