USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_FA_REIMBURSEMENT_SALARY_HISTORY_TRIGGER]    Script Date: 9/21/2018 6:24:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create TRIGGER [dbo].[TB_FA_REIMBURSEMENT_SALARY_HISTORY_TRIGGER]
on [dbo].[TB_FA_REIMBURSEMENT_SALARY]
after UPDATE, INSERT
as

insert into TB_FA_REIMBURSEMENT_SALARY_HISTORY
select top 1 * from TB_FA_REIMBURSEMENT_SALARY where Salary_ID =( select Salary_ID from inserted) order by Modified_Date desc



GO


