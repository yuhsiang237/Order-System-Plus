CREATE TABLE [dbo].[Users] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (100) NULL,
    [Salt]      VARCHAR (50)   NULL,
    [Email]     NVARCHAR (100) NULL,
    [Account]   NVARCHAR (100) NULL,
    [Password]  NVARCHAR (500) NULL,
    [RoleId]    INT            NULL,
    [CreateAt]  DATETIME2 (7)  CONSTRAINT [DF_Users_CreateAt] DEFAULT (getdate()) NULL,
    [UpdatedAt] DATETIME2 (7)  CONSTRAINT [DF_Users_UpdatedAt] DEFAULT (getdate()) NULL,
    [IsDeleted] BIT            NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

