
create trigger TB_APPLICATION_COMPANY_CORE_MEMBER_HISTORY_trigger
on TB_APPLICATION_COMPANY_CORE_MEMBER
after UPDATE, INSERT
as

insert into TB_APPLICATION_COMPANY_CORE_MEMBER_HISTORY
select top 1 * from TB_APPLICATION_COMPANY_CORE_MEMBER where Core_Member_ID =( select Core_Member_ID from inserted) 

