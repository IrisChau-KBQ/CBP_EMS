USE [CyberportEMS]
GO

/****** Object:  Trigger [dbo].[TB_COMPANY_CONTACT_HISTORY_TRIGGER]    Script Date: 9/21/2018 5:59:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create TRIGGER [dbo].[TB_COMPANY_CONTACT_HISTORY_TRIGGER]
on [dbo].[TB_COMPANY_CONTACT]
after UPDATE, INSERT
as

insert into TB_COMPANY_CONTACT_HISTORY
select top 1 * from TB_COMPANY_CONTACT where Contact_ID =( select Contact_ID from inserted) order by Modified_Date desc

GO


