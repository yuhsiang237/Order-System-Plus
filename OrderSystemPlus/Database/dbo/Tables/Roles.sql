CREATE TABLE [dbo].[Roles] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NULL,
    [CreatedAt] DATETIME2 (7)  NULL,
    [UpdatedAt] DATETIME2 (7)  CONSTRAINT [DF_Roles_UpdatedAt] DEFAULT (getdate()) NULL,
    [IsDeleted] BIT            NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

