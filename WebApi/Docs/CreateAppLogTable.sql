CREATE TABLE AppLog (
	[ID] uniqueidentifier PRIMARY KEY not null DEFAULT newsequentialid(),
	[MachineName] nvarchar(300) null,
	[LoggedOn] datetime2(7) not null,
	[Level] nvarchar(50) not null,
	[Message] nvarchar(max) not null,
	[Template] nvarchar(500) null,
	[Logger] nvarchar(300) null,
	[Callsite] nvarchar(max) null,
	[Exception] nvarchar(max) null,
	[Properties] nvarchar(max) null
)