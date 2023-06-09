USE [CyberportEMS]
GO
/****** Object:  Table [dbo].[TB_BANK_INFO_FOR_DIRECT_TRANSFER]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_BANK_INFO_FOR_DIRECT_TRANSFER](
	[Bank_Info_ID] [uniqueidentifier] NOT NULL,
	[CPIP_FA_ID] [uniqueidentifier] NOT NULL,
	[Bank_Name] [nvarchar](255) NULL,
	[Bank_Code] [nvarchar](50) NULL,
	[Bank_Account_No] [nvarchar](50) NULL,
	[Bank_SWIFT_Code] [nvarchar](50) NULL,
	[Account_Holder] [nvarchar](255) NULL,
	[Agree] [bit] NULL,
	[Remark] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Bank_Info_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_BANK_INFO_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_BANK_INFO_HISTORY](
	[Bank_Info_ID] [uniqueidentifier] NOT NULL,
	[CPIP_FA_ID] [uniqueidentifier] NOT NULL,
	[Bank_Name] [nvarchar](255) NULL,
	[Bank_Code] [nvarchar](50) NULL,
	[Bank_Account_No] [nvarchar](50) NULL,
	[Bank_SWIFT_Code] [nvarchar](50) NULL,
	[Account_Holder] [nvarchar](255) NULL,
	[Agree] [bit] NULL,
	[Remark] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_APPLICATION]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_APPLICATION](
	[CASP_ID] [uniqueidentifier] NOT NULL,
	[Programme_ID] [int] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Applicant] [nvarchar](255) NOT NULL,
	[Company_Project] [uniqueidentifier] NOT NULL,
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
 CONSTRAINT [PK__TB_CASP___D615FE56DA5E1855] PRIMARY KEY CLUSTERED 
(
	[CASP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TB_CASP_APPLICATION] UNIQUE NONCLUSTERED 
(
	[Application_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_APPLICATION_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_APPLICATION_HISTORY](
	[CASP_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Company_Proejct] [uniqueidentifier] NOT NULL,
	[CCMF_CPIP_App_No] [nvarchar](50) NULL,
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
	[Declaration] [bit] NOT NULL,
	[Principle_Full_Name] [nvarchar](255) NULL,
	[Principle_Title] [nvarchar](255) NULL,
	[Submitted_Date] [datetime] NULL,
	[Created_By] [nvarchar](50) NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Status] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_FA_REIMBURSEMENT_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_FA_REIMBURSEMENT_HISTORY](
	[CASP_FA_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NULL,
	[Category] [nvarchar](1) NOT NULL,
	[Company_ID] [uniqueidentifier] NULL,
	[CASP_Attended] [uniqueidentifier] NULL,
	[Payable_To] [nvarchar](50) NULL,
	[Preapproved_SpecialRequest] [uniqueidentifier] NULL,
	[Conflict_of_Interest] [bit] NULL,
	[Conflict_detail] [nvarchar](max) NULL,
	[Total_Amount] [decimal](15, 2) NULL,
	[Declared_A] [bit] NULL,
	[Declared_D] [bit] NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT](
	[CASP_FA_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NULL,
	[Category] [nvarchar](1) NOT NULL,
	[Company_ID] [uniqueidentifier] NULL,
	[CASP_Attended] [uniqueidentifier] NULL,
	[Payable_To] [nvarchar](50) NULL,
	[Preapproved_SpecialRequest] [uniqueidentifier] NULL,
	[Conflict_of_Interest] [bit] NULL,
	[Conflict_detail] [nvarchar](max) NULL,
	[Total_Amount] [decimal](15, 2) NULL,
	[Declared_A] [bit] NULL,
	[Declared_D] [bit] NULL,
	[Status] [nvarchar](50) NULL,
	[Created_By] [nvarchar](50) NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CASP_FA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] UNIQUE NONCLUSTERED 
(
	[Application_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_SPECIAL_REQUEST]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_SPECIAL_REQUEST](
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
PRIMARY KEY CLUSTERED 
(
	[CASP_Special_Request_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TB_CASP_SPECIAL_REQUEST] UNIQUE NONCLUSTERED 
(
	[Application_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CASP_SPECIAL_REQUEST_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CASP_SPECIAL_REQUEST_HISTORY](
	[CASP_Special_Request_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NOT NULL,
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
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_ADMIN]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_ADMIN](
	[Administrator_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Full_Name] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Administrator_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_APPLICATION_MAP]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_APPLICATION_MAP](
	[Map_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Application_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Applicaition_Type] [nvarchar](50) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
 CONSTRAINT [PK__TB_COMPA__BD7A620905145443] PRIMARY KEY CLUSTERED 
(
	[Map_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_AWARD]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_AWARD](
	[Award_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Awarded_Year_Month] [datetime] NULL,
	[Type] [nvarchar](20) NULL,
	[Name] [nvarchar](255) NULL,
	[Nature] [nvarchar](20) NULL,
	[Product_Name] [nvarchar](255) NULL,
	[Award_Name] [nvarchar](255) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Award_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_AWARD_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_AWARD_HISTORY](
	[Award_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Awarded_Year_Month] [datetime] NULL,
	[Type] [nvarchar](20) NULL,
	[Name] [nvarchar](255) NULL,
	[Nature] [nvarchar](20) NULL,
	[Product_Name] [nvarchar](255) NULL,
	[Award_Name] [nvarchar](255) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_CONTACT]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_CONTACT](
	[Contact_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NOT NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Position] [nvarchar](255) NULL,
	[Contact_No] [nvarchar](20) NULL,
	[Fax_No] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[Mailing_Address] [nvarchar](max) NULL,
	[Additional_Info] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Contact_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_CONTACT_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_CONTACT_HISTORY](
	[Contact_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NOT NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Position] [nvarchar](255) NULL,
	[Contact_No] [nvarchar](20) NULL,
	[Fax_No] [nvarchar](20) NULL,
	[Email] [nvarchar](255) NULL,
	[Mailing_Address] [nvarchar](max) NULL,
	[Additional_Info] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_FUND]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_FUND](
	[Funding_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Programme_Name] [nvarchar](255) NULL,
	[Programme_Type] [nvarchar](20) NULL,
	[Application_No] [nvarchar](50) NULL,
	[Funding_Status] [nvarchar](255) NULL,
	[Expenditure_Nature] [nvarchar](255) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount_Received] [decimal](15, 2) NULL,
	[Maximum_Amount] [decimal](15, 2) NULL,
	[Application_Status] [nvarchar](50) NULL,
	[Funding_Origin] [nvarchar](255) NULL,
	[Invertor_Info] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Funding_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_FUND_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_FUND_HISTORY](
	[Funding_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Programme_Name] [nvarchar](255) NULL,
	[Programme_Type] [nvarchar](20) NULL,
	[Application_No] [nvarchar](50) NULL,
	[Funding_Status] [nvarchar](255) NULL,
	[Expenditure_Nature] [nvarchar](255) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount_Received] [decimal](15, 2) NULL,
	[Maximum_Amount] [decimal](15, 2) NULL,
	[Application_Status] [nvarchar](50) NULL,
	[Funding_Origin] [nvarchar](255) NULL,
	[Invertor_Info] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_IP]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_IP](
	[IP_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](255) NULL,
	[Registration_Date] [datetime] NULL,
	[Reported_Date] [datetime] NULL,
	[Reference_No] [nvarchar](50) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[IP_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_IP_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_IP_HISTORY](
	[IP_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Category] [nvarchar](255) NULL,
	[Registration_Date] [datetime] NULL,
	[Reported_Date] [datetime] NULL,
	[Reference_No] [nvarchar](50) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_MEMBER]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_MEMBER](
	[Core_Member_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Position] [nvarchar](255) NULL,
	[CCMF] [bit] NULL,
	[CPIP] [bit] NULL,
	[CASP] [bit] NULL,
	[HKID] [nvarchar](255) NULL,
	[Masked_HKID] [nvarchar](255) NULL,
	[Background_Information] [nvarchar](max) NULL,
	[Bootcamp_Eligible_Number] [nvarchar](255) NULL,
	[Professional_Qualifications] [nvarchar](255) NULL,
	[Working_Experiences] [nvarchar](255) NULL,
	[Special_Achievements] [nvarchar](255) NULL,
	[CoreMember_Profile] [nvarchar](255) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Core_Member_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_MEMBER_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_MEMBER_HISTORY](
	[Core_Member_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Position] [nvarchar](255) NULL,
	[CCMF] [bit] NULL,
	[CPIP] [bit] NULL,
	[CASP] [bit] NULL,
	[HKID] [nvarchar](255) NULL,
	[Masked_HKID] [nvarchar](255) NULL,
	[Background_Information] [nvarchar](max) NULL,
	[Bootcamp_Eligible_Number] [nvarchar](255) NULL,
	[Professional_Qualifications] [nvarchar](255) NULL,
	[Working_Experiences] [nvarchar](255) NULL,
	[Special_Achievements] [nvarchar](255) NULL,
	[CoreMember_Profile] [nvarchar](255) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Core_Member_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_MERGE_ACQUISITION]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_MERGE_ACQUISITION](
	[Merge_Acquistion_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Merge_Acquistion] [nvarchar](50) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount] [decimal](15, 2) NULL,
	[Valuation] [decimal](15, 2) NULL,
	[Date] [datetime] NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Merge_Acquistion_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_MERGE_ACQUISITION_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_MERGE_ACQUISITION_HISTORY](
	[Merge_Acquistion_ID] [int] NOT NULL,
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Merge_Acquistion] [nvarchar](50) NULL,
	[Currency] [nvarchar](10) NULL,
	[Amount] [decimal](15, 2) NULL,
	[Valuation] [decimal](15, 2) NULL,
	[Date] [datetime] NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_PROFILE_BASIC]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_PROFILE_BASIC](
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Brand_Name] [nvarchar](255) NULL,
	[CCMF_Custer] [nvarchar](50) NULL,
	[CPIP_Custer] [nvarchar](50) NULL,
	[Tag] [nvarchar](255) NULL,
	[CCMF_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract_Chi] [nvarchar](max) NULL,
	[CASP_Abstract] [nvarchar](max) NULL,
	[Company_Ownership_Structure] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Company_Profile_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_COMPANY_PROFILE_BASIC_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_COMPANY_PROFILE_BASIC_HISTORY](
	[Company_Profile_ID] [uniqueidentifier] NOT NULL,
	[Name_Eng] [nvarchar](255) NULL,
	[Name_Chi] [nvarchar](255) NULL,
	[Company_Name] [nvarchar](255) NULL,
	[Brand_Name] [nvarchar](255) NULL,
	[CCMF_Custer] [nvarchar](50) NULL,
	[CPIP_Custer] [nvarchar](50) NULL,
	[Tag] [nvarchar](255) NULL,
	[CCMF_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract] [nvarchar](max) NULL,
	[CPIP_Abstract_Chi] [nvarchar](max) NULL,
	[CASP_Abstract] [nvarchar](max) NULL,
	[Company_Ownership_Structure] [nvarchar](max) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CPIP_FA_REIMBURSEMENT_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CPIP_FA_REIMBURSEMENT_HISTORY](
	[CPIP_FA_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NULL,
	[Category] [nvarchar](1) NOT NULL,
	[CPIP_ID] [uniqueidentifier] NOT NULL,
	[Company_ID] [uniqueidentifier] NULL,
	[Payable_To] [nvarchar](255) NULL,
	[Event_Attended] [nvarchar](255) NULL,
	[Location_ID] [nvarchar](50) NULL,
	[Service_Not_Completed] [bit] NULL,
	[Service_From] [datetime] NULL,
	[Service_To] [datetime] NULL,
	[Freelancer] [bit] NULL,
	[Service_Provider_Name] [nvarchar](255) NULL,
	[Service_Contract] [nvarchar](255) NULL,
	[Total_Fee] [decimal](15, 2) NULL,
	[Conflict_of_Interest] [bit] NULL,
	[Conflict_detail] [nvarchar](max) NULL,
	[Professional_Service_Category] [nvarchar](50) NULL,
	[Other_Service_Category] [nvarchar](255) NULL,
	[Total_Amount] [decimal](15, 2) NULL,
	[Special_Request_ID] [uniqueidentifier] NULL,
	[Declared] [bit] NULL,
	[Declared_B] [bit] NULL,
	[Declared_E] [bit] NULL,
	[WebSite] [nvarchar](255) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT](
	[CPIP_FA_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NULL,
	[Category] [nvarchar](1) NOT NULL,
	[CPIP_ID] [uniqueidentifier] NOT NULL,
	[Company_ID] [uniqueidentifier] NULL,
	[Payable_To] [nvarchar](255) NULL,
	[Event_Attended] [nvarchar](255) NULL,
	[Location_ID] [nvarchar](50) NULL,
	[Service_Not_Completed] [bit] NULL,
	[Service_From] [datetime] NULL,
	[Service_To] [datetime] NOT NULL,
	[Freelancer] [bit] NULL,
	[Service_Provider_Name] [nvarchar](255) NULL,
	[Service_Contract] [nvarchar](255) NULL,
	[Total_Fee] [decimal](15, 2) NULL,
	[Conflict_of_Interest] [bit] NULL,
	[Conflict_detail] [nvarchar](max) NULL,
	[Professional_Service_Category] [nvarchar](50) NULL,
	[Other_Service_Category] [nvarchar](255) NULL,
	[Total_Amount] [decimal](15, 2) NULL,
	[Special_Request_ID] [uniqueidentifier] NULL,
	[Declared] [bit] NULL,
	[Declared_B] [bit] NULL,
	[Declared_E] [bit] NULL,
	[WebSite] [nvarchar](255) NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
 CONSTRAINT [PK__TB_CPIP___84C5A403D29C199F] PRIMARY KEY CLUSTERED 
(
	[CPIP_FA_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] UNIQUE NONCLUSTERED 
(
	[Application_No] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CPIP_SPECIAL_REQUEST]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CPIP_SPECIAL_REQUEST](
	[CPIP_Special_Request_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NULL,
	[CPIP_id] [uniqueidentifier] NOT NULL,
	[Company_ID] [uniqueidentifier] NOT NULL,
	[Contact_Name] [nvarchar](255) NULL,
	[Phone_No] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Event_Name] [nvarchar](255) NULL,
	[Event_Date_Start] [datetime] NULL,
	[Event_Date_End] [datetime] NULL,
	[Country_City] [nvarchar](50) NULL,
	[Fee] [decimal](15, 2) NULL,
	[Purpose_1] [nvarchar](max) NULL,
	[Service_Provider_Name] [nvarchar](50) NULL,
	[Estimate_Amount] [decimal](9, 2) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Justification] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[CPIP_Special_Request_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_CPIP_SPECIAL_REQUEST_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_CPIP_SPECIAL_REQUEST_HISTORY](
	[CPIP_Special_Request_ID] [uniqueidentifier] NOT NULL,
	[Application_No] [nvarchar](50) NOT NULL,
	[Submitted_Date] [datetime] NOT NULL,
	[CPIP_id] [uniqueidentifier] NULL,
	[Company_ID] [uniqueidentifier] NOT NULL,
	[Contact_Name] [nvarchar](255) NULL,
	[Phone_No] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Event_Name] [nvarchar](255) NULL,
	[Event_Date_Start] [datetime] NULL,
	[Event_Date_End] [datetime] NULL,
	[Country_City] [nvarchar](50) NULL,
	[Fee] [decimal](15, 2) NULL,
	[Purpose_1] [nvarchar](max) NULL,
	[Service_Provider_Name] [nvarchar](50) NULL,
	[Estimate_Amount] [decimal](9, 2) NULL,
	[Purpose] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Justification] [nvarchar](max) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_ITEM]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_FA_REIMBURSEMENT_ITEM](
	[Item_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Advertisement] [bit] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Item_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_ITEM_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_FA_REIMBURSEMENT_ITEM_HISTORY](
	[Item_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NULL,
	[Description] [nvarchar](max) NULL,
	[Advertisement] [bit] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_SALARY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_FA_REIMBURSEMENT_SALARY](
	[Salary_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Intern_Name] [nvarchar](255) NULL,
	[Monthly_Salary] [decimal](15, 2) NULL,
	[MPF] [decimal](15, 2) NULL,
	[Tax] [decimal](15, 2) NULL,
	[Period_From] [datetime] NULL,
	[Period_To] [datetime] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Salary_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TB_FA_REIMBURSEMENT_SALARY_HISTORY]    Script Date: 5/24/2018 1:01:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TB_FA_REIMBURSEMENT_SALARY_HISTORY](
	[Salary_ID] [uniqueidentifier] NOT NULL,
	[FA_Application_ID] [uniqueidentifier] NOT NULL,
	[Intern_Name] [nvarchar](255) NULL,
	[Monthly_Salary] [decimal](15, 2) NULL,
	[MPF] [decimal](15, 2) NULL,
	[Tax] [decimal](15, 2) NULL,
	[Period_From] [datetime] NULL,
	[Period_To] [datetime] NULL,
	[Amount] [decimal](15, 2) NULL,
	[Created_By] [nvarchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TB_BANK_INFO_FOR_DIRECT_TRANSFER] ADD  CONSTRAINT [DF_TB_BANK_INFO_FOR_DIRECT_TRANSFER_Agree]  DEFAULT ((0)) FOR [Agree]
GO
ALTER TABLE [dbo].[TB_CASP_APPLICATION] ADD  CONSTRAINT [DF_TB_CASP_APPLICATION_Endorsed_by_Cyberport]  DEFAULT ((0)) FOR [Endorsed_by_Cyberport]
GO
ALTER TABLE [dbo].[TB_CASP_APPLICATION] ADD  CONSTRAINT [DF_TB_CASP_APPLICATION_Declaration]  DEFAULT ((0)) FOR [Declaration]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Conflict_of_Interest]  DEFAULT ((0)) FOR [Conflict_of_Interest]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Declared_A]  DEFAULT ((0)) FOR [Declared_A]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Declared_D]  DEFAULT ((0)) FOR [Declared_D]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Freelancer]  DEFAULT ((0)) FOR [Freelancer]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Conflict_of_Interest]  DEFAULT ((0)) FOR [Conflict_of_Interest]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Declared]  DEFAULT ((0)) FOR [Declared]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Declared_B]  DEFAULT ((0)) FOR [Declared_B]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ADD  CONSTRAINT [DF_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_Declared_E]  DEFAULT ((0)) FOR [Declared_E]
GO
ALTER TABLE [dbo].[TB_FA_REIMBURSEMENT_ITEM] ADD  CONSTRAINT [DF_TB_FA_REIMBURSEMENT_ITEM_Advertisement]  DEFAULT ((0)) FOR [Advertisement]
GO
ALTER TABLE [dbo].[TB_BANK_INFO_FOR_DIRECT_TRANSFER]  WITH CHECK ADD  CONSTRAINT [FK_TB_BANK_INFO_FOR_DIRECT_TRANSFER_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] FOREIGN KEY([CPIP_FA_ID])
REFERENCES [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ([CPIP_FA_ID])
GO
ALTER TABLE [dbo].[TB_BANK_INFO_FOR_DIRECT_TRANSFER] CHECK CONSTRAINT [FK_TB_BANK_INFO_FOR_DIRECT_TRANSFER_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CASP_APPLICATION] FOREIGN KEY([CASP_Attended])
REFERENCES [dbo].[TB_CASP_APPLICATION] ([CASP_ID])
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CASP_APPLICATION]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CASP_SPECIAL_REQUEST] FOREIGN KEY([Preapproved_SpecialRequest])
REFERENCES [dbo].[TB_CASP_SPECIAL_REQUEST] ([CASP_Special_Request_ID])
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CASP_SPECIAL_REQUEST]
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_CASP_SPECIAL_REQUEST]  WITH CHECK ADD  CONSTRAINT [FK_TB_CASP_SPECIAL_REQUEST_TB_CASP_APPLICATION] FOREIGN KEY([CASP_ID])
REFERENCES [dbo].[TB_CASP_APPLICATION] ([CASP_ID])
GO
ALTER TABLE [dbo].[TB_CASP_SPECIAL_REQUEST] CHECK CONSTRAINT [FK_TB_CASP_SPECIAL_REQUEST_TB_CASP_APPLICATION]
GO
ALTER TABLE [dbo].[TB_CASP_SPECIAL_REQUEST]  WITH CHECK ADD  CONSTRAINT [FK_TB_CASP_SPECIAL_REQUEST_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_CASP_SPECIAL_REQUEST] CHECK CONSTRAINT [FK_TB_CASP_SPECIAL_REQUEST_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_ADMIN]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_ADMIN_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_ADMIN] CHECK CONSTRAINT [FK_TB_COMPANY_ADMIN_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_APPLICATION_MAP]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_APPLICATION_MAP_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_APPLICATION_MAP] CHECK CONSTRAINT [FK_TB_COMPANY_APPLICATION_MAP_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_AWARD]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_AWARD_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_AWARD] CHECK CONSTRAINT [FK_TB_COMPANY_AWARD_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_CONTACT]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_CONTACT_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_CONTACT] CHECK CONSTRAINT [FK_TB_COMPANY_CONTACT_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_FUND]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_FUND_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_FUND] CHECK CONSTRAINT [FK_TB_COMPANY_FUND_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_IP]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_IP_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_IP] CHECK CONSTRAINT [FK_TB_COMPANY_IP_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_MEMBER]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_MEMBER_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_MEMBER] CHECK CONSTRAINT [FK_TB_COMPANY_MEMBER_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_COMPANY_MERGE_ACQUISITION]  WITH CHECK ADD  CONSTRAINT [FK_TB_COMPANY_MERGE_ACQUISITION_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_Profile_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_COMPANY_MERGE_ACQUISITION] CHECK CONSTRAINT [FK_TB_COMPANY_MERGE_ACQUISITION_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_COMPANY_PROFILE_BASIC] FOREIGN KEY([Company_ID])
REFERENCES [dbo].[TB_COMPANY_PROFILE_BASIC] ([Company_Profile_ID])
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_COMPANY_PROFILE_BASIC]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CPIP_SPECIAL_REQUEST] FOREIGN KEY([Special_Request_ID])
REFERENCES [dbo].[TB_CPIP_SPECIAL_REQUEST] ([CPIP_Special_Request_ID])
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_CPIP_SPECIAL_REQUEST]
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]  WITH CHECK ADD  CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_INCUBATION_APPLICATION] FOREIGN KEY([CPIP_ID])
REFERENCES [dbo].[TB_INCUBATION_APPLICATION] ([Incubation_ID ])
GO
ALTER TABLE [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] CHECK CONSTRAINT [FK_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT_TB_INCUBATION_APPLICATION]
GO
ALTER TABLE [dbo].[TB_FA_REIMBURSEMENT_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_TB_FA_REIMBURSEMENT_ITEM_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] FOREIGN KEY([FA_Application_ID])
REFERENCES [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ([CPIP_FA_ID])
GO
ALTER TABLE [dbo].[TB_FA_REIMBURSEMENT_ITEM] CHECK CONSTRAINT [FK_TB_FA_REIMBURSEMENT_ITEM_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]
GO
ALTER TABLE [dbo].[TB_FA_REIMBURSEMENT_SALARY]  WITH CHECK ADD  CONSTRAINT [FK_TB_FA_REIMBURSEMENT_SALARY_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] FOREIGN KEY([FA_Application_ID])
REFERENCES [dbo].[TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT] ([CPIP_FA_ID])
GO
ALTER TABLE [dbo].[TB_FA_REIMBURSEMENT_SALARY] CHECK CONSTRAINT [FK_TB_FA_REIMBURSEMENT_SALARY_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]
GO
