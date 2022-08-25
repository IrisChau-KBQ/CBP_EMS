
create trigger TB_APPLICATION_ATTACHMENT_HISTORY_trigger
on TB_APPLICATION_ATTACHMENT
after UPDATE, INSERT
as

insert into TB_APPLICATION_ATTACHMENT_HISTORY
select top 1 * from TB_APPLICATION_ATTACHMENT where Attachment_ID =( select Attachment_ID from inserted) 

