USE [CyberportWMS1]
GO

EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_EC_RESULT'

GO

/****** Object:  Table [dbo].[TB_EC_RESULT]    Script Date: 5/10/2017 6:22:44 PM ******/
DROP TABLE [dbo].[TB_EC_RESULT]
GO

/****** Object:  Table [dbo].[TB_EC_RESULT]    Script Date: 5/10/2017 6:22:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_EC_RESULT](
	[EC_Result_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Application_Number] [nvarchar](50) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Cluster] [nchar](30) NULL,
	[Company_Program] [nchar](255) NULL,
	[Programme_Type] [nchar](30) NULL,
	[Application_Type] [nchar](30) NULL,
	[Recommended] [bit] NULL,
	[Total_votes] [int] NULL,
	[Remarks] [ntext] NULL,
	[Status] [nvarchar](50) NULL,
	[Recommendedcount] [int] NULL,
	[NotRecommendedcount] [int] NULL,
 CONSTRAINT [PK_TB_EC_RESULT] PRIMARY KEY CLUSTERED 
(
	[EC_Result_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A table to store EC result' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_EC_RESULT'
GO


