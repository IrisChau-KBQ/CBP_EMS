USE [CyberportEMS]
GO

drop table TB_COMPANY_MEMBER_HISTORY
go
/****** Object:  Table [dbo].[TB_COMPANY_MEMBER]    Script Date: 9/21/2018 6:15:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_COMPANY_MEMBER_HISTORY](
	[Core_Member_ID] [int]  NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Position] [nvarchar](255) NULL,
	[CCMF] [bit] NULL,
	[CPIP] [bit] NULL,
	[CASP] [bit] NULL,
	[HKID] [nvarchar](255) NULL,
	[Masked_HKID] [nvarchar](255) NULL,
	[Background_Information] [nvarchar](max) NULL,
	[Bootcamp_Eligible_Number] [nvarchar](255) NULL,
	[Professional_Qualifications] [nvarchar](255) NULL,
	[Working_Experiences] [nvarchar](255) NULL,
	[Special_Achievements] [nvarchar](255) NULL,
	[CoreMember_Profile] [nvarchar](255) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[No_Edit] [bit] NULL)