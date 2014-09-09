CREATE TABLE [dbo].[Etapes] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [Keyword]           NVARCHAR (MAX)   NOT NULL,
    [NativeKeyword]     NVARCHAR (MAX)   NOT NULL,
    [DocString]         NVARCHAR (MAX)   NULL,
    [ScenarioId]        UNIQUEIDENTIFIER NOT NULL,
    [ScenarioOutlineId] UNIQUEIDENTIFIER NOT NULL,
    [Nom]               NVARCHAR (MAX)   NOT NULL,
    [Table_Id]          UNIQUEIDENTIFIER NULL,
    [EtapeIndex]        INT              NOT NULL,
    CONSTRAINT [PK_Etapes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_EtapeTable] FOREIGN KEY ([Table_Id]) REFERENCES [dbo].[Tables] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ScenarioEtape] FOREIGN KEY ([ScenarioId]) REFERENCES [dbo].[Scenarios] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ScenarioEtape]
    ON [dbo].[Etapes]([ScenarioId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_EtapeTable]
    ON [dbo].[Etapes]([Table_Id] ASC);

