use CyberportEMS
go

drop table [dbo].[TB_CASP_APPLICATION_HISTORY]
go

CREATE TABLE [dbo].[TB_CASP_APPLICATION_HISTORY](
	[CASP_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Applicant] [nvarchar](255) NOT NULL,
	[Company_Project] [nvarchar](255) NULL,
	[CCMF_CPIP_App_No] [nvarchar](50) NOT NULL,
	[Company_Address] [nvarchar](max) NULL,
	[Abstract] [nvarchar](max) NULL,
	[Company_Ownership_Structure] [nvarchar](max) NULL,
	[Additional_Info] [nvarchar](max) NULL,
	[Accelerator_Name] [nvarchar](255) NULL,
	[Endorsed_by_Cyberport] [bit] NULL,
	[Commencement_Date] [datetime] NULL,
	[Duration] [int] NULL,
	[Background] [nvarchar](max) NULL,
	[Offer] [nvarchar](max) NULL,
	[Fund_Raising_Capabilities] [nvarchar](max) NULL,
	[Size_of_Alumni] [nvarchar](max) NULL,
	[Reputation] [nvarchar](max) NULL,
	[Website] [nvarchar](255) NULL,
	[Declaration] [bit] NULL,
	[Principle_Full_Name] [nvarchar](255) NULL,
	[Principle_Title] [nvarchar](255) NULL,
	[Submitted_Date] [datetime] NULL,
	[Created_By] [nvarchar](50) NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Application_Parent_ID] [nvarchar](36) NULL,
	[Version_Number] [nvarchar](20) NOT NULL,
	[Last_Submitted] [datetime] NULL,
	[Completed_Date] [datetime] NULL
)