USE [CyberportWMS]
GO
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Application_Updated_BDM', N'<p>Hi,</p>
<br />
<p>Applicant has updated the application with your comment:<br />
@@Comment</p>
<br />
<p>Please assess the application by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Application_Updated_Coordinator', N'<p>Hi,</p>
<br />
<p>Applicant has updated the application with your comment:<br />
@@Comment</p>
<br />
<p>Please assess the application by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'BDM_Reject_Validation', N'<p>Hi,</p>
<br />
<p>BDM has rejected the validation of the application @@AppNumber with the following comments:<br />
@@Comment</p>
<br />
<p>Please assess the application again by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')

INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Daily_Notification_BDM', N'<p>Hi,</p>
<br />
<p>The following list of Applications were updated and ready for Scoring / Disqualified.<br />
<List of applications><br />
@@AppList</p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Duplicated_Submission', N'<p>Hi,</p>
<br />
<p>The following application submissions maybe duplicated:<br />
@@DupList​</p>​
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')

INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Manual_Email_BDM', N'<p>Hi,</p>
<br />
<p>The applications of @@ProgramName for @@IntakeNumber are eligibility checked or to be disqualified. Please access and score the applications by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Manual_Email_CPMO', N'<p>Hi,</p>
<br />
<p>The applications of @@ProgramName for @@IntakeNumber are reviewed by Senior Manager. Please access and score the applications by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Manual_Email_Final_BDM', N'<p>Hi,</p>
<br />
<p>The applications of @@ProgramName for @@IntakeNumber are reviewed by CPMO. Please access and complete the application screening by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Manual_Email_SrMgr', N'<p>Hi,</p>
<br />
<p>The applications of @@ProgramName for @@IntakeNumber are reviewed by BDM. Please access and score the applications by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Manual_Email_VTco', N'<p>Hi,</p>
<br />
<p>The applications of @@ProgramName for @@IntakeNumber had completed screening. Please arrange the presentation sessions and venue by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Presentation_Invitation', N'<p>Hi,</p>
<br />
<p>We would like to invite you for the presentation of @@ProgramName for intake @@IntakeNumber on @@VettingDate at @@VettingVenue from @@PresentationFrom to @@PresentationTo.<br />
Please response your availability with URL below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')

INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Reminder_Presentation_Applicants', N'<p>Hi,</p>
<br />
<p>We would like to remind you for the presentation of @@ProgramName for intake @@IntakeNumber on @@VettingDate at @@VettingVenue from @@PresentationFrom to @@PresentationTo.</p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Reminder_Presentation_VT', N'<p>Hi,</p>
<br />
<p>We would like to remind you for the vetting section of @@ProgramName for intake @@IntakeNumber on @@VettingDate at @@VettingVenue from @@PresentationFrom to @@PresentationTo.</p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Request_Update_Application', N'<p>Hi,</p>
<br />
<p>There is missing or invalid Information of your application of @@ProgramName for @@IntakeNumber (application no. @@AppNumber):<br />
@@Comment</p>
<br />
<p>Please update your application accordin​gly by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Shortlisting_CPMO', N'<p>Hi,</p>
<br />
<p>BDM has confirmed the score and shortlisted the application @@AppNumber​ with the following score:<br />
BDM Score: @@BDMScore<br />
Senior Manager Score: @@SrMgrScore</p>​
<br /> 
<p>Please assess and score the application by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Shortlisting_Final_BDM', N'<p>Hi,</p>
<br />
<p>BDM has confirmed the score and shortlisted the application @@AppNumber​ with the following score:<br />
BDM Score: @@BDMScore<br />
Senior Manager Score: @@SrMgrScore<br />
CPMO Score: @@CPMOScore​</p>​
<br /> 
<p>Please assess and score the application by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Shortlisting_SrMgr', N'<p>Hi,</p>
<br />
<p>BDM has confirmed the score and shortlisted the application @@AppNumber​ with the following score:<br />
BDM Score: @@BDMScore</p>
<br /> 
<p>Please assess and score the application by the link below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Start_Eligibility_Checking', N'<p>Hi,</p>
<br />
<p>@@ProgramName intake @@IntakeNumber is due, there are @@Total applications in total.<br />
Please access to the link below for eligibility checking.<Br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Successful_Application_Email', N'<p>Hi,</p>
<br />
<p>Please noted that your application for @@ProgramName intake @@IntakeNumber is successed. Further Information will be provided in the coming email.
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Vetting_Email_to_Coordinator', N'<p>Hi,</p>
<br />
<p>The following list of Applications had completed screening and remarked for Vetting:<br />
@@VetList</p>
<br />
<p>Please arrange the presentation sessions and venue.<br />
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'Vetting_Meeting_Invitation', N'<p>Hi,</p>
<br />
<p>We would like to invite you for the vetting meeting of @@ProgramName for intake @@IntakeNumber on @@VettingDate at @@VettingVenue from @@VettingMettingFrom to @@VettingMeetingTo.<br />
You can access the application details below:<br />
<a href="@@WebsiteUrl" target="_blank">@@WebsiteUrl</a></p>
<br />
<p>Regards,</p>
<p>EMS Administrator</p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'ZipDownloadEndEmail', N'<p>Hi, Zip File is ready, please download : @@m_downloadlink. <br/>Password is : @@m_Password <br/>Zip Start time : @@m_Starttime to End Time @@m_Endtime </p>')
INSERT [dbo].[TB_EMAIL_TEMPLATE] ([Email_Template], [Email_Template_Content]) VALUES (N'ZipDownloadStartEmail', N'<p>Hi, Zip File is processing, please wait next email to confirm Zip File is ready.</p>')


SET IDENTITY_INSERT [dbo].[TB_SYSTEM_PARAMETER] ON 

INSERT [dbo].[TB_SYSTEM_PARAMETER] ([ID], [Config_Code], [Value], [Created_by], [Created_date], [Modified_by], [Modified_date]) VALUES (15, N'zipfiledownloadurl', N'http://cyberportemssp:10870/', NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[TB_SYSTEM_PARAMETER] OFF
