


--2.TABLE TB_COMPANY_CONTACT

Alter TABLE TB_COMPANY_CONTACT
Add	Salutation nvarchar(50) NULL

--3.TABLE TB_COMPANY_IP

Alter TABLE TB_COMPANY_IP
Add	Title nvarchar(255) NULL

--4.TABLE TB_COMPANY_FUND

Alter TABLE TB_COMPANY_FUND	
Add	Reported_Date datetime NULL,
	Received_Date datetime NULL

--5.TABLE TB_COMPANY_CONTACT
Alter TABLE TB_COMPANY_CONTACT	
Add	Salutation nvarchar(50) NULL,
	HKID nvarchar(50) NULL,
	Graduation_Date datetime NULL,
	Education_Institution nvarchar(255) NULL,
	Student_ID nvarchar(50) NULL,
	Programme_Enrolled nvarchar(255) NULL,
	Organization_Name nvarchar(255) NULL,
	Contact_No_Home nvarchar(20) NULL,
	Contact_No_Office nvarchar(20) NULL,
	Area nvarchar(255) NULL,
    Masked_HKID nvarchar(255) NULL
