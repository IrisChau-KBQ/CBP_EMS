 CREATE PROCEDURE usp_GetClusters(@Programme AS varchar(200)
   ,@Status varchar(200)
	, @ApplicationStartDate Datetime
	 ,@ApplicationEndDate Datetime
	 ,@IntakeStartNumber varchar(6)
	 ,@IntakeEndNumber varchar(6)
 )
 AS
 SET NOCOUNT ON;
IF OBJECT_ID('tempdb.dbo.#CCMF_data', 'U') IS NOT NULL
  DROP TABLE #CCMF_data; 

  IF OBJECT_ID('tempdb.dbo.#INCUB_data', 'U') IS NOT NULL
  DROP TABLE #INCUB_data; 

  IF OBJECT_ID('tempdb.dbo.#CASP_data', 'U') IS NOT NULL
  DROP TABLE #CASP_data; 

   
 --   set @Status ='All'
	--set @ApplicationStartDate=NULL--'2017-07-04'
	--set @ApplicationEndDate=NULL--'2017-07-06'
	--set @IntakeStartNumber = '201704'
	--set @IntakeEndNumber = '201806'
	--Declare @Programme nvarchar(200)
   -- set @Programme ='Cyberport Creative Micro Fund - Hong Kong,Cyberport Incubation Programme'
	----,Cyberport Creative Micro Fund - Hong Kong,Cyberport Accelerator Support programme'
	--SELECT RTRIM(',', @Programme)

SELECT a.Submission_Date
      ,b.Shortlisted
     , a.[Intake_Number]
      ,Count(a.[Intake_Number]) as ccmf
      ,Case when [Business_Area] ='Open Data' Then 'AI / Big Data' else Business_Area End as Business_Area
	  ,'CCMF' as ProgramType
	  ,p.Programme_Name
     into #CCMF_data  
  FROM [CyberportEMS].[dbo].[TB_CCMF_APPLICATION] a
   inner join TB_APPLICATION_SHORTLISTING b on a.Application_Number = b.Application_Number
   left join TB_PROGRAMME_INTAKE p on b.Programme_ID = p.Programme_ID
  where a.Status not in ('Saved','Deleted')
  group by Business_Area,a.[Intake_Number],a.Application_Number,b.Shortlisted,a.Submission_Date,p.Programme_Name


  select a.Submission_Date,b.Shortlisted, a.[Intake_Number]
      ,Count(a.[Intake_Number]) as Incu
       ,Case when [Business_Area] ='Open Data' Then 'AI / Big Data' else Business_Area End as Business_Area
	  ,'Incubation' as ProgramType
	  ,p.Programme_Name
	  into #INCUB_data
	  from TB_INCUBATION_APPLICATION a
	  inner join TB_APPLICATION_SHORTLISTING b on a.Application_Number = b.Application_Number
	  left join TB_PROGRAMME_INTAKE p on b.Programme_ID = p.Programme_ID
  where a.Status not in ('Saved','Deleted')
   group by Business_Area,a.[Intake_Number],a.Application_Number,b.Shortlisted,a.Submission_Date,p.Programme_Name

   SELECT a.Submitted_Date
      ,b.Shortlisted
     , p.[Intake_Number]
      ,Count(p.[Intake_Number]) as ccmf
      ,'' as Business_Area
	  ,'CASP' as ProgramType
	  ,p.Programme_Name
     into #CASP_data  
  FROM [CyberportEMS].[dbo].[TB_CASP_APPLICATION] a
   inner join TB_APPLICATION_SHORTLISTING b on a.Application_No = b.Application_Number
   left join TB_PROGRAMME_INTAKE p on b.Programme_ID = p.Programme_ID
  where a.Status not in ('Saved','Deleted')
  group by p.[Intake_Number],a.Application_No,b.Shortlisted,a.Submitted_Date,p.Programme_Name
  
   select * from (
   select Business_Area,Intake_Number,count(Business_Area)as applicationCount,ProgramType,Programme_Name from #CCMF_data 
    WHERE NULLIF(Business_Area, '') IS NOT NULL  and (CASE WHEN @Status = 'All' THEN Shortlisted END = Shortlisted OR
	CASE WHEN @Status = '1' THEN Shortlisted END = 1)
	and ((Submission_Date >= @ApplicationStartDate OR @ApplicationStartDate IS NULL) AND (Submission_Date <= @ApplicationEndDate OR @ApplicationEndDate IS NULL))
	and ((Intake_Number >= @IntakeStartNumber OR @IntakeStartNumber IS NULL) AND (Intake_Number <= @IntakeEndNumber OR @IntakeEndNumber IS NULL))
	group by  Business_Area,Intake_Number,ProgramType,Programme_Name
   union all
   select Business_Area,Intake_Number,count(Business_Area)as applicationCount,ProgramType,Programme_Name from #INCUB_data 
   WHERE NULLIF(Business_Area, '') IS NOT NULL and (CASE WHEN @Status = 'All' THEN Shortlisted END = Shortlisted OR
	CASE WHEN @Status = '1' THEN Shortlisted END = 1) 
	and ((Submission_Date >= @ApplicationStartDate OR @ApplicationStartDate IS NULL) AND (Submission_Date <= @ApplicationEndDate OR @ApplicationEndDate IS NULL))
	and ((Intake_Number >= @IntakeStartNumber OR @IntakeStartNumber IS NULL) AND (Intake_Number <= @IntakeEndNumber OR @IntakeEndNumber IS NULL))
	group by  Business_Area,Intake_Number,ProgramType,Programme_Name

	union all

	 select Business_Area,Intake_Number,count(Business_Area)as applicationCount,ProgramType,Programme_Name from #CASP_data 
   WHERE NULLIF(Business_Area, '') IS NOT NULL and (CASE WHEN @Status = 'All' THEN Shortlisted END = Shortlisted OR
	CASE WHEN @Status = '1' THEN Shortlisted END = 1) 
	and ((Submitted_Date >= @ApplicationStartDate OR @ApplicationStartDate IS NULL) AND (Submitted_Date <= @ApplicationEndDate OR @ApplicationEndDate IS NULL))
	and ((Intake_Number >= @IntakeStartNumber OR @IntakeStartNumber IS NULL) AND (Intake_Number <= @IntakeEndNumber OR @IntakeEndNumber IS NULL))
	group by  Business_Area,Intake_Number,ProgramType,Programme_Name

	) as t where 
	--CHARINDEX(@Programme,REVERSE(Programme_Name),1)
	--dbo.INSTR(@Programme,Programme_Name, 1, 1) >0
 CHARINDEX(Programme_Name,(@Programme ),1)>0 
	SET NOCOUNT OFF