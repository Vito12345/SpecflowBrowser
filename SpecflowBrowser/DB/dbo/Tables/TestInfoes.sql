CREATE TABLE [dbo].[TestInfoes] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Result]          NVARCHAR (MAX)   NULL,
    [DateStart]       DATETIME         NULL,
    [DateEnd]         DATETIME         NULL,
    [Duree]           TIME (7)         NULL,
    [ConsoleOutput]   NVARCHAR (MAX)   NULL,
    [ErrorMessage]    NVARCHAR (MAX)   NULL,
    [ErrorStacktrace] NVARCHAR (MAX)   NULL,
    [Scenario_Id]     UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_TestInfoes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ScenarioTestInfo] FOREIGN KEY ([Scenario_Id]) REFERENCES [dbo].[Scenarios] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ScenarioTestInfo]
    ON [dbo].[TestInfoes]([Scenario_Id] ASC);

