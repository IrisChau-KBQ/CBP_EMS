USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_FA_REIMBURSEMENT_ITEM_HISTORY_TRIGGER]    Script Date: 9/21/2018 6:22:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create TRIGGER [dbo].[TB_FA_REIMBURSEMENT_ITEM_HISTORY_TRIGGER]
on [dbo].[TB_FA_REIMBURSEMENT_ITEM]
after UPDATE, INSERT
as

insert into TB_FA_REIMBURSEMENT_ITEM_HISTORY
select top 1 * from TB_FA_REIMBURSEMENT_ITEM where Item_ID =( select Item_ID from inserted) order by Modified_Date desc



GO


