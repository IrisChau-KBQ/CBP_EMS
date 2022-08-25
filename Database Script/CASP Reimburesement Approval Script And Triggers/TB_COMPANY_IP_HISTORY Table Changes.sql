USE [CyberportEMS]
GO

drop table TB_COMPANY_IP_HISTORY
go
/****** Object:  Table [dbo].[TB_COMPANY_IP]    Script Date: 9/21/2018 6:13:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_COMPANY_IP_HISTORY(
	[IP_ID] [int]  NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](255) NULL,
	[Registration_Date] [datetime] NULL,
	[Reported_Date] [datetime] NULL,
	[Reference_No] [nvarchar](50) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Title] [nvarchar](255) NULL)