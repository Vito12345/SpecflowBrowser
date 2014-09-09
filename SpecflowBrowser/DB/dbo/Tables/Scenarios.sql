CREATE TABLE [dbo].[Scenarios] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [FonctionnaliteId]   UNIQUEIDENTIFIER NULL,
    [Nom]                NVARCHAR (MAX)   NOT NULL,
    [Description]        NVARCHAR (MAX)   NOT NULL,
    [Fonctionnalite1_Id] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Scenarios] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FonctionnaliteScenario] FOREIGN KEY ([FonctionnaliteId]) REFERENCES [dbo].[Fonctionnalites] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_FonctionnaliteScenario1] FOREIGN KEY ([Fonctionnalite1_Id]) REFERENCES [dbo].[Fonctionnalites] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_FonctionnaliteScenario1]
    ON [dbo].[Scenarios]([Fonctionnalite1_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_FonctionnaliteScenario]
    ON [dbo].[Scenarios]([FonctionnaliteId] ASC);

