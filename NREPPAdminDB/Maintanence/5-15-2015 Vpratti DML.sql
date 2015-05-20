INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Pre-Screener');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Mathematica Review Coordinator');
INSERT INTO AspNetRoles (Id, Name) VALUES (NEWID(), 'Mathematica PRM');
UPDATE AspNetRoles SET NAME = 'PRM' WHERE NAME = 'DSG PRM'