
create trigger TB_APPLICATION_CONTACT_DETAIL_HISTORY_trigger
on TB_APPLICATION_CONTACT_DETAIL
after UPDATE, INSERT
as

insert into TB_APPLICATION_CONTACT_DETAIL_HISTORY
select top 1 * from TB_APPLICATION_CONTACT_DETAIL where CONTACT_DETAILS_ID =( select CONTACT_DETAILS_ID from inserted) 

