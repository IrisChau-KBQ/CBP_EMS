USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_COMPANY_AWARD_HISTORY_TRIGGER]    Script Date: 9/21/2018 5:51:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create TRIGGER [dbo].[TB_COMPANY_AWARD_HISTORY_TRIGGER]
on [dbo].[TB_COMPANY_AWARD]
after UPDATE, INSERT
as

insert into TB_COMPANY_AWARD_HISTORY
select top 1 * from TB_COMPANY_AWARD where Award_ID =( select Award_ID from inserted) order by Modified_Date desc

GO


