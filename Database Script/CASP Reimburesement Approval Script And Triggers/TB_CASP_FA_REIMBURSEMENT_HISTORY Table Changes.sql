use CyberportEMS
go

drop table TB_CASP_FA_REIMBURSEMENT_HISTORY
go

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
	[Status] [nvarchar](50) NULL,
	[Created_By] [nvarchar](50) NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [nvarchar](50) NULL,
	[Modified_Date] [datetime] NULL,
	[Estimated_Service_From] [datetime] NULL,
	[Estimated_Service_To] [datetime] NULL,
	[Prepaid_Service] [bit] NOT NULL,
	[Freelancer] [bit] NOT NULL,
	[Service_Provider_Name] [nvarchar](255) NULL,
	[Service_Contract] [nvarchar](255) NULL,
	[Total_Fee] [decimal](18, 0) NULL,
	[Completed_Date] [datetime] NULL,
	[Actual_Claim_Amount] [decimal](18, 0) NULL,
	[Deliver_Cheque_By] [nvarchar](255) NULL,
	[Deliver_Cheque_Date_Finance] [datetime] NULL,
	[Deliver_Cheque_Date_Coordinator] [datetime] NULL)

