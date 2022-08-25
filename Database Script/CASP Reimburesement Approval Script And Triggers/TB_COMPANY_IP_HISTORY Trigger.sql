USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_COMPANY_IP_HISTORY_TRIGGER]    Script Date: 9/21/2018 6:14:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



Create TRIGGER [dbo].[TB_COMPANY_IP_HISTORY_TRIGGER]
on [dbo].[TB_COMPANY_IP]
after UPDATE, INSERT
as

insert into TB_COMPANY_IP_HISTORY
select top 1 * from TB_COMPANY_IP where IP_ID =( select IP_ID from inserted) order by Modified_Date desc



GO


