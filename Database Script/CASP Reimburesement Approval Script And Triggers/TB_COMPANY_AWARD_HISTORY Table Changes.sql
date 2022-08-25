USE [CyberportEMS]
GO

drop table TB_COMPANY_AWARD_HISTORY
go 
/****** Object:  Table [dbo].[TB_COMPANY_AWARD]    Script Date: 9/21/2018 5:49:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_COMPANY_AWARD_HISTORY](
	[Award_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Awarded_Year_Month] [datetime] NULL,
	[Type] [nvarchar](20) NULL,
	[Name] [nvarchar](255) NULL,
	[Nature] [nvarchar](20) NULL,
	[Product_Name] [nvarchar](255) NULL,
	[Award_Name] [nvarchar](255) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
)