USE [CyberportEMS]
GO

/****** Object:  StoredProcedure [dbo].[GetSearchReimburesement]    Script Date: 4/5/2020 11:45:06 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--use CyberportEMS
--go

ALTER proc [dbo].[GetSearchReimburesement]

 @srchAppNumber nvarchar(255) =''
,@srchStatus nvarchar(255) =''
,@searchTxt nvarchar(255) =''
,@srchProgType nvarchar(255) =''
,@srchCategory nvarchar(255) =''
,@srchDateFrom datetime =null
,@srchDateTo datetime = null
as begin

 Declare @sql nvarchar(max)     
 Declare @sql_FA nvarchar(max)   
 Declare @sql_SR nvarchar(max)  

Set @sql = '
select app.FA_ID,app.ApplicationNo,app.ApplicationType,app.Category,app.Status,app.SubmissionDate,com.Company_Name as CompanyName from (
select CASP_FA_ID as FA_ID,''CASP'' as ApplicationType, Application_No as ApplicationNo
,Company_ID , Category, Submitted_Date as SubmissionDate,Status
  from TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT
  
  union all 

select CASP_Special_Request_ID as FA_ID,''CASPSR'' as ApplicationType, Application_No as ApplicationNo
,Company_ID ,'''' as Category, Submitted_Date as SubmissionDate,Status
  from TB_CASP_SPECIAL_REQUEST
  ) as app
left join TB_COMPANY_PROFILE_BASIC com on com.Company_Profile_ID =app.Company_ID 
where (1=1) and Status != ''Saved'' and Status != ''Deleted'' 
 ' 
  
Set @sql_FA = '
select CASP_FA_ID, r.Application_No as ApplicationNo,''CASP''as ApplicationType, Category,r.Status, r.Submitted_Date as SubmissionDate,com.Company_Name as CompanyName
  from TB_CASP_FINANCIAL_ASSISTANCE_REIMBURSEMENT r
  left join TB_COMPANY_PROFILE_BASIC com on com.Company_Profile_ID =  Company_ID 
  where (1=1) and Status != ''Saved'' and Status != ''Deleted'' 
'
  
Set @sql_SR = '
select CASP_Special_Request_ID as FA_ID,''CASPSR'' as ApplicationType, Application_No as ApplicationNo
,Company_ID ,'''' as Category, Submitted_Date as SubmissionDate,Status,com.Company_Name as CompanyName
  from TB_CASP_SPECIAL_REQUEST s
 left join TB_COMPANY_PROFILE_BASIC com on com.Company_Profile_ID = s.Company_ID 
where (1=1) and Status != ''Saved'' and Status != ''Deleted'' 
'
if (@srchProgType = 'FA')
	Set @sql = @sql_FA 
else if (@srchProgType = 'SR')
	Set @sql = @sql_SR


if(len(@srchAppNumber)>0)
     Set @sql = @sql + ' and app.ApplicationNo='''+@srchAppNumber+''''

if(len(@srchStatus)>0)
     Set @sql = @sql + ' and app.Status='''+@srchStatus+''''
		  
if(len(@searchTxt)>0)
     Set @sql = @sql + ' and com.Company_Name like ''%'+@searchTxt+'%'''

--if(len(@srchProgType)>0)
--     Set @sql = @sql + ' and app.ApplicationType='''+@srchProgType+''''

if(len(@srchCategory)>0)
     Set @sql = @sql + ' and app.Category='''+@srchCategory+''''


if(@srchDateFrom !=null )
     Set @sql = @sql + ' and app.SubmissionDate >= '''+@srchDateFrom+''''

if(@srchDateTo !=null)
     Set @sql = @sql + ' and app.SubmissionDate <= '''+@srchDateTo +''''

	 set @sql = @sql +' order by SubmissionDate desc'

	 print(@sql)
 Execute sp_executesql @sql
 end
