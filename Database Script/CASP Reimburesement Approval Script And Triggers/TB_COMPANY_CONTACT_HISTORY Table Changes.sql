USE [CyberportEMS]
GO

drop table TB_COMPANY_CONTACT_HISTORY
go
/****** Object:  Table [dbo].[TB_COMPANY_CONTACT]    Script Date: 9/21/2018 5:58:04 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_COMPANY_CONTACT_HISTORY(
	[Contact_ID] [int]  NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Position] [nvarchar](255) NULL,
	[Contact_No] [nvarchar](20) NULL,
	[Fax_No] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[Mailing_Address] [nvarchar](max) NULL,
	[Additional_Info] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Salutation] [nvarchar](50) NULL,
	[HKID] [nvarchar](50) NULL,
	[Graduation_Date] [datetime] NULL,
	[Education_Institution] [nvarchar](255) NULL,
	[Student_ID] [nvarchar](50) NULL,
	[Programme_Enrolled] [nvarchar](255) NULL,
	[Organization_Name] [nvarchar](255) NULL,
	[Contact_No_Home] [nvarchar](20) NULL,
	[Contact_No_Office] [nvarchar](20) NULL,
	[Area] [nvarchar](255) NULL,
	[Masked_HKID] [nvarchar](255) NULL,
	[No_Edit] [bit] NULL)


