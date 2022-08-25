USE CyberportEMS
go

alter table TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
add 
Completed_Date datetime null,
Actual_Claim_Amount decimal(18,0) null,
Deliver_Cheque_By nvarchar(255) null,
Deliver_Cheque_Date_Finance datetime null,
Deliver_Cheque_Date_Coordinator datetime null