USE [CyberportEMS]
GO

drop table TB_COMPANY_PROFILE_BASIC_HISTORY
go

/****** Object:  Table [dbo].[TB_COMPANY_PROFILE_BASIC]    Script Date: 9/21/2018 6:19:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_COMPANY_PROFILE_BASIC_HISTORY(
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Brand_Name] [nvarchar](255) NULL,
	[CCMF_Custer] [nvarchar](50) NULL,
	[CPIP_Custer] [nvarchar](50) NULL,
	[Tag] [nvarchar](255) NULL,
	[CCMF_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract_Chi] [nvarchar](max) NULL,
	[CASP_Abstract] [nvarchar](max) NULL,
	[Company_Ownership_Structure] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[CCMF_Abstract_Chi] [nvarchar](max) NULL)


