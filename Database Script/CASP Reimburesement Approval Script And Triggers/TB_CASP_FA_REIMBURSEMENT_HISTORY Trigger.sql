use CyberportEMS
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create TRIGGER [dbo].[TB_CASP_FA_REIMBURSEMENT_HISTORY_TRIGGER]
on [dbo].TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
after UPDATE, INSERT
as

insert into TB_CASP_FA_REIMBURSEMENT_HISTORY
select top 1 * from TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT where CASP_FA_ID =( select CASP_FA_ID from inserted) order by Modified_Date desc
