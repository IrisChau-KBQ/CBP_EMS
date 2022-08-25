use CyberportEMS
go

/****** Object:  StoredProcedure [dbo].[GetSearchReimburesement]    Script Date: 10/16/2018 6:10:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--use CyberportEMS
--go

Create proc [dbo].[GetSearchReimburesement]

 @srchAppNumber nvarchar(255) =''
,@srchStatus nvarchar(255) =''
,@searchTxt nvarchar(255) =''
,@srchProgType nvarchar(255) =''
,@srchCategory nvarchar(255) =''
,@srchDateFrom datetime =null
,@srchDateTo datetime = null
as begin

 Declare @sql nvarchar(max)     
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
if(len(@srchAppNumber)>0)
     Set @sql = @sql + ' and app.ApplicationNo='''+@srchAppNumber+''''

if(len(@srchStatus)>0)
     Set @sql = @sql + ' and app.Status='''+@srchStatus+''''

		  
if(len(@searchTxt)>0)
     Set @sql = @sql + ' and com.Company_Name like ''%'+@searchTxt+'%'''

if(len(@srchProgType)>0)
     Set @sql = @sql + ' and app.ApplicationType='''+@srchProgType+''''

if(len(@srchCategory)>0)
     Set @sql = @sql + ' and app.Category='''+@srchCategory+''''


if(@srchDateFrom !=null )
     Set @sql = @sql + ' and app.SubmissionDate >= '''+@srchDateFrom+''''

if(@srchDateTo !=null)
     Set @sql = @sql + ' and app.SubmissionDate <= '''+@srchDateTo +''''

	 set @sql = @sql +' order by SubmissionDate desc'

	 --print(@sql)
 Execute sp_executesql @sql
 end