use CyberportEMS
go

/****** Object:  Trigger [dbo].[TB_CCMF_APPLICATION_HISTORY_trigger]    Script Date: 9/21/2018 5:32:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create TRIGGER [dbo].[TB_CASP_APPLICATION_HISTORY_TRIGGER]
on [dbo].[TB_CASP_APPLICATION]
after UPDATE, INSERT
as

insert into TB_CASP_APPLICATION_HISTORY
select top 1 * from TB_CASP_APPLICATION where CASP_ID =( select CASP_ID from inserted) order by Modified_Date desc
