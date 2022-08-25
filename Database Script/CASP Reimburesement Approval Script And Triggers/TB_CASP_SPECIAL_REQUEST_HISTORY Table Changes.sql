use CyberportEMS
go

drop table TB_CASP_SPECIAL_REQUEST_HISTORY
go


CREATE TABLE [dbo].[TB_CASP_SPECIAL_REQUEST_HISTORY](
	[CASP_Special_Request_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NULL,
	[Submitted_Date] [datetime] NULL,
	[CASP_ID] [uniqueidentifier] NOT NULL,
	[Company_ID] [uniqueidentifier] NULL,
	[Contact_Name] [nvarchar](255) NULL,
	[Phone_No] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[CASP_Attended] [uniqueidentifier] NULL,
	[Service_Provider_Name] [nvarchar](255) NULL,
	[Estimate_Amount] [decimal](15, 2) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Justification] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Status] [nvarchar](50) NULL)

