
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/10/2015 14:34:28
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

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UsersSet'
CREATE TABLE [dbo].[UsersSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Email] nvarchar(max)  NULL
);
GO

-- Creating table 'FilmsSet'
CREATE TABLE [dbo].[FilmsSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Id_R] int  NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateSee] datetime  NOT NULL,
    [Rating] int  NOT NULL,
    [Genre] int  NOT NULL,
    [DateChange] datetime  NOT NULL,
    [isDeleted] bit  NULL
);
GO

-- Creating table 'SerialsSet'
CREATE TABLE [dbo].[SerialsSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Id_R] int  NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [LastSeason] int  NOT NULL,
    [LastSeries] int  NOT NULL,
    [DateBegin] datetime  NOT NULL,
    [DateLast] datetime  NOT NULL,
    [Rating] int  NOT NULL,
    [Genre] int  NOT NULL,
    [DateChange] datetime  NOT NULL,
    [isDeleted] bit  NULL
);
GO

-- Creating table 'BooksSet'
CREATE TABLE [dbo].[BooksSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Id_R] nvarchar(max)  NULL,
    [UsersId] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Authors] nvarchar(max)  NOT NULL,
    [DateRead] datetime  NOT NULL,
    [Genre] int  NOT NULL,
    [Rating] int  NOT NULL,
    [DateChange] datetime  NOT NULL,
    [isDeleted] bit  NULL
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

-- Creating primary key on [Id] in table 'BooksSet'
ALTER TABLE [dbo].[BooksSet]
ADD CONSTRAINT [PK_BooksSet]
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
    ON DELETE CASCADE ON UPDATE NO ACTION;
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
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UsersSerials'
CREATE INDEX [IX_FK_UsersSerials]
ON [dbo].[SerialsSet]
    ([UsersId]);
GO

-- Creating foreign key on [UsersId] in table 'BooksSet'
ALTER TABLE [dbo].[BooksSet]
ADD CONSTRAINT [FK_BooksUsers]
    FOREIGN KEY ([UsersId])
    REFERENCES [dbo].[UsersSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BooksUsers'
CREATE INDEX [IX_FK_BooksUsers]
ON [dbo].[BooksSet]
    ([UsersId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------