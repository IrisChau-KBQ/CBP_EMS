update TB_EMAIL_TEMPLATE set Email_Template_Content='<p>Hi,</p>

<p>
Your application for @@ProgramName intake @@intakeno has been submitted to Cyberport Entrepreneurship Management System. You may access to the application form by the link below.
<br/><a href="@@ApplicationUrl" target="_blank">@@ApplicationUrl</a><br />

 
<p>Regards,</p>
<p>EMS Administrator</p>' where Email_Template='Application_Applicant_Submitted'


update TB_EMAIL_TEMPLATE set Email_Template_Content='<p>Hi,</p>

<p>
Your application for @@ProgramName intake @@intakeno has been resubmitted to Cyberport Entrepreneurship Management System. You may view  the application form by the link below.<br/>
<a href="@@ApplicationUrl" target="_blank">@@ApplicationUrl</a><br />

 
<p>Regards,</p>
<p>EMS Administrator</p>' where Email_Template='Requestor_Applicant_Resubmitted'