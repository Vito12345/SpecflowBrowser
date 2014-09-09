CREATE TABLE [dbo].[Tags] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [TagName]          NVARCHAR (MAX)   NOT NULL,
    [ScenarioId]       UNIQUEIDENTIFIER NULL,
    [FonctionnaliteId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FonctionnaliteTag] FOREIGN KEY ([FonctionnaliteId]) REFERENCES [dbo].[Fonctionnalites] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ScenarioTag] FOREIGN KEY ([ScenarioId]) REFERENCES [dbo].[Scenarios] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ScenarioTag]
    ON [dbo].[Tags]([ScenarioId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_FonctionnaliteTag]
    ON [dbo].[Tags]([FonctionnaliteId] ASC);

