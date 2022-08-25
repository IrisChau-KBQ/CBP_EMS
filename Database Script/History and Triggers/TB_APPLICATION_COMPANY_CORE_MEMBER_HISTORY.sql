
CREATE TABLE [dbo].[TB_APPLICATION_COMPANY_CORE_MEMBER_HISTORY](
	[Core_Member_ID] [int] NOT NULL,
	[Application_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
	[Position] [nvarchar](255) NULL,
	[HKID] [nvarchar](255) NULL,
	[Background_Information] [ntext] NULL,
	[Bootcamp_Eligible_Number] [nvarchar](255) NULL,
	[Professional_Qualifications] [nvarchar](255) NULL,
	[Working_Experiences] [nvarchar](255) NULL,
	[Special_Achievements] [nvarchar](255) NULL,
	[CoreMember_Profile] [ntext] NULL,
	[Masked_HKID] [nvarchar](255) NULL
)
