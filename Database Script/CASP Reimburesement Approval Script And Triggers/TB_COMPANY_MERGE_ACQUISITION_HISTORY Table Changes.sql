USE [CyberportEMS]
GO

drop table TB_COMPANY_MERGE_ACQUISITION_HISTORY
go
/****** Object:  Table [dbo].[TB_COMPANY_MERGE_ACQUISITION]    Script Date: 9/21/2018 6:17:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_COMPANY_MERGE_ACQUISITION_HISTORY(
	[Merge_Acquistion_ID] [int]  NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Merge_Acquistion] [nvarchar](50) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount] [decimal](15, 2) NULL,
	[Valuation] [decimal](15, 2) NULL,
	[Date] [datetime] NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL)

