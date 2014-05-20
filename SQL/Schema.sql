CREATE TABLE [dbo].[amp_ContentType] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [IsEnabled]          BIT              NOT NULL,
    [Name]               NVARCHAR (64)    NOT NULL,
    [SupportsVersioning] BIT              NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[amp_AllowedContentType] (
    [ContentTypeId]        UNIQUEIDENTIFIER NOT NULL,
    [AllowedContentTypeId] UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([AllowedContentTypeId] ASC, [ContentTypeId] ASC),
    CONSTRAINT [fk_ContentType_AllowedContentType] FOREIGN KEY ([ContentTypeId]) REFERENCES [dbo].[amp_ContentType] ([Id]),
    CONSTRAINT [fk_AllowedContentType_ContentType] FOREIGN KEY ([AllowedContentTypeId]) REFERENCES [dbo].[amp_ContentType] ([Id])
);

CREATE TABLE [dbo].[amp_Content] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [TypeId]            UNIQUEIDENTIFIER NOT NULL,
    [Title]             NVARCHAR (256)   NOT NULL,
    [HtmlDescription]   NVARCHAR (MAX)   NULL,
    [CreateDateUtc]     DATETIME         NOT NULL,
    [LastUpdateDateUtc] DATETIME         NOT NULL,
    [Author]            NVARCHAR (64)    NOT NULL,
    [IsActive]          BIT              NOT NULL,
    [ParentId]          UNIQUEIDENTIFIER NULL,
    [IsApproved]        BIT              NOT NULL
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_ContentType_Content] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[amp_ContentType] ([Id]),
    CONSTRAINT [fk_Content_Content] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[amp_Content] ([Id])
);


CREATE TABLE [dbo].[amp_Content_History] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
    [TypeId]            UNIQUEIDENTIFIER NOT NULL,
    [Title]             NVARCHAR (256)   NOT NULL,
    [HtmlDescription]   NVARCHAR (MAX)   NULL,
    [CreateDateUtc]     DATETIME         NOT NULL,
    [LastUpdateDateUtc] DATETIME         NOT NULL,
    [Author]            NVARCHAR (64)    NOT NULL,
    [ParentId]          UNIQUEIDENTIFIER NULL,
    [IsApproved]        BIT              NOT NULL
    PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [fk_ContentType_Content_history] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[amp_ContentType] ([Id])
);

CREATE TABLE [dbo].[amp_Page] (
    [Id]                UNIQUEIDENTIFIER NOT NULL,
	[URL]               NVARCHAR(256)    NULL,
	[IsHomePage] BIT NOT NULL DEFAULT(0),
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_Content_Page] FOREIGN KEY (Id) REFERENCES [dbo].[amp_Content] ([Id])
);