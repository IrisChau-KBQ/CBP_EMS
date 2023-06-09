update [TB_EMAIL_TEMPLATE] 
set [Email_Template_Content]   = 
'<!DOCTYPE html>
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
.PLeft{
    text-indent: 40px;
}

</style>
</head>
<body>

<p>Dear Vetting Members,</p>
<p>
Thank you for your kind support to be one of the Vetting Members of the Vetting Meetings and Presentation Sessions of Cyberport Incubation Programme (CPIP) – @@IntakeNumber which will be held on <span class="fontboldborderbottom">@@VettingDate</span>. Please refer to the meeting details as follows:
</p>

<p>
Date : @@VettingDate<br>
Time : @@VettingMettingFrom - @@VettingMeetingTo (Vetting Meeting)<br>
<span class="PLeft">@@PresentFrom - @@PresentTo (Presentation Session)</span><br>
Venue : @@VettingVenue<br>
Map : <a href="http://www.cyberport.hk/en/about_cyberport/about_cyberport_map">http://www.cyberport.hk/en/about_cyberport/about_cyberport_map</a><br>
</p>
<p>
I am pleased to inform you that Cyberport Entrepreneurs Team has received a total of <span class="fontbold">@@TotalEligibleApplications eligible CPIP applications</span> by the deadline on @@ApplicationDeadline. Those <span class="fontbold">@@TotalEligibleApplications CPIP applications</span> (@@TotalShortlistedApplications shortlisted applications) are uploaded to the FTP server <a href="http://ftp.cyberport.hk">ftp.cyberport.hk</a> and you can use the designated user name and password sending to you via SMS for your assessment. In addition, you can also login to the <a href="https://ems.cyberport.hk/">Entrepreneurship Management System (EMS)</a> to view all the applications. We have created an EMS account for you and an invitation email has been sent to you, please reset your password for the first time login.
</p>
<p>
You are welcomed to login the EMS to conduct the marking before or during the Vetting Meeting and Presentation Session. A brief user manual for the EMS is attached for your information.
</p>
<p>
Should you require a car park ticket, please kindly inform us <span class="fontboldborderbottom">by @@CarParkDeadline</span> for arrangement. We look forward to seeing you! Should you have further enquiries, please feel free to contact Mr. Franco Leung at 3166 3814 or Ms. Dorothy Yim at 3166 3723.
<p>

<p>
<div class="fontbold">Remarks:</div>
<div>According to “Policy for Application Assessment – Entrepreneurship Programmes”, a Vetting Team, consist of at least <span class="fontboldborderbottom">five</span> Vetting Team Members (Including Vetting Team Leader) selected from ECAG and any other subject matter experts approved by the EC Chairman, will be formed to assess the applications. The shortlisted applicants are required to enter the Presentation Session which consists of at least <span class="fontboldborderbottom">three</span> of the Vetting Team Members. Please be punctual in order to secure the quorum for the meetings.</div>
</p>
<p>
Prior to the vetting process, you are kindly advised to read the <span class="fontbold">Briefing Notes</span> as attached in detail. Please login to the EMS for completing the Note of Declaration of Interests. You are obliged to give clear indication of any direct personal or pecuniary interests in any application.
</p>
<p>
Best Regards,<br>
Entrepreneurs Team
</p>

<p>
For and on behalf of,<br>
Alice So<br>
Secretary to Entrepreneurship Committee (EC)<br>
Hong Kong Cyberport Management Company Limited<br>
</p>
</body>
</html>'
where [Email_Template] = 'Vetting_Team_Invitaion_CPIP'
Go

update [TB_EMAIL_TEMPLATE] 
set [Email_Template_Content] =
'<!DOCTYPE html>
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
.PLeft{
    text-indent: 40px; 40px;
}
</style>
</head>
<body>

<p>Dear Vetting Members,</p>
<p>
Thank you for your kind support to be one of the Vetting Members of the Vetting Meetings and Presentation Sessions of Cyberport Creative Micro Fund (CCMF) – @@IntakeNumber which will be held on <span class="fontboldborderbottom">@@VettingDate</span>. Please refer to the meeting details as follows:
</p>

<p>
Date : @@VettingDate<br>
Time : @@VettingMettingFrom - @@VettingMeetingTo (Vetting Meeting)<br>
<span class="PLeft">@@PresentFrom - @@PresentTo (Presentation Session)</span><br>
Venue : @@VettingVenue<br>
Map : <a href="http://www.cyberport.hk/en/about_cyberport/about_cyberport_map">http://www.cyberport.hk/en/about_cyberport/about_cyberport_map</a><br>
</p>
<p>
I am pleased to inform you that Cyberport Entrepreneurs Team has received a total of <span class="fontbold">@@TotalEligibleApplications eligible CCMF applications</span> by the deadline on @@ApplicationDeadline. Those <span class="fontbold">@@TotalEligibleApplications CCMF applications</span> (@@TotalShortlistedApplications shortlisted applications) are uploaded to the FTP server <a href="http://ftp.cyberport.hk">ftp.cyberport.hk</a> and you can use the designated user name and password sending to you via SMS for your assessment. In addition, you can also login to the <a href="https://ems.cyberport.hk/">Entrepreneurship Management System (EMS)</a> to view all the applications. We have created an EMS account for you and an invitation email has been sent to you, please reset your password for the first time login.
</p>
<p>
You are welcomed to login the EMS to conduct the marking before or during the Vetting Meeting and Presentation Session. A brief user manual for the EMS is attached for your information.
</p>
<p>
Should you require a car park ticket, please kindly inform us <span class="fontboldborderbottom">by @@CarParkDeadline</span> for arrangement. We look forward to seeing you! Should you have further enquiries, please feel free to contact Ms. Flora Yeung at 3166 3903.
<p>

<p>
<div class="fontbold">Remarks:</div>
<div>According to “Policy for Application Assessment – Entrepreneurship Programmes”, a Vetting Team, consist of at least <span class="fontboldborderbottom">five</span> Vetting Team Members (Including Vetting Team Leader) selected from ECAG and any other subject matter experts approved by the EC Chairman, will be formed to assess the applications. The shortlisted applicants are required to enter the Presentation Session which consists of at least <span class="fontboldborderbottom">three</span> of the Vetting Team Members. Please be punctual in order to secure the quorum for the meetings.</div>
</p>
<p>
Prior to the vetting process, you are kindly advised to read the <span class="fontbold">Briefing Notes</span> as attached in detail. Please login to the EMS for completing the Note of Declaration of Interests. You are obliged to give clear indication of any direct personal or pecuniary interests in any application.
</p>
<p>
Best Regards,<br>
Flora Yeung
</p>

<p>
For and on behalf of,<br>
Alice So<br>
Secretary to Entrepreneurship Committee (EC)<br>
Hong Kong Cyberport Management Company Limited<br>
</p>
</body>
</html>'
where Email_Template = 'Vetting_Team_Invitaion_CCMF'
GO

