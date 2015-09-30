
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/30/2015 11:25:10
-- Generated from EDMX file: D:\Work_vve\workspace_sharp_git\vvevgeny_myseen\My Seen\My Seen\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MySeen];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UsersFilms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FilmsSet] DROP CONSTRAINT [FK_UsersFilms];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSerials]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SerialsSet] DROP CONSTRAINT [FK_UsersSerials];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersFilms_New]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Films_NewSet] DROP CONSTRAINT [FK_UsersFilms_New];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSerials_New]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Serials_NewSet] DROP CONSTRAINT [FK_UsersSerials_New];
GO
IF OBJECT_ID(N'[dbo].[FK_UsersSync]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SyncSet] DROP CONSTRAINT [FK_UsersSync];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UsersSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UsersSet];
GO
IF OBJECT_ID(N'[dbo].[FilmsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FilmsSet];
GO
IF OBJECT_ID(N'[dbo].[SerialsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SerialsSet];
GO
IF OBJECT_ID(N'[dbo].[Films_NewSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Films_NewSet];
GO
IF OBJECT_ID(N'[dbo].[Serials_NewSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Serials_NewSet];
GO
IF OBJECT_ID(N'[dbo].[SyncSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SyncSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [NameRemote] nvarchar(max)  NULL,
    [PasswordRemote] nvarchar(max)  NULL
);
GO

-- Creating table 'FilmsSet'
CREATE TABLE [dbo].[FilmsSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateSee] datetime  NOT NULL,
    [Rate] int  NOT NULL,
    [DateChange] datetime  NOT NULL
);
GO

-- Creating table 'SerialsSet'
CREATE TABLE [dbo].[SerialsSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LastSeason] int  NOT NULL,
    [LastSeries] int  NOT NULL,
    [DateBegin] datetime  NOT NULL,
    [DateLast] datetime  NOT NULL,
    [Rate] int  NOT NULL,
    [DateChange] datetime  NOT NULL
);
GO

-- Creating table 'Films_NewSet'
CREATE TABLE [dbo].[Films_NewSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateSee] datetime  NOT NULL,
    [Rate] int  NOT NULL
);
GO

-- Creating table 'Serials_NewSet'
CREATE TABLE [dbo].[Serials_NewSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LastSeason] int  NOT NULL,
    [LastSeries] int  NOT NULL,
    [DateBegin] datetime  NOT NULL,
    [DateLast] datetime  NOT NULL,
    [Rate] int  NOT NULL
);
GO

-- Creating table 'SyncSet'
CREATE TABLE [dbo].[SyncSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UsersId] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UsersSet'
ALTER TABLE [dbo].[UsersSet]
ADD CONSTRAINT [PK_UsersSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FilmsSet'
ALTER TABLE [dbo].[FilmsSet]
ADD CONSTRAINT [PK_FilmsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SerialsSet'
ALTER TABLE [dbo].[SerialsSet]
ADD CONSTRAINT [PK_SerialsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Films_NewSet'
ALTER TABLE [dbo].[Films_NewSet]
ADD CONSTRAINT [PK_Films_NewSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Serials_NewSet'
ALTER TABLE [dbo].[Serials_NewSet]
ADD CONSTRAINT [PK_Serials_NewSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SyncSet'
ALTER TABLE [dbo].[SyncSet]
ADD CONSTRAINT [PK_SyncSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UsersId] in table 'FilmsSet'
ALTER TABLE [dbo].[FilmsSet]
ADD CONSTRAINT [FK_UsersFilms]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersFilms'
CREATE INDEX [IX_FK_UsersFilms]
ON [dbo].[FilmsSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'SerialsSet'
ALTER TABLE [dbo].[SerialsSet]
ADD CONSTRAINT [FK_UsersSerials]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSerials'
CREATE INDEX [IX_FK_UsersSerials]
ON [dbo].[SerialsSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'Films_NewSet'
ALTER TABLE [dbo].[Films_NewSet]
ADD CONSTRAINT [FK_UsersFilms_New]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersFilms_New'
CREATE INDEX [IX_FK_UsersFilms_New]
ON [dbo].[Films_NewSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'Serials_NewSet'
ALTER TABLE [dbo].[Serials_NewSet]
ADD CONSTRAINT [FK_UsersSerials_New]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSerials_New'
CREATE INDEX [IX_FK_UsersSerials_New]
ON [dbo].[Serials_NewSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'SyncSet'
ALTER TABLE [dbo].[SyncSet]
ADD CONSTRAINT [FK_UsersSync]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSync'
CREATE INDEX [IX_FK_UsersSync]
ON [dbo].[SyncSet]
    ([UsersId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------