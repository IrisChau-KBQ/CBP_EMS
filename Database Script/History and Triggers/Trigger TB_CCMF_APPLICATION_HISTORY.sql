create trigger TB_CCMF_APPLICATION_HISTORY_trigger
on TB_CCMF_APPLICATION
after UPDATE, INSERT
as

insert into TB_CCMF_APPLICATION_HISTORY
select top 1 * from TB_CCMF_APPLICATION where CCMF_ID =( select CCMF_ID from inserted) order by Modified_Date desc

