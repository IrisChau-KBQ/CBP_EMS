USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_COMPANY_MEMBER_HISTORY_HISTORY_TRIGGER]    Script Date: 9/21/2018 6:16:37 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create TRIGGER [dbo].[TB_COMPANY_MEMBER_HISTORY_TRIGGER]
on [dbo].[TB_COMPANY_MEMBER]
after UPDATE, INSERT
as

insert into TB_COMPANY_MEMBER_HISTORY
select top 1 * from TB_COMPANY_MEMBER where Core_Member_ID =( select Core_Member_ID from inserted) order by Modified_Date desc



GO


