alter table TB_FA_REIMBURSEMENT_SALARY drop constraint FK_TB_FA_REIMBURSEMENT_SALARY_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
go

alter table TB_FA_REIMBURSEMENT_ITEM drop constraint FK_TB_FA_REIMBURSEMENT_ITEM_TB_CPIP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
go

alter table [dbo].[TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT]

add 
Estimated_Service_From	datetime null,
Estimated_Service_To	datetime	null,
Prepaid_Service	bit	not null default(0),
Freelancer	bit	not null default(0),
Service_Provider_Name	nvarchar(255)	null,
Service_Contract	nvarchar(255)	null

go

alter table TB_CASP_APPLICATION  alter column
 [Company_Project] nvarchar(255)
 go


 alter table tb_company_Profile_basic add CCMF_Abstract_Chi nvarchar(max) null

 go