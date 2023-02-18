CREATE TABLE [dbo].[Permissions] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [RoleId] INT            NULL,
    [Code]   NVARCHAR (200) NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC)
);

