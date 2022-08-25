ALTER TABLE TB_INCUBATION_APPLICATION add  Awarded BIT not null default 0;
ALTER TABLE TB_CCMF_Application add  Awarded BIT not null default 0;

ALTER TABLE TB_CCMF_APPLICATION_HISTORY add  Awarded BIT not null default 0;
ALTER TABLE TB_INCUBATION_APPLICATION_HISTORY add  Awarded BIT not null default 0;

