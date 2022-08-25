USE [CyberportEMS]
GO

drop table TB_COMPANY_FUND_HISTORY
go 
/****** Object:  Table [dbo].[TB_COMPANY_FUND]    Script Date: 9/21/2018 6:00:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_COMPANY_FUND_HISTORY(
	[Funding_ID] [int]  NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Programme_Name] [nvarchar](255) NULL,
	[Programme_Type] [nvarchar](20) NULL,
	[Application_No] [nvarchar](50) NULL,
	[Funding_Status] [nvarchar](255) NULL,
	[Expenditure_Nature] [nvarchar](255) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount_Received] [decimal](15, 2) NULL,
	[Maximum_Amount] [decimal](15, 2) NULL,
	[Application_Status] [nvarchar](50) NULL,
	[Funding_Origin] [nvarchar](255) NULL,
	[Invertor_Info] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Reported_Date] [datetime] NULL,
	[Received_Date] [datetime] NULL,
	[No_Edit] [bit] NULL)

