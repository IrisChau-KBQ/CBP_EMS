/****** Script for SelectTopNRows command from SSMS  ******/
  update [TB_EMAIL_TEMPLATE]
  set [Email_Template_Content] = 
  '<p>Dear @@FullName,</p><br/><br/>
<p>You are invited to register at the Cyberport Entrepreneurship Management System as the Vetting Member, please access the link below to set up paswword for your account.</p><br/>
<p><a href="@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink">@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink</a></p><br/><br/>
<p>Regards,</p><br/>
<p>Cyberport EMS Administrator"</p><br/><br/>
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p>'
where [Email_Template] = 'Vetting_Team_Invitaion'