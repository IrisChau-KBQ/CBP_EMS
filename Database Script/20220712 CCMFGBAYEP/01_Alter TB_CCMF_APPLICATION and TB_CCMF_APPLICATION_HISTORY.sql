/*
Add field for TB_CCMF_APPLICATION and TB_CCMF_APPLICATION_HISTORY;

1.Question Field
Question2_1_2l
Question2_1_2m
Question2_1_2n

2 signature field 
Principal_2nd_Full_Name
Principal_2nd_Position_Title
*/

ALTER TABLE TB_CCMF_APPLICATION ADD Question2_1_2l bit null;
ALTER TABLE TB_CCMF_APPLICATION ADD Question2_1_2m bit null;
ALTER TABLE TB_CCMF_APPLICATION ADD Question2_1_2n bit null;
ALTER TABLE TB_CCMF_APPLICATION ADD Principal_2nd_Full_Name nvarchar(255) null;
ALTER TABLE TB_CCMF_APPLICATION ADD Principal_2nd_Position_Title nvarchar(255) null;


ALTER TABLE TB_CCMF_APPLICATION_HISTORY ADD Question2_1_2l bit null;
ALTER TABLE TB_CCMF_APPLICATION_HISTORY ADD Question2_1_2m bit null;
ALTER TABLE TB_CCMF_APPLICATION_HISTORY ADD Question2_1_2n bit null;
ALTER TABLE TB_CCMF_APPLICATION_HISTORY ADD Principal_2nd_Full_Name nvarchar(255) null;
ALTER TABLE TB_CCMF_APPLICATION_HISTORY ADD Principal_2nd_Position_Title nvarchar(255) null;

