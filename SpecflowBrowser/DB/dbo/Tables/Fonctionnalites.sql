CREATE TABLE [dbo].[Fonctionnalites] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Nom]             NVARCHAR (MAX)   NOT NULL,
    [Description]     NVARCHAR (MAX)   NOT NULL,
    [AnalyseProjetId] UNIQUEIDENTIFIER NOT NULL,
    [AnalyseDate]     DATETIME         NOT NULL,
    [TestResult]      NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Fonctionnalites] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AnalyseFonctionnalite] FOREIGN KEY ([AnalyseProjetId], [AnalyseDate]) REFERENCES [dbo].[Analyses] ([ProjetId], [Date]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AnalyseFonctionnalite]
    ON [dbo].[Fonctionnalites]([AnalyseProjetId] ASC, [AnalyseDate] ASC);

