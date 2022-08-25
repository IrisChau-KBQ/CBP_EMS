USE [CyberportEMS]
GO

drop table TB_FA_REIMBURSEMENT_ITEM_HISTORY
go
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_ITEM]    Script Date: 9/21/2018 6:21:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].TB_FA_REIMBURSEMENT_ITEM_HISTORY(
	[Item_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Advertisement] [bit] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL)


