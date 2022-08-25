USE [CyberportEMS_QA]
GO
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'CollaborationExistsInvitation', N'<p>Hi,</p>
<br />
<p>You are invited by @@ApplicantEmail to collaborate the @@ProgramName application in the Cyberport Entrepreneurship Management System, please access to the application form by the link below. 
<br/> 
<a href="@@ApplicationUrl" target="_blank">@@ApplicationUrl</a></p>
<br />
<p>Regards,</p>

<p>EMS Administrator</p>
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p>')
GO
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'CollaborationNewInvitation', N'<p>Hi,</p>
<br />
<p>You are invited by @@ApplicantEmail to access Cyberport Entrepreneurship Management System, please access the link below to set up password for your designated email account.
<br/> 
<a href="@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink">@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink</a>
<br/>
Once account setup is done, you may start editing the application via the link below.
<br/> <a href="@@ApplicationUrl" target="_blank">@@ApplicationUrl</a>
<br />
<p>Regards,</p>
<br />
<p>EMS Administrator</p>
<br />
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p>
')
GO
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'ForgotPassword', N'<p>Hi,</p>
<br />
<p>Please access the following page to reset password in the Cyberport Entrepreneurship Management System (EMS). <a href="@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink">@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink</a></p>
<br />
<p>Regards,</p>
<br />
<p>EMS Administrator</p>
<br />
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p>')
GO
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Registration', N'<p>Hi,</p>
<br />
<p>Thank you for registering Entrepreneurship Management System, you may now login the system at <a href=''@@WebsiteUrl'' target=''_blank''>@@WebsiteUrl</a> to apply Cyberport Entrepreneurship programmes.</p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>
<br />
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p>')
GO
