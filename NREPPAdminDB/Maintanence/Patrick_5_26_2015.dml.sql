SET Identity_Insert MaskList ON
INSERT INTO MaskList (Id, MaskPower, MaskValueName, MaskCategory) VALUES (9, 1, '1. Is there an evaluation study that assess mental health or substance abuse outcomes?', 'RCPrescreen')
INSERT INTO MaskList (Id, MaskPower, MaskValueName, MaskCategory) VALUES (10, 2, '2. Is there an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?', 'RCPrescreen')
INSERT INTO MaskList (Id, MaskPower, MaskValueName, MaskCategory) VALUES (11, 3, '3. Has the effectiveness of the intervention been assessed with at least one experiment or quasi-experimental design, with a comparison group? (Studies with single-group, pretest-posttest designs do not meet this requirement.)', 'RCPrescreen')
INSERT INTO MaskList (Id, MaskPower, MaskValueName, MaskCategory) VALUES (12, 4, '4. Is there an evaluation study that assesses other behavioral health-related outcomes on populations with mental health issues or substance abuse problems?', 'RCPrescreen')
SET Identity_Insert MaskList OFF

GO

update RoutingTable
SET DestDescription = 'Does not pass pre-screening'
WHERE CurrentStatus = 2 AND DestStatus = 92

INSERT INTO RoutingTable (CurrentStatus, DestStatus, DestUserRole, DestDescription) VALUES (2, 3, '2EDB67EB-3FA1-45DA-8770-C51C23FC5A25', 'Meets Minimum Requirements')

GO