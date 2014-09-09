CREATE TABLE [dbo].[Analyses] (
    [ProjetId]  UNIQUEIDENTIFIER NOT NULL,
    [Date]      DATETIME         NOT NULL,
    [ProjetId1] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Analyses] PRIMARY KEY CLUSTERED ([ProjetId] ASC, [Date] ASC),
    CONSTRAINT [FK_ProjetAnalysis] FOREIGN KEY ([ProjetId1]) REFERENCES [dbo].[Projets] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ProjetAnalysis]
    ON [dbo].[Analyses]([ProjetId1] ASC);

