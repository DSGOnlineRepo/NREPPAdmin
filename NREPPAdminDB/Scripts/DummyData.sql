INSERT INTO AspNetUsers(Id, Fitstname, LastName, UserName) values (newid(), 'Nrepp', 'Test','nrepptest1')
INSERT INTO AspNetUsers(Id, Fitstname, LastName, UserName) values (newid(), 'Nrepp2', 'Test2','nrepptest2')
INSERT INTO AspNetUsers(Id, Fitstname, LastName, UserName) values (newid(), 'PRM1', 'Person','prm1')
INSERT INTO AspNetUsers(Id, Fitstname, LastName, UserName) values (newid(), 'reviewer', 'One','rev1')

DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')

DECLARE @USERID1 NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = 'nrepptest1'),
		@USERID2 NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = 'nrepptest2'),
		@USERID3 NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = 'prm1'),
		@USERID4 NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = 'rev1')

INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@USERID1, @ROLEID1)
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@USERID2, @ROLEID5)
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@USERID3, @ROLEID6)
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@USERID4, @ROLEID8)

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 1', 'Some Description', @USERID1, GetDate(), null, 1);

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 2', 'Some Other Description', @USERID2, GetDate(), null, 3);


-- Dummy Document Data --> Doesn't Map To Anything

INSERT INTO Document (Description, FileName, MIME, InterventionId, UploadedBy, TypeOfDocument, ReviewerId, IsLitSearch, ReviewerName)
	VALUES ('Some Document', '[NOT RELEVANT]', 'NOT IMPLEMENTED', 1, @USERID1, 4, @USERID1, 0, 'A File')

INSERT INTO Document (Description, FileName, MIME, InterventionId, UploadedBy, TypeOfDocument, ReviewerId, IsLitSearch, ReviewerName)
	VALUES ('Some Document 2', '[NOT RELEVANT]', 'NOT IMPLEMENTED', 1, @USERID1, 4, @USERID1, 0, 'A File')

-- Dummy Study Data

INSERT INTO Studies (StudyId, Reference, FromLitSearch, Exclusion1, Exclusion2, Exclusion3, StudyDesign, GroupSize, BaselineEquiv, UseMultivariate,
	LongestFollowup, SAMHSARelated, TargetPop, AuthorQueryNeeded, RecommendReview, Notes, DocumentId) VALUES
	(1, 'Empty', 1, 1, 1, 1, 1, '0123456789', 'Some Baseline Text', 1, '0123456789', 1, '0123456789', 0, 1, 'Notes go here', 1);

-- Dummy Outcomes

INSERT INTO Outcome (OutcomeName, InterventionId) VALUES ('Mental Health', 1)
INSERT INTO Outcome (OutcomeName, InterventionId) VALUES ('Alcohol Use', 1)

