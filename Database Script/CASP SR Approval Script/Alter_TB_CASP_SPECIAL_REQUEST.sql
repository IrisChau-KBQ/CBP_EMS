alter table TB_CASP_SPECIAL_REQUEST
add 
Actual_Claim_Amount decimal(18,0) null,
Deliver_Cheque_By nvarchar(255) null,
Deliver_Cheque_Date_Finance datetime null
go

alter table TB_CASP_SPECIAL_REQUEST_History
add 
Actual_Claim_Amount decimal(18,0) null,
Deliver_Cheque_By nvarchar(255) null,
Deliver_Cheque_Date_Finance datetime null