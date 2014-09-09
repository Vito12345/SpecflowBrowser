﻿CREATE TABLE [dbo].[Projets] (
    [Id]      UNIQUEIDENTIFIER NOT NULL,
    [Nom]     NVARCHAR (MAX)   NOT NULL,
    [Version] NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Projets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

