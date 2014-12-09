-- This file enters non-production data into the database. FOR DEVELOPMENT USE ONLY

INSERT INTO Users (Username, Firstname, Lastname, hash, salt, RoleID) VALUES ('nrepptest1', 'Nrepp', 'Test', '12p3821p312', '23p[18210', 1)
INSERT INTO Users (Username, Firstname, Lastname, hash, salt, RoleID) VALUES ('nrepptest2', 'Nrepp2', 'Test2', '12p3821p312', '23p[18210', 1)

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 1', 'Some Description', 1, GetDate(), null, 1);

INSERT INTO Interventions (Title, FullDescription, Submitter, UpdateDate, PublishDate, Status) VALUES
	('Dummy Intervention 2', 'Some Other Description', 2, GetDate(), null, 1);