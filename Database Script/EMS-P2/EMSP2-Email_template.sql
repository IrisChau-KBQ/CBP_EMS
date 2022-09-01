 alter table TB_CASP_SPECIAL_REQUEST add Status nvarchar(50) null
 go;
 insert into TB_EMAIL_TEMPLATE(Email_Template,Email_Template_Content)
  values
  ('CASP_Reimbursement_ReSubmitted_Applicant','<p>Hi,</p><br /><p>Please noted that your application (@@AppNumber) for @@ProgramName is successfully re-submitted. <br /><p>Regards,</p><p>EMS Administrator</p>'),
  ('CASP_Reimbursement_Submitted','<p>Dear applicant,</p><br /><p>Your CASP Financial Assistance Reimbursement form @@Application_No has been successfully submitted.You can view the application by the link below:<br /><p><a href="@@ApplicationUrl">@@ApplicationUrl</a></p><p>Should you have any questions, please contact us at <a href="mailto:collaboration@cyberport.hk">collaboration@cyberport.hk</a></p><p>Regards,</p><p>Cyberport EMS Administrator</p><p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p></p>'),
  ('CASP_Special_Request','<p>Dear applicant,</p><br /><p>Your CASP Special Request Form @@Application_No has been successfully submitted.You can view the application by the link below:<br /><p><a href="@@ApplicationUrl">@@ApplicationUrl</a></p><p>Should you have any questions, please contact us at <a href="mailto:collaboration@cyberport.hk">collaboration@cyberport.hk</a></p><p>Regards,</p><p>Cyberport EMS Administrator</p><p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)</p></p>')