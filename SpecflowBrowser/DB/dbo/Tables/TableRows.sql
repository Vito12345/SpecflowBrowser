CREATE TABLE [dbo].[TableRows] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [TableId]        UNIQUEIDENTIFIER NULL,
    [TableHeader_Id] UNIQUEIDENTIFIER NULL,
    [RowIndex]       INT              NOT NULL,
    CONSTRAINT [PK_TableRows] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TableTableRow] FOREIGN KEY ([TableId]) REFERENCES [dbo].[Tables] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_TableTableRow1] FOREIGN KEY ([TableHeader_Id]) REFERENCES [dbo].[Tables] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_TableTableRow1]
    ON [dbo].[TableRows]([TableHeader_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_TableTableRow]
    ON [dbo].[TableRows]([TableId] ASC);

