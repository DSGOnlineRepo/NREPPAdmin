CREATE TABLE [dbo].[SubmissionPds]
(
	[StartDate] SMALLDATETIME NOT NULL, 
    [EndDate] SMALLDATETIME NOT NULL,
	[Active] bit not null default 1,
	[CreatedBy] nvarchar(128) not null,
	[CreatedOn] datetime not null,
	[ModifiedBy] nvarchar(128) not null,
	[ModifiedOn] datetime not null
)
