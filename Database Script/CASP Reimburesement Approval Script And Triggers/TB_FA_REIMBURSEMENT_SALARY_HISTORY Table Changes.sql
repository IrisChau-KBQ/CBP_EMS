USE [CyberportEMS]
GO

drop table TB_FA_REIMBURSEMENT_SALARY_HISTORY
go
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_SALARY]    Script Date: 9/21/2018 6:23:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_FA_REIMBURSEMENT_SALARY_HISTORY(
	[Salary_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Intern_Name] [nvarchar](255) NULL,
	[Monthly_Salary] [decimal](15, 2) NULL,
	[MPF] [decimal](15, 2) NULL,
	[Tax] [decimal](15, 2) NULL,
	[Period_From] [datetime] NULL,
	[Period_To] [datetime] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL)

