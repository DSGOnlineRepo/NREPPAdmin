GO

CREATE TABLE QoRAnswerType (
	Id INT NOT NULL PRIMARY KEY,
	TypeName VARCHAR(300) NULL,
	Comparison VARCHAR(MAX) NULL
)

GO

CREATE TABLE QoRAnswer (
	OutcomeId INT NULL,
	StudyId INT NULL,
	AnswerTypeId INT NOT NULL,
	ReviewerId VARCHAR(256) NOT NULL,
	FixedAnswer VARCHAR(20) NULL,
	CalcAnswer VARCHAR(20) NULL
)

GO

USE [NREPPAdmin]
GO
/****** Object:  StoredProcedure [dbo].[SPGetQoRFinalAnswers]    Script Date: 7/28/2015 9:25:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SPGetQoRFinalAnswers]
@InvId INT
AS SET NOCOUNT ON

DECLARE @OutTable table(OutcomeId INT, OutcomeName VARCHAR(30), StudyId INT, AnswerTypeId INT, ReviewerId VARCHAR(256),
						FixedAnswer VARCHAR(30), CalcAnswer VARCHAR(30), TaxOutcomeId INT, TaxOutcomeName VARCHAR(350))

INSERT INTO @OutTable (OutcomeId, OutcomeName, StudyId, AnswerTypeId, ReviewerId, FixedAnswer, CalcAnswer)
	SELECT  OutcomeId, OutcomeName, s.StudyId, AnswerTypeId, q.ReviewerId, FixedAnswer, CalcAnswer FROM QoRAnswer q
INNER JOIN Studies s on q.StudyId = s.Id
INNER JOIN Outcome o on o.Id = q.OutcomeId
INNER JOIN Document d on s.DocumentId = d.Id
where o.InterventionId = @InvId OR d.InterventionId = @InvId

UPDATE @OutTable
SET o.TaxOutcomeId = ot.Id,
o.TaxOutcomeName = ot.OutcomeName,
o.OutcomeName = om.OutcomeMeasure
FROM @OutTable o
INNER JOIN OutcomeMeasure om on o.OutcomeId = om.OutcomeId
INNER JOIN OutcomeTaxonomy ot ON om.TaxonomyOutcome = ot.Id


select OutcomeId, OutcomeName, StudyId, AnswerTypeId, ReviewerId,
						FixedAnswer, CalcAnswer, TaxOutcomeId, TaxOutcomeName from @OutTable

GO