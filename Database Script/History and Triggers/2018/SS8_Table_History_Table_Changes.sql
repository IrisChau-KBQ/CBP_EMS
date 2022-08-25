
alter table TB_INCUBATION_APPLICATION_History
add 
Project_Name	nvarchar(255) null,
Establishment_Year	datetime null,
Country_Of_Origin	nvarchar(255)	null,
NEW_to_HK	bit	null
go

alter table TB_CCMF_APPLICATION_History
add 
SmartSpace	nvarchar(50)	null,
Project_Name	nvarchar(255)	null,
Company_Name	nvarchar(255)	null,
Establishment_Year	datetime	null,
Country_Of_Origin	nvarchar(255)	null,
NEW_to_HK	bit	null,
Question1_3	bit	null

go

alter table TB_APPLICATION_COMPANY_CORE_MEMBER_history
add 
Email	nvarchar(255) null,
Nationality	nvarchar(255) null

go

alter table TB_APPLICATION_CONTACT_DETAIL_history
add Nationality nvarchar(255) null

go
