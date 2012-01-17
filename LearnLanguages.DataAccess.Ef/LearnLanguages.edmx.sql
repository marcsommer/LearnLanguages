
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/16/2012 20:33:32
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

IF OBJECT_ID(N'[dbo].[FK_AssociateLanguageDataWithPhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_AssociateLanguageDataWithPhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithPhraseData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PhraseDatas] DROP CONSTRAINT [FK_AssociationUserDataWithPhraseData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithLanguageData_UserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithLanguageData] DROP CONSTRAINT [FK_AssociationUserDataWithLanguageData_UserData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithLanguageData_LanguageData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithLanguageData] DROP CONSTRAINT [FK_AssociationUserDataWithLanguageData_LanguageData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithRoleData_UserData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithRoleData] DROP CONSTRAINT [FK_AssociationUserDataWithRoleData_UserData];
GO
IF OBJECT_ID(N'[dbo].[FK_AssociationUserDataWithRoleData_RoleData]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AssociationUserDataWithRoleData] DROP CONSTRAINT [FK_AssociationUserDataWithRoleData_RoleData];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LanguageDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LanguageDatas];
GO
IF OBJECT_ID(N'[dbo].[PhraseDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhraseDatas];
GO
IF OBJECT_ID(N'[dbo].[UserDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserDatas];
GO
IF OBJECT_ID(N'[dbo].[RoleDatas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleDatas];
GO
IF OBJECT_ID(N'[dbo].[AssociationUserDataWithLanguageData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssociationUserDataWithLanguageData];
GO
IF OBJECT_ID(N'[dbo].[AssociationUserDataWithRoleData]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssociationUserDataWithRoleData];
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
    [LanguageDataId] uniqueidentifier  NOT NULL,
    [UserDataId] uniqueidentifier  NOT NULL,
    [UserDataUsername] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserDatas'
CREATE TABLE [dbo].[UserDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Salt] int  NOT NULL,
    [SaltedHashedPasswordValue] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RoleDatas'
CREATE TABLE [dbo].[RoleDatas] (
    [Id] uniqueidentifier  NOT NULL,
    [Text] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AssociationUserDataWithLanguageData'
CREATE TABLE [dbo].[AssociationUserDataWithLanguageData] (
    [AssociationUserDataWithLanguageData_LanguageData_Id] uniqueidentifier  NOT NULL,
    [LanguageDatas_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'AssociationUserDataWithRoleData'
CREATE TABLE [dbo].[AssociationUserDataWithRoleData] (
    [AssociationUserDataWithRoleData_RoleData_Id] uniqueidentifier  NOT NULL,
    [RoleDatas_Id] uniqueidentifier  NOT NULL
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

-- Creating primary key on [Id] in table 'UserDatas'
ALTER TABLE [dbo].[UserDatas]
ADD CONSTRAINT [PK_UserDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleDatas'
ALTER TABLE [dbo].[RoleDatas]
ADD CONSTRAINT [PK_RoleDatas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AssociationUserDataWithLanguageData_LanguageData_Id], [LanguageDatas_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [PK_AssociationUserDataWithLanguageData]
    PRIMARY KEY NONCLUSTERED ([AssociationUserDataWithLanguageData_LanguageData_Id], [LanguageDatas_Id] ASC);
GO

-- Creating primary key on [AssociationUserDataWithRoleData_RoleData_Id], [RoleDatas_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [PK_AssociationUserDataWithRoleData]
    PRIMARY KEY NONCLUSTERED ([AssociationUserDataWithRoleData_RoleData_Id], [RoleDatas_Id] ASC);
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

-- Creating foreign key on [UserDataId] in table 'PhraseDatas'
ALTER TABLE [dbo].[PhraseDatas]
ADD CONSTRAINT [FK_AssociationUserDataWithPhraseData]
    FOREIGN KEY ([UserDataId])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithPhraseData'
CREATE INDEX [IX_FK_AssociationUserDataWithPhraseData]
ON [dbo].[PhraseDatas]
    ([UserDataId]);
GO

-- Creating foreign key on [AssociationUserDataWithLanguageData_LanguageData_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [FK_AssociationUserDataWithLanguageData_UserData]
    FOREIGN KEY ([AssociationUserDataWithLanguageData_LanguageData_Id])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [LanguageDatas_Id] in table 'AssociationUserDataWithLanguageData'
ALTER TABLE [dbo].[AssociationUserDataWithLanguageData]
ADD CONSTRAINT [FK_AssociationUserDataWithLanguageData_LanguageData]
    FOREIGN KEY ([LanguageDatas_Id])
    REFERENCES [dbo].[LanguageDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithLanguageData_LanguageData'
CREATE INDEX [IX_FK_AssociationUserDataWithLanguageData_LanguageData]
ON [dbo].[AssociationUserDataWithLanguageData]
    ([LanguageDatas_Id]);
GO

-- Creating foreign key on [AssociationUserDataWithRoleData_RoleData_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [FK_AssociationUserDataWithRoleData_UserData]
    FOREIGN KEY ([AssociationUserDataWithRoleData_RoleData_Id])
    REFERENCES [dbo].[UserDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleDatas_Id] in table 'AssociationUserDataWithRoleData'
ALTER TABLE [dbo].[AssociationUserDataWithRoleData]
ADD CONSTRAINT [FK_AssociationUserDataWithRoleData_RoleData]
    FOREIGN KEY ([RoleDatas_Id])
    REFERENCES [dbo].[RoleDatas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AssociationUserDataWithRoleData_RoleData'
CREATE INDEX [IX_FK_AssociationUserDataWithRoleData_RoleData]
ON [dbo].[AssociationUserDataWithRoleData]
    ([RoleDatas_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------