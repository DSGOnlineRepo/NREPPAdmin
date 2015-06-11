
DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')

DELETE FROM RoutingTable where CurrentStatus = 2

INSERT INTO RoutingTable (CurrentStatus, DestStatus, DestUserRole, DestDescription) VALUES (2, 92, @ROLEID3, 'Failed to meet minimum requirements')
INSERT INTO RoutingTable (CurrentStatus, DestStatus, DestUserRole, DestDescription) VALUES (2, 4, @ROLEID5, 'Failed to meet minimum requirements')