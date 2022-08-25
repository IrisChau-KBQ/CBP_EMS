create trigger TB_APPLICATION_FUNDING_STATUS_HISTORY_trigger
on TB_APPLICATION_FUNDING_STATUS
after UPDATE, INSERT
as

insert into TB_APPLICATION_FUNDING_STATUS_HISTORY
select top 1 * from TB_APPLICATION_FUNDING_STATUS where Funding_ID =( select Funding_ID from inserted) 

