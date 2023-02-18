CREATE TABLE [dbo].[ProductInventory] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [ProductId]   INT             NULL,
    [Unit]        DECIMAL (18, 4) NULL,
    [Description] VARCHAR (500)   NULL,
    [CreatedAt]   DATETIME2 (7)   CONSTRAINT [DF_ProductInventory_created_at] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_productInventory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

