update TB_EMAIL_TEMPLATE set Email_Template_Content='<p>Hi,</p>
<br />
<p>You are invited by @@ApplicantEmail to collaborate the @@ProgramName application, please access to the application form by the link below. <br/> <a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>' where Email_Template='CollaborationExistsInvitation'


update TB_EMAIL_TEMPLATE set Email_Template_Content='<p>Hi,</p>
<br />
<p>You are invited by @@ApplicantEmail to Cyberport Entrepreneurship Management System, please access the link below to set up password for your account.<br/> 
<a href="@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink">@@WebsiteUrl/_layouts/15/AuthenticationPage/ResetPassword.aspx?token=@@ForgotPasswordLink</a><br/>
Once account setup is done, you may start editing the application via the link below.
<br/> <a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>' where Email_Template='CollaborationNewInvitation'

