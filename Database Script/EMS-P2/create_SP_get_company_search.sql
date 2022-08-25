USE [CyberportEMS]
GO

/****** Object:  StoredProcedure [dbo].[get_company_search]    Script Date: 15/8/2018 11:26:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[get_company_search]
@ProfileName nvarchar(255),
@coreMemberName nvarchar(255),
@programmeType nvarchar(255),
@intake_No nvarchar(255),
@cluster nvarchar(255),
@tag nvarchar(255)
as
begin
    Declare @sql nvarchar(max)     
     Set @sql = 'select distinct cpb.Company_Profile_ID as[Company_Profile_ID]
	 --,  cpb.Name_Eng as[Name_Eng],cpb.Name_Chi as [Name_Chi],cpb.CCMF_Custer as[CCMF_Custer],cpb.CPIP_Custer as [CPIP_Custer],cpb.Brand_Name as [Brand_Name],cpb.Company_Name as [Company_Name], cmap.Applicaition_Type as[Programme_Type], cpb.Tag as [tag]
from  TB_COMPANY_PROFILE_BASIC as cpb left join TB_COMPANY_APPLICATION_MAP as cmap on cpb.Company_Profile_ID = cmap.Company_Profile_ID 
left join TB_CCMF_APPLICATION ccmf on cmap.Application_No=ccmf.Application_Number
left join TB_INCUBATION_APPLICATION as cpip on cmap.Application_No=cpip.Application_Number
left join TB_COMPANY_MEMBER cpm on cpm.Company_Profile_ID=cpb.Company_Profile_ID 

where 1 = 1 '
if(LEN(@tag) > 0)
          Set @sql = @sql + ' and cpb.Tag='''+@tag+''''
if(LEN(@programmeType) > 0)
		  Set @sql = @sql + ' and cmap.Applicaition_Type = '''+@programmeType +''''
if(LEN(@ProfileName) > 0)
         Set @sql = @sql + ' and cpb.Company_Name like ''%'+@ProfileName +'%'' or cpb.Brand_Name like ''%'+@ProfileName+'%'''
if (LEN(@cluster) > 0)
		 set @sql=@sql +' and cpb.CCMF_Custer='''+@cluster+'''  or cpb.CPIP_Custer='''+@cluster+''''

if (LEN(@coreMemberName) > 0)
		set @sql=@sql+ ' and cpm.Name like ''%'+@coreMemberName+'%'''

if(LEN(@intake_No)>0)
begin
declare @intk int
set @intk=TRY_PARSE(@intake_No AS INT);
if(@intk is not null)
		set @sql=@sql+'  and cpip.Intake_Number='''+@intake_No+''' or ccmf.Intake_Number='''+@intake_No+''''  
 end

 print(@sql)
 Execute sp_executesql @sql
end


GO


