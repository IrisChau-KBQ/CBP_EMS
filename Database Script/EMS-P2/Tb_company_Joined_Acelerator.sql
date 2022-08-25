
/****** Object:  Table [dbo].[TB_Company_Joined_Accelerator]    Script Date: 8/9/2018 10:46:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_Company_Joined_Accelerator](
	[Joined_Accelerator_ID] [int] IDENTITY(1,1) NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Participation_Year_Month] [datetime] NULL,
	[Accelerator_Programme] [nvarchar](255) NULL,
	[Remarks] [nvarchar](255) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
 CONSTRAINT [PK_TB_Company_Joined_Accelerator] PRIMARY KEY CLUSTERED 
(
	[Joined_Accelerator_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_Company_Joined_Accelerator]  WITH CHECK ADD  CONSTRAINT [FK_TB_Company_Joined_Accelerator_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO

ALTER TABLE [dbo].[TB_Company_Joined_Accelerator] CHECK CONSTRAINT [FK_TB_Company_Joined_Accelerator_TB_COMPANY_PROFILE_BASIC]
GO


