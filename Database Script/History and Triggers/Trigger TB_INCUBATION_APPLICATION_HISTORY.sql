create trigger TB_INCUBATION_APPLICATION_HISTORY_trigger
on TB_INCUBATION_APPLICATION
after UPDATE, INSERT
as

insert into TB_INCUBATION_APPLICATION_HISTORY 
select top 1 * from TB_INCUBATION_APPLICATION where Incubation_ID  =( select Incubation_ID from inserted) order by Modified_Date desc

