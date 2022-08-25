alter table TB_INCUBATION_APPLICATION 
alter column Other_Company_Type nvarchar (255)

alter table TB_INCUBATION_APPLICATION 
alter column Management_Positioning nvarchar (255)

alter table TB_INCUBATION_APPLICATION 
alter column Other_Positioning nvarchar (255)




drop table [dbo].[TB_RESET_PASSWORD]

CREATE TABLE [dbo].[TB_RESET_PASSWORD](
	[Reset_Password_ID] [uniqueidentifier] NOT NULL default newid(),
	[Email] [nvarchar](50) NOT NULL,
	[Email_Type] [nvarchar](30) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
)




Insert into TB_SYSTEM_PARAMETER (Config_Code,Value) values('Reset_Password_Link_Expire_In_Hours','24')

update TB_EMAIL_TEMPLATE set Email_Template_Content = '<p>Hi,</p><br /><p>Please access the following page to reset password .<a href="@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink">@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink</a></p><br /><p>Regards,</p><p>EMS Administrator</p>' where Email_Template = 'ForgotPassword'