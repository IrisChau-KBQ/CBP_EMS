UPDATE [TB_EMAIL_TEMPLATE]
   SET [Email_Template_Content] = N'<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<style>
body{
	font-size:14px;
}
.title{
	font-weight: bold;
    font-size: 18px;
    border-bottom: 2px black solid;
    margin-bottom: 6px;
}
.fontboldborderbottom{
	font-weight: bold;
	border-bottom: 2px black solid;
}
.fontborderbottom{
	border-bottom: 1px black solid;
}
.fontbold{
	font-weight: bold;
}
.ulfont{
	margin-left: -22px;
    margin-top: 2px;
}
</style>
</head>
<body>
<p>Dear CCMF applicant,</p>
<p>You are cordially invited to attend the Presentation Session of the Cyberport Creative Micro Fund (CCMF) @@IntakeNumber Recruitment on <span class="fontboldborderbottom">@@VettingDate</span>. Please find the meeting details as follows:</p>
<p>
<span class="fontbold">Date:</span> @@VettingDate<br>
<span class="fontbold">Presentation Time:</span> @@PresentationFrom (Please arrive 15 minutes earlier than the presentation time.)<br>
<span class="fontbold">Venue:</span> @@VettingVenue<br>
<span class="fontbold">Map:</span> <a href="http://www.cyberport.hk/en/about_cyberport/about_cyberport_map">http://www.cyberport.hk/en/about_cyberport/about_cyberport_map</a><br>
</p>
<p>
<div><span class="title">Registration</span></div>
<div>Please register at <a href="@@WebsiteUrl">@@WebsiteUrl</a> to confirm your attendance <span class="fontborderbottom">with name of attendees and the presentation tools</span> that you are going to use on or before <span class="fontboldborderbottom">@@ConfirmDeadline</span>.</div>
</p>
<p>
<div><span class="title">Attendance</span></div>
<div>Principal applicant must attend the Presentation Session.</div>
</p>
<p>
<div><span class="title">Presentation Session Language and Format</span></div>
<div>Conducted in English, the Presentation Session will consist of the following:</div>
<div>
	<ul class="ulfont">
		<li>A 5-minute Presentation</li>
		<li>A 3-minute Q&A Session</li>
	</ul>
</div>
<p>
<p>
The use of presentation tools, such as PowerPoint, videos, or prototype products, etc., are allowed during your Presentation Session.
</p>
<p>Please keep your presentation within the time limit. You will be asked to stop the presentation when the time is up.</p>
<p>
<div><span class="title">Presentation file</span></div>
<div>Please send the presentation file to <a href="@@WebsiteUrl">@@WebsiteUrl</a> on or before <span class="fontboldborderbottom">@@Deadline</span>. The file will be used for the presentation. No further update is allowed after submission.</div>
</p>
<p>
<div><span class="title">What You Need to Prepare</span></div>
<div>
You are highly recommended to cover the following major areas during your Presentation Session:
</div>
<div>
	<ul class="ulfont">
 		<li>Introduction of the Project Management Team</li>
 		<li>Value proposition of your project and the market needs it solves</li>
 		<li>Introduction of your products or services</li>
 		<li>Market positioning of your products or services among competitions</li>
 		<li>Explain the business model and revenue model</li>
 		<li>Describe how the project can be completed within the 6-month project milestone</li>
	</ul>
</div>
</p>
<p>
<div><span class="title">Remarks</span></div>
<div>
	<ul class="ulfont">
		<li>For individual applicants, please bring your HKID card (for both Professional and Hong Kong Young Entrepreneur Programme (HKYEP) streams) and Hong Kong Student ID Card with valid expiry date/Graduation Certificate (for HKYEP teams only) for eligibility check.</li>
		<li>For company applicants, please bring your business registration certificate, the NC1 form with shares and directors information, and your HKID card/passport for eligibility check.</li>
		<li>Applicants who do not fulfil the eligibility criteria for the CCMF streams that they apply, will be disqualified on-site and will not be allowed to enter into the Presentation Session.</li>
	</ul>
</div>
</p>
<p>
<div><span class="title">FAQ</span></div>
<div class="fontbold">Q: I am not in HK on the presentation date, could I change to another date?</div>
<div>獲安排參與演示會議那天我並非身處香港，我能否更改演示會議的日期？</div>
<div class="fontbold">A: Please note that the date for vetting and presentation session could not be changed. Applicants could conduct the presentation via Skype.</div>
<div>請注意演示會議的日期和時間將不能更改。如申請人不在香港，則可以透過Skype 進行演示。</div>
</p>
<p>
<div class="fontbold">Q: How many rounds of interview do you have?</div>
<div>如獲選入圍，我將會有多少次面試機會？</div>
<div class="fontbold">A: Only one round of presentation session will be conducted for each recruitment.</div>
<div>在每一屆招募中，入圍申請人只會獲得一次面試機會。</div>
</p>
<p>
Should you have any enquiries, please feel free to contact me at <span class="fontborderbottom">maggiekwok@cyberport.hk</span> or 3166 3907.
<p>
<p>
<span class="fontbold">Maggie Kwok</span><br>
Assistant Manager - Business Development<br>
Entrepreneurs Team<br>
T +852 3166 3907 F +852 3027 0408<br>
</p>

</body>

</html>'
 WHERE [Email_Template] = 'Presentation_Invitation_CCMF'


 UPDATE [TB_EMAIL_TEMPLATE]
   SET [Email_Template_Content] = N'<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<style>
