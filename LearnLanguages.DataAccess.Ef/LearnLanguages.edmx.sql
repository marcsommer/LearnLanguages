
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/09/2012 22:31:40
-- Generated from EDMX file: C:\Users\User\Documents\Visual Studio 2010\Projects\LearnLanguages\LearnLanguages.DataAccess.Ef\LearnLanguages.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LearnLanguagesDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LanguageDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LanguageDatas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LanguageDatas'
CREATE TABLE [dbo].[LanguageDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PhraseDatas'
CREATE TABLE [dbo].[PhraseDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [LanguageDataId] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LanguageDatas'
ALTER TABLE [dbo].[LanguageDatas]
ADD CONSTRAINT [PK_LanguageDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [PK_PhraseDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LanguageDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_AssociateLanguageDataWithPhraseData]
    FOREIGN KEY ([LanguageDataId])
    REFERENCES [dbo].[LanguageDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociateLanguageDataWithPhraseData'
CREATE INDEX [IX_FK_AssociateLanguageDataWithPhraseData]
ON [dbo].[PhraseDatas]
    ([LanguageDataId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------