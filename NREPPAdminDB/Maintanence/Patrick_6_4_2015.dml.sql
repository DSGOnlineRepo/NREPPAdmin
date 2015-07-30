SET IDENTITY_INSERT InterventionStatus ON

INSERT INTO InterventionStatus (Id, StatusName) VALUES (6, 'Pending SAMHSA Approval')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (7, 'Reviewer Assignment')
INSERT INTO InterventionStatus (Id, StatusName) VALUES (96, 'Rejected by SAMHSA')

SET IDENTITY_INSERT InterventionStatus OFF

GO


DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Reviewer'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')


INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus, DestDescription) VALUES (5, @ROLEID5, 6, 'Forward to SAMHSA For Approval')
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus, DestDescription) VALUES (6, @ROLEID5, 96, 'Mark Rejected by SAMHSA')
INSERT INTO RoutingTable (CurrentStatus, DestUserRole, DestStatus, DestDescription) VALUES (6, @ROLEID5, 7, 'Approved by SAMHSA')