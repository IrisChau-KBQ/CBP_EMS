 CREATE PROCEDURE usp_GetIntakes(@Programme AS varchar(200))
 AS
 SET NOCOUNT ON;

select Intake_Number from TB_PROGRAMME_INTAKE 
where CHARINDEX(Programme_Name,@Programme,1)<>0  
--WHERE CHARINDEX(','+Programme_Name+',',','​+@Programme+',') > 0  
UNION SELECT NULL

SET NOCOUNT OFF;