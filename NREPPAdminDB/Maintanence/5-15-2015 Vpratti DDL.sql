ALTER TABLE InterventionStatus ADD NotifyUser bit default(0)
GO
ALTER TABLE InterventionStatus ADD NotifySubmitter bit default(0)
GO
ALTER TABLE InterventionStatus ADD NotifyRC bit default(0)
GO