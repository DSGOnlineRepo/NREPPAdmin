﻿CREATE TABLE [dbo].[AspNetUsers](
	[Id] NVARCHAR(128) NOT NULL, 
   	[FirstName] NVARCHAR(250) NULL, 
    [LastName] NVARCHAR(250) NULL,     
    [HomeAddressLine1] VARCHAR(100) NULL, 
    [HomeAddressLine2] VARCHAR(100) NULL, 
	[HomeCity] VARCHAR(30) NULL,
    [HomeState] CHAR(2) NULL, 
    [HomeZip] VARCHAR(11) NULL, 
	[PhoneNumber] VARCHAR(15) NULL, 
    [FaxNumber] VARCHAR(15) NULL, 
    [Email] VARCHAR(100) NULL, 
    [Employer] VARCHAR(40) NULL, 
    [Department] VARCHAR(30) NULL, 
    [WorkAddressLine1] VARCHAR(100) NULL, 
	[WorkAddressLine2] VARCHAR(100) NULL, 
    [WorkCity] VARCHAR(30) NULL,
	[WorkState] CHAR(2) NULL,
    [WorkZip] VARCHAR(11) NULL, 
    [WorkPhoneNumber] VARCHAR(15) NULL, 
    [WorkFaxNumber] VARCHAR(15) NULL, 
    [WorkEmail] NCHAR(100) NULL, 
    [ExperienceSummary] VARCHAR(MAX) NULL, 	
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]