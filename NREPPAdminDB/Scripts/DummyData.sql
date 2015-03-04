DECLARE @pass VARCHAR(100)
DECLARE @salt VARCHAR(100)

SET @pass = 'Gj51geLeI8EJfCd/7qdldQ6F8Oat0mq0YEXiZJpHqHo='
SET @salt = 'TOjQdGWJCsEyeefw78zSzo+ouCp7/WmQ'

-- This file enters non-production data into the database. FOR DEVELOPMENT USE ONLY

INSERT INTO Users (Username, Firstname, Lastname, hash, salt, RoleID) VALUES ('nrepptest1', 'Nrepp', 'Test', @pass, @salt, 1)
INSERT INTO Users (Username, Firstname, Lastname, hash, salt, RoleID) VALUES ('nrepptest2', 'Nrepp2', 'Test2', @pass, @salt, 5)

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 1', 'Some Description', 1, GetDate(), null, 1);

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 2', 'Some Other Description', 2, GetDate(), null, 1);


-- Dummy Document Data --> Doesn't Map To Anything

INSERT INTO Document (Description, FileName, MIME, InterventionId, UploadedBy, TypeOfDocument, ReviewerId, IsLitSearch, ReviewerName)
	VALUES ('Some Document', '[NOT RELEVANT]', 'NOT IMPLEMENTED', 1, 1, 4, 1, 0, 'A File')

INSERT INTO Document (Description, FileName, MIME, InterventionId, UploadedBy, TypeOfDocument, ReviewerId, IsLitSearch, ReviewerName)
	VALUES ('Some Document 2', '[NOT RELEVANT]', 'NOT IMPLEMENTED', 1, 1, 4, 1, 0, 'A File')

-- Dummy Study Data

INSERT INTO Studies (StudyId, Reference, FromLitSearch, Exclusion1, Exclusion2, Exclusion3, StudyDesign, GroupSize, BaselineEquiv, UseMultivariate,
	LongestFollowup, SAMHSARelated, TargetPop, AuthorQueryNeeded, RecommendReview, Notes, DocumentId) VALUES
	(1, 'Empty', 1, 1, 1, 1, 1, '0123456789', 'Some Baseline Text', 1, '0123456789', 1, '0123456789', 0, 1, 'Notes go here', 1);

-- Dummy Outcomes

INSERT INTO Outcome (OutcomeName, InterventionId) VALUES ('Mental Health', 1)
INSERT INTO Outcome (OutcomeName, InterventionId) VALUES ('Alcohol Use', 1)

