
/****** Object:  Table [dbo].[TB_VETTING_MEETING]    Script Date: 18/4/17 5:01:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VETTING_MEETING](
	[Vetting_Meeting_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Venue] [nvarchar](255) NOT NULL,
	[Vetting_Meeting_From] [datetime] NOT NULL,
	[Vetting_Meeting_To] [datetime] NOT NULL,
	[Presentation_From] [datetime] NOT NULL,
	[Presentation_To] [datetime] NOT NULL,
	[Vetting_Team_Leader] [nvarchar](255) NOT NULL,
	[No_of_Attendance] [int] NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NOT NULL,
	[Modified_Date] [datetime] NOT NULL,
	[Time_Interval] [nvarchar](50) NULL,
	[Meeting_status] [nvarchar](50) NULL,
 CONSTRAINT [PK_TB_VETTING_MEETING] PRIMARY KEY CLUSTERED 
(
	[Vetting_Meeting_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A table to store vetting meeting information.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_VETTING_MEETING'
GO


/****** Object:  Table [dbo].[TB_VETTING_MEMBER]    Script Date: 18/4/17 5:01:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VETTING_MEMBER](
	[Vetting_Meeting_ID] [uniqueidentifier] NOT NULL,
	[Vetting_Member_ID] [uniqueidentifier] NOT NULL,
	[isLeader] [bit] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_VETTING_MEMBER] ADD  CONSTRAINT [DF_TB_VETTING_MEMBER_isLeader]  DEFAULT ((0)) FOR [isLeader]
GO

ALTER TABLE [dbo].[TB_VETTING_MEMBER]  WITH CHECK ADD FOREIGN KEY([Vetting_Meeting_ID])
REFERENCES [dbo].[TB_VETTING_MEETING] ([Vetting_Meeting_ID])
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A table to store vetting meeting member.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TB_VETTING_MEMBER'
GO


/****** Object:  Table [dbo].[TB_VETTING_MEMBER_INFO]    Script Date: 18/4/17 5:02:14 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_VETTING_MEMBER_INFO](
	[Vetting_Member_ID] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Full_Name] [nvarchar](255) NULL,
	[Registered] [bit] NOT NULL,
	[Disabled] [bit] NOT NULL,
 CONSTRAINT [PK_TB_VETTING_MEMBER_INFO] PRIMARY KEY CLUSTERED 
(
	[Vetting_Member_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[TB_VETTING_MEMBER_INFO] ADD  CONSTRAINT [DF_TB_VETTING_MEMBER_INFO_Registered]  DEFAULT ((0)) FOR [Registered]
GO

ALTER TABLE [dbo].[TB_VETTING_MEMBER_INFO] ADD  CONSTRAINT [DF_TB_VETTING_MEMBER_INFO_Disabled]  DEFAULT ((0)) FOR [Disabled]
GO





