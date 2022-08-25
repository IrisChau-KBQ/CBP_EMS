CREATE TABLE [dbo].[TB_APPLICATION_FUNDING_STATUS_HISTORY](
	[Funding_ID] [int] NOT NULL,
	[Application_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Programme_Name] [nvarchar](255) NULL,
	[Application_Status] [nvarchar](255) NULL,
	[Funding_Status] [nvarchar](255) NULL,
	[Expenditure_Nature] [nvarchar](255) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount_Received] [decimal](15, 2) NULL,
	[Maximum_Amount] [decimal](15, 2) NULL
	)