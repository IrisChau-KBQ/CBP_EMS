
CREATE TABLE [dbo].[TB_APPLICATION_ATTACHMENT_HISTORY](
	[Attachment_ID] [int]  NOT NULL,
	[Application_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Attachment_Type] [nvarchar](255) NULL,
	[Attachment_Path] [nvarchar](255) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NOT NULL,
	[Modified_Date] [datetime] NOT NULL
)