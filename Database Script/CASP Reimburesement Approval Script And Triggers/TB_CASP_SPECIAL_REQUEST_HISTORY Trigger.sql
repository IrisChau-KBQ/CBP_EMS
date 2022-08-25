USE [CyberportWMS1]
GO

/****** Object:  Trigger [dbo].[TB_CASP_SPECIAL_REQUEST_HISTORY_TRIGGER]    Script Date: 9/21/2018 5:53:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create TRIGGER [dbo].[TB_CASP_SPECIAL_REQUEST_HISTORY_TRIGGER]
on [dbo].[TB_CASP_SPECIAL_REQUEST]
after UPDATE, INSERT
as

insert into TB_CASP_SPECIAL_REQUEST_HISTORY
select top 1 * from TB_CASP_SPECIAL_REQUEST where CASP_Special_Request_ID =( select CASP_Special_Request_ID from inserted) order by Modified_Date desc

GO