body{
	font-size:14px;
}
.title{
	font-weight: bold;
    font-size: 18px;
    border-bottom: 2px black solid;
    margin-bottom: 6px;
}
.fontboldborderbottom{
	font-weight: bold;
	border-bottom: 2px black solid;
}
.fontborderbottom{
	border-bottom: 1px black solid;
}
.fontbold{
	font-weight: bold;
}
.ulfont{
	margin-left: -22px;
    margin-top: 2px;
}
</style>
</head>
<body>
<p>Dear CPIP applicant,</p>
<p>You are cordially invited to attend the Presentation Session of the Cyberport Incubation Programme (CPIP) @@IntakeNumber Recruitment on <span class="fontboldborderbottom">@@VettingDate</span>. Please find the meeting details as follows:</p>
<p>
<span class="fontbold">Date:</span> @@VettingDate<br>
<span class="fontbold">Presentation Time:</span> @@PresentationFrom (Please arrive 15 minutes earlier than the presentation time.)<br>
<span class="fontbold">Venue:</span> @@VettingVenue<br>
<span class="fontbold">Map:</span> <a href="http://www.cyberport.hk/en/about_cyberport/about_cyberport_map">http://www.cyberport.hk/en/about_cyberport/about_cyberport_map</a><br>
</p>
<p>
<div><span class="title">Registration</span></div>
<div>Please register at <a href="@@WebsiteUrl">@@WebsiteUrl</a> to confirm your attendance <span class="fontborderbottom">with name of attendees and the presentation tools</span> that you are going to use on or before <span class="fontboldborderbottom">@@ConfirmDeadline</span>.</div>
</p>
<p>
<div><span class="title">Attendance</span></div>
<div>Principal applicant must attend the Presentation Session.</div>
</p>
<p>
<div><span class="title">Presentation Session Language and Format</span></div>
<div>Conducted in English, the Presentation Session will consist of the following:</div>
<div>
	<ul class="ulfont">
		<li>A 5-minute Presentation</li>
		<li>A 3-minute Q&A Session</li>
	</ul>
</div>
<p>
<p>
The use of presentation tools, such as PowerPoint, videos, or prototype products, etc., are allowed during your Presentation Session.
</p>
<p>Please keep your presentation within the time limit. You will be asked to stop the presentation when the time is up.</p>
<p>
<div><span class="title">Presentation file</span></div>
<div>Please send the presentation file to <a href="@@WebsiteUrl">@@WebsiteUrl</a> on or before <span class="fontboldborderbottom">@@Deadline</span>. The file will be used for the presentation. No further update is allowed after submission.</div>
</p>
<p>
<div><span class="title">What You Need to Prepare</span></div>
<div>
You are highly recommended to cover the following major areas during your Presentation Session:
</div>
<div>
	<ul class="ulfont">
 		<li>Introduction of the Project Management Team</li>
 		<li>Value proposition of your project and the market needs it solves</li>
 		<li>Introduction of your products or services</li>
 		<li>Market positioning of your products or services among competitions</li>
 		<li>Explain the business model and revenue model</li>
 		<li>Describe how the project can be completed within the 6-month project milestone</li>
	</ul>
</div>
</p>
<p>
<div><span class="title">Remarks</span></div>
<div>
	<ul class="ulfont">
		<li>For individual applicants, please bring your HKID card (for both Professional and Hong Kong Young Entrepreneur Programme (HKYEP) streams) and Hong Kong Student ID Card with valid expiry date/Graduation Certificate (for HKYEP teams only) for eligibility check.</li>
		<li>For company applicants, please bring your business registration certificate, the NC1 form with shares and directors information, and your HKID card/passport for eligibility check.</li>
		<li>Applicants who do not fulfil the eligibility criteria for the CPIP streams that they apply, will be disqualified on-site and will not be allowed to enter into the Presentation Session.</li>
	</ul>
</div>
</p>
<p>
<div><span class="title">FAQ</span></div>
<div class="fontbold">Q: I am not in HK on the presentation date, could I change to another date?</div>
<div>獲安排參與演示會議那天我並非身處香港，我能否更改演示會議的日期？</div>
<div class="fontbold">A: Please note that the date for vetting and presentation session could not be changed. Applicants could conduct the presentation via Skype.</div>
<div>請注意演示會議的日期和時間將不能更改。如申請人不在香港，則可以透過Skype 進行演示。</div>
</p>
<p>
<div class="fontbold">Q: How many rounds of interview do you have?</div>
<div>如獲選入圍，我將會有多少次面試機會？</div>
<div class="fontbold">A: Only one round of presentation session will be conducted for each recruitment.</div>
<div>在每一屆招募中，入圍申請人只會獲得一次面試機會。</div>
</p>
<p>
Should you have any enquiries, please feel free to contact me at <span class="fontborderbottom">maggiekwok@cyberport.hk</span> or 3166 3907.
<p>
<p>
<span class="fontbold">Maggie Kwok</span><br>
Assistant Manager - Business Development<br>
Entrepreneurs Team<br>
T +852 3166 3907 F +852 3027 0408<br>
</p>
</body>

</html>'
 WHERE [Email_Template] = 'Presentation_Invitation_CPIP'


Update TB_EMAIL_TEMPLATE
SET Email_Template_Content = 
'<p>Hi,</p><br/><br/>
<p>The Presentation Session of @@ProgramName for @@IntakeNumber on @@VettingDate at @@VettingVenue  from @@PresentationFrom to @@PresentationTo is cancelled.</p><br/><br/>
<p>Regards,</p><br/>
<p>Cyberport EMS Administrator</p><br/><br/>
<p>(This message is auto-generated from the system. Please do not reply to this email. The messages or attachments received from this email address cannot be processed.)'
where Email_Template = 'Presentation_Invitation_cancel'
GO

