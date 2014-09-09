CREATE TABLE [dbo].[TableColonnes] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Valeur]     NVARCHAR (MAX)   NOT NULL,
    [TableRowId] UNIQUEIDENTIFIER NOT NULL,
    [ColIndex]   INT              NOT NULL,
    CONSTRAINT [PK_TableColonnes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TableRowTableColonne] FOREIGN KEY ([TableRowId]) REFERENCES [dbo].[TableRows] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_TableRowTableColonne]
    ON [dbo].[TableColonnes]([TableRowId] ASC);

