IF (OBJECT_ID('ampPage') IS  NULL)
BEGIN
   CREATE TABLE ampPage(
          Id uniqueidentifier not null primary key,
          Title nvarchar(256) not null,
          HtmlDescription nvarchar(max) not null,
          CreateDateUtc datetime not null,
          LastUpdateDateUtc datetime not null,
          PublishDateUtc datetime null,
          Author nvarchar(32) null,
          IsActive bit not null default(1),
          IsApproved bit not null default(0),
          Slug nvarchar(256) not null,
          [Path] nvarchar(512) null,
          MetaKeywords nvarchar(512) null,
          MetaDescription nvarchar(512) null,
          ParentId uniqueidentifier null
		constraint fk_page_parentpage foreign key (ParentId) references ampPage(Id)
   )

   insert into ampPage(
          Id,
          Title,
          HtmlDescription,
          CreateDateUtc,
          LastUpdateDateUtc,
          PublishDateUtc,
          Author,
          IsActive,
          IsApproved,
          Slug,
          [Path],
          MetaKeywords,
          MetaDescription,
          ParentId
   )
   values
   (
	'9026ee12-6e29-46cc-ba87-72defd40754f',
	'Welcome to Amphiprion CMS',
	'<p>This is your site''s home page</p>',
	getutcdate(),
	getutcdate(),
	getutcdate(),
	'admin',
	1,
	1,
	'home',
	null,
	null,
	null,
	null
   )
END

IF (OBJECT_ID('ampUser') IS  NULL)
BEGIN
	create table ampUser
	(
		Id uniqueidentifier not null primary key,
		Email nvarchar(128) not null,
		UserName nvarchar(64) not null,
		IsActive bit not null default(1),
		PasswordHash nvarchar(128) null
	)
END
IF (OBJECT_ID('ampRole') IS  NULL)
BEGIN
	create table ampRole
	(
		Id nvarchar(64) not null primary key,
		Description nvarchar(512) null
	)
END
IF (OBJECT_ID('ampUserRole') IS  NULL)
BEGIN
	create table ampUserRole
	(
		RoleId nvarchar(64) not null,
		UserId uniqueidentifier not null,
		constraint pk_role_user primary key (RoleId,UserId),
		constraint fk_role_role foreign key (RoleId) references ampRole (Id) on delete cascade,
		constraint fl_roleuser_user foreign key (UserId) references ampUser(Id) on delete cascade
	)
END


IF NOT EXISTS(SELECT 1 FROM ampRole WHERE Id='administrators')
BEGIN
	INSERT INTO ampRole(Id,Description) VALUES('administrators','The site administrator role')
END
IF NOT EXISTS(SELECT 1 FROM ampRole WHERE Id='everyone')
BEGIN
	INSERT INTO ampRole(Id,Description) VALUES('everyone','The anonymous access role')
END
IF NOT EXISTS(SELECT 1 FROM ampRole WHERE Id='users')
BEGIN
	INSERT INTO ampRole(Id,Description) VALUES('users','The authenticated user role')
END
IF NOT EXISTS(SELECT 1 FROM ampRole WHERE Id='editors')
BEGIN
	INSERT INTO ampRole(Id,Description) VALUES('editors','The role for editing and creating pages')
END
IF NOT EXISTS(SELECT 1 FROM ampRole WHERE Id='publishers')
BEGIN
	INSERT INTO ampRole(Id,Description) VALUES('publishers','The role for editing and creating and publishing pages')
END

