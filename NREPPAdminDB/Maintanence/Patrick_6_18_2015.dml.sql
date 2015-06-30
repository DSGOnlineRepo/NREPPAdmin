﻿GO

SET NOCOUNT ON
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Co-occurring/ dual disorders', 'incidence/ prevalence/ status of co-occurring or dual mental health and substance-use disorders or behavioral and physical health disorders or disabilities', 'Mental Health', 'measures that assess only substance use, mental health, and/ or physical health, even if doing so within a co-occurring disorder population--instead, code the separate measures as their separate outcomes')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Obsessive-compulsive disorders/ symptoms', 'depressive symptoms, incidence/ prevalence/ status of depression diagnoses (e.g., maternal depression, postnatal depression, major depression, dysthymic disorder, bipolar depression, schizoaffective depression)', 'Mental Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Other anxiety disorders and symptoms', 'symptoms of anxiety; incidence/ prevalence/ status of anxiety disorders other than trauma- and stressor-related disorders; include phobias, generalized anxiety disorder, panic attacks, panic disorder, separation anxiety disorder, selective mutism', 'Mental Health', '"disorders and symptoms included under ""Trauma-and stressor-related disorders"" or  ""Obsessive-compulsive disorders""--instead, code them as the more specific outcome "')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Trauma- and stressor-related disorders and symptoms', 'trauma symptoms (described as such), incidence/ prevalence/ status of traumatic stress-related diagnoses (e.g., post-traumatic stress disorder/ PTSD, acute stress disorder, adjustment disorders, reactive attachment disorder), dissociative symptoms and disorders (e.g., depersonalization, dissociative identity disorder, multiple personality disorder)', 'Mental Health', '"general measures of attachment--these should be coded as ""Social functioning"" instead"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('General psychological health', 'mental health symptoms that do not fall within other NREPP outcomes and scales and composite measures assessing mental health symptoms falling under multiple NREPP outcomes (including age of onset); This outcome also includes measures of inward emotional experience, including stress management, coping, emotional resilience, hopefulness, feelings of distress, emotional self-regulation, self concept, self-esteem, self-efficacy, empowerment, meaning of life, locus of control, negative thought patterns, anger management; positive youth/ child development', 'Mental Health', '"1) symptoms described under the NREPP ""Serious mental illness/ Serious emotional disturbance"" outcomes (these should be coded as the specific ""Serious mental illness/ Serious emotional disturbance"" outcome to which they apply), 2) measure concepts included in the NREPP ""Functioning"" outcomes, 3) anger management--this should be coded as ""Aggression, violence, and externalizing behavior"" "')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Aggression, violence, and externalizing behaviors', 'This outcome is intended to assess aggression, violence, and externalizing behaviors of the person who is the ultimate target of the intervention (that is, the person whose mental health or substance use is of concern).  Aggression, violence, and externalizing behaviors include perpetration of bullying, violence, abuse, harm to other people, animals, or property; physical aggression; anger control; anger management; hostility; homicidality; impulsivity; hyperactivity; all without explicit reference to criminality', 'Mental Health', '"1) actions explicitly resulting in arrests, convictions, jail/ prison/ juvenile detention tim--these should instead be recorded under ""Criminal justice and delinquency""; 2) incidence/ prevalence/ status of disorders defined in other NREPP outcomes; 3) conflict management skills--this should be coded as ""Social functioning""; 4) similar concepts assessed in family members or caregivers of the ultimate target population--these should instead be coded as ""Family/ caregiver knowledge, attitudes, and behaviors."""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Autism spectrum disorder/ symptoms', 'incidence/ prevalence/ status of autism spectrum disorder diagnoses (e.g., autism, Asperger''s disorder,  childhood disintegrative disorder, pervasive developmental disorder); restricted repetitive behaviors, interests, and activities (RRBs); measures assessing a combination of social and RRB symptoms of autism spectrum disorders', 'Mental Health', '"measures that solely assess deficits in social communication and social interaction--instead, these should be coded as ""Social functioning"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Borderline personality disorder', 'incidence/ prevalence/ status of borderline personality disorder ', 'Mental Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Feeding and eating disorders/ symptoms', 'incidence/ prevalence/ status of eating disorder diagnoses (e.g., bulimia, anorexia nervosa, pica); eating-disorder-related symptoms; feeding disorders and symptoms', 'Mental Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Self-injury', 'behaviors done with intention to self-harm that are not explicit suicide attempts (e.g., cutting, slitting wrists, self-mutilation)', 'Mental Health', '"restrictive repetitive behaviors, interests, and activities (RRBs) described as a manifestation of autism spectrum disorders--instead, these should be coded as ""Autism spectrum disorders/ symptoms"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Unspecified and other mental health disorders', '"incidence/ prevalence/ status of ""mental health problems,"" ""mental disorders,"" or ""mental illnesses"" when outcomes are reported for either 1) individuals described using one or more of these terms rather than a specific diagnostic term, 2) individuals/ groups with mental health diagnoses or disorders that do not fall within other NREPP outcomes, or 3) a group of individuals encompassing multiple diagnostic categories"', 'Mental Health', '"1) incidence/ prevalence/ status of individuals or groups described using terms for the NREPP ""Serious Mental Illness/ Serious Emotional Disturbance"" outcome category (these should instead be coded as ""Unspecified serious mental illness/ serious emotional disorder""), 2) measures of specific types of symptoms--these should instead be coded under the corresponding outcome that includes those symptoms or, if a general measure of mental health symptoms, under ""General psychological health"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Bipolar disorder and manic symptoms', 'manic symptoms (described as such); incidence/ prevalence/ status of bipolar disorder diagnoses; measures of symptoms associated with bipolar disorder that include manic symptoms as well as depressive or psychotic symptoms', 'Mental Health', '"measures that assess only depressive symptoms or only psychotic symptoms--instead, these should be coded as ""Depression and depressive symptoms"" or ""Psychotic disorders and symptoms,"" respectively"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Depression and depressive symptoms', 'depressive symptoms, incidence/ prevalence/ status of depression diagnoses (e.g., maternal depression, postnatal depression, major depression, dysthymic disorder, bipolar depression, schizoaffective depression)', 'Mental Health', '"measures that assess only only psychotic symptoms associated with diagnoses of depression--instead, these should be coded as ""Psychotic disorders and symptoms"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Disruptive behavior disorders', 'incidence/ prevalence/ status of disruptive behavior disorder diagnoses (e.g., attention deficit/ hyperactivity disorder--ADD or ADHD; oppositional defiant disorder; conduct disorder; intermittent explosive disorder).', 'Mental Health', '"measures of hyperactivity, attention, aggression, etc.--instead, these should be coded as ""Aggression, violence, and externalizing behavior"" or ""Cognitive functioning"" (for attention) . This outcome is only applicable to assessments of children. ADHD in adults would fall under XXXX"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Psychotic disorders and symptoms', '"hallucinations, delusions, bizarre behavior, formal thought disorder, paranoia, catatonia symptoms, regardless of the diagnosis with which they are associated (e.g., if the measure assesses psychotic symptoms associated with depression or bipolar disorder, include it here); pre-psychotic symptoms (e.g., unusual beliefs, illusions, strange sensations); incidence/ prevalence/ status of psychotic disorders (schizophrenia, schizoaffective disorder, schizophreniform disorder, brief reactive psychosis, delusional disorder, catatonia); also use this category if a general category of ""psychotic disorders"" is assessed that includes depression, mood/ affective, or bipolar disorders with psychotic features"', 'Mental Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Suicidality', 'suicide attempts, suicidal thoughts or ideation', 'Mental Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Unspecified serious mental illness/ serious emotional disturbance', '"incidence/ prevalence/ status of ""serious mental illness,"" ""severe and persistent mental illness,"" ""chronic mental illness,"" ""psychiatric or mental health disabilities,"" or ""serious emotional disturbances"" when outcomes are reported for 1) individuals described using one or more of these terms rather than a specific diagnosis or 2) a group encompassing multiple diagnoses falling within the NREPP ""Serious Mental Illness/ Serious Emotional Disturbance"" outcome category"', 'Mental Health', '"1) individuals or groups described using more general terms, such as mental health problems, mental disorders, or mental illnesses--these should be coded as ""Unspecified and other mental health disorders""; 2) measures of specific types of symptoms--these should instead be coded under the corresponding outcome that includes those symptoms or, if a general measure of mental health symptoms, under ""General psychological health"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Educational achievement/ attainment', 'graduation rate, on-time graduation, credit completion, GED attainment, highest education level/ degree attained, standardized achievement test scores, literacy, school readiness', 'Other', '"1) class test scores or course grades--these should be coded as ""School engagement/ connectedness, 2) scores on cognitive/ neuropsychological tests--these should instead be coded under ""Cognitive functioning"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Employment', 'earned income, competitive employment, job attainment, job retention; hourly, weekly, monthly, annual dollars earned; job benefits (e.g., health insurance); job types; career advancement; job satisfaction; work experience (e.g., transitional employment, internships, volunteer experience, apprenticeships)', 'Other', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Financial management competency', 'financial literacy; savings, checking, credit account ownership and management; amount of savings and investments; cash-flow management; investment knowledge/ skills; financial planning; tax skills; budgeting, balancing checkbook skills', 'Other', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Work readiness', 'job search competence, technical job competencies, work readiness competencies, attitudes toward work', 'Other', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Child custody', 'family reunification; out-of-home child placement; child custody; loss of parental rights', 'Other', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Other physical health conditions', 'incidence/ prevalence/ status of physical health conditions, diseases, or disorders, including HIV and sexually transmitted diseases; physical health symptoms; cholesterol, diabetes, high blood pressure, waist circumference, blood sugar; scales or single-item or composite measures of health that do not specify the nature of the health problem; morbidity and mortality/ death (other than by suicide); hygiene (e.g., cleanliness, grooming)', 'Other', '"measure concepts associated with substance-induced diseases, 2) engagement in exercise, nutrition or other health care improvement programs--these should be coded as ""Physical healthcare service utilization"" instead"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Sexuality', 'frequency of sex, number of sexual partners, risky sexual behaviors (e.g., condom use, contraceptive use, drinking before sex, early sexual initiation), teen pregnancy (e.g., number pf pregnancies before age 20)', 'Other', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Substance-induced diseases', 'fetal alcohol syndrome, cirrhosis of the liver, hepatitis, Korsakoff''s dementia and other cognitive problems resulting for alcohol or drug use', 'Other', '"HIV and sexually transmitted diseases--these should be coded as ""Other physical health conditions"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Cognitive functioning', 'tests of general neuropsychological function, such as task orientation, concentration, attention, memory, etc.', 'Other Behavioral Health', '"1) class test scores or course grades--these should be coded as ""School engagement/ connectedness,"" 2) literacy or scores on standardized achievement tests--these should instead be coded as ""Educational achievement/ attainment"" "')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Criminal justice/ delinquency', 'arrests, incarceration, time in jail/ prison/ juvenile detention, prevalence/ incidence/ status of antisocial personality disorder diagnosis; crimes, misdemeanors, felonies; convictions; criminal/ delinquent offenses; criminal/ juvenile delinquency recidivism; violations of parole; court appearances', 'Other Behavioral Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('General functioning and well-being', '"In general, this outcome is distinguished by its focus on the interaction of the person with their environment.  ""Wellness"" and ""well-being"" are also included here (rather than under ""General psychological health"") because they may include aspects of environmental interaction or well-being beyond just mental, emotional, or psychological health.  Include scales and composite measures of ""functioning,"" ""well-being,"" ""wellness,"" ""activities of daily living (ADLs),"" ""instrumental activities of daily living (IADLs),"" behavioral health disability, quality of life, and life satisfaction that include measure concepts associated with multiple NREPP outcomes; single-item measures and scales of ""functioning,"" ""well-being,"" ""wellness,"" ADLs, IADLs, behavioral health disability, quality of life, and life satisfaction that do not further specify the measure concepts covered"', 'Other Behavioral Health', '"1) measures that focus SOLELY on measure concepts associated with a single other NREPP outcome (these should be coded under the single other NREPP outcome to which they apply), 2) measures specifically designed to assess mental health, substance use, or trauma ""recovery"" (these should be coded as ""Behavioral health recovery"")"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Housing and homelessness', 'home ownership, attainment of permanent housing, receipt of supportive housing, housing retention, homelessness, housing stability, relationship with landlord, paying the rent/ mortgage', 'Other Behavioral Health', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Behavioral health recovery', '"scales and composite measures specifically designed to assess mental health, substance use, or trauma ""recovery"" that include measure concepts associated with multiple NREPP outcomes; single-item measures and scales of mental health, substance use, or trauma ""recovery"" that do not further specify the measure concepts covered"', 'Other Behavioral Health', '"measures of ""recovery"" that focus SOLELY on measure concepts associated with a single other NREPP outcome (e.g., measures that assess only ""functioning"" or symptoms)--these should be coded under the single other NREPP outcome to which they apply"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Social functioning/ competence', 'This outcome is intended to assess social functioning and competence in the person who is the ultimate target of the intervention (that is, the person whose mental health or substance use is of concern).  The focus of this outcome is on how the person interacts with others.  Social functioning/ competence includes social skills, social information-processing, hostile attribution bias, conflict management skills, cooperation, negotiation, peer resistance, self-advocacy, help-seeking, empathy/ emotional understanding, perspective-taking, decision-making, substance use resistance skills; social functioning, sociability, social relationships (friendships, peer relations), social connectedness, marital satisfaction, general measures of attachment, deficits in social communication and social interaction associated with autism spectrum disorders; healthy selection of peers (e.g., involvement with prosocial or antisocial peers)', 'Other Behavioral Health', '"1) similar skills assessed in family members or caregivers of the ultimate target population--these should instead be coded as ""Family/ caregiver knowledge, attitudes, and behaviors""; 2) diagnosis or symptoms of diagnosis of reactive attachment disorder; 3) anger management--this should instead be coded as ""Aggression, violence, and externalizing behaviors"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Access to substances in the community', 'access to and availability and affordability of substances of abuse in the community  ', 'R&P for SU', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Family/ caregiver knowledge & attitudes', 'parental attitudes towards substance use; perceived parental attitudes toward substance use; family/ caregiver attitudes toward psychotherapy, medication, etc.;  knowledge of child development.', 'R&P for SU/MH', '"1) measures of societal perceptions, knowledge, etc. regarding substance used intended to assess program effects on society (rather than individuals)--these should instead be coded as ""Societal knowledge, attitudes, and beliefs"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Family/ caregiver behaviors', 'communication and implementation of clear rules and consequences regarding alcohol, tobacco, drug use, etc.; family management skills (e.g., supervision, monitoring, limit-setting, discipline, clear behavioral expectations, etc.); family relationship quality (e.g., close, caring, nurturing, supportive, etc.); positive parenting skills; involvement in child''s education (e.g., checks in on homework, attends parent-teacher meetings, etc.)', 'R&P for SU/MH', '"1) measures of attachment--these would instead be coded as ""Social functioning"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Knowledge, attitudes, and beliefs of individuals at risk ', '"This outcome is meant to assess factors that put individuals at risk for or protect against the likelihood of their actually experiencing (or continuing to experience) substance use or mental health disorders or negative consequences of them.  Include the individual''s perceived norms, e.g., their perceptions of substance use by peers or acceptance of substance use by society; beliefs about the causes of mental illness (e.g., include ""insight"" here); motivations and intentions regarding using substances; the individual''s attitudes toward or motivations and intentions regarding using behavioral health services, etc.  "', 'R&P for SU/MH', '"1) family and caregiver knowlege, attitudes, etc.--these should instead be coded as ""family and caregiver attitudes""; 2) measures of societal perceptions, knowledge, etc. intended to assess program effects on society (rather than individuals)--these should instead be coded as ""Societal knowledge, attitudes, and beliefs"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('School engagement/ connectedness', 'school attendance, tardiness, homework completion, class test scores, course grades, drop-out rates, feelings of connectedness and belonging to school, positive relationships with teachers', 'R&P for SU/MH', '" 1) scores on standardized achievement tests--these should instead be coded as ""Educational achievement/ attainment,"" 2) scores on cognitive/ neuropsychological tests--these should instead be coded under ""Cognitive functioning"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Victimization, trauma, and maltreatment', 'This outcome is intended to assess victimization and trauma as experienced by the person who is the ultimate target of the intervention (that is, the person whose mental health or substance use is of concern).  Victimization and trauma include being the recipient of bullying or a victim of crime; experiencing traumatic events (e.g., observing or experiencing violence or life-threatening or emotionally upsetting events, such as death of or separation from loved ones, natural or man-made catastrophes, military combat, etc); being a victim of child maltreatment, physical or sexual abuse, physical or emotional neglect; experiencing adverse childhood events.  ', 'R&P for SU/MH', '"1) the actions of families, caregivers, or others that may have resulted in the victim''s experience.  If the intervention aims to change the behavior of family members/ caregivers, the perpetration of actions that affect the victim should be coded as ""Family/ caregiver knowledge, attitudes, and behaviors.""  If the intervention aims to change the behavior of other aggressors, the change in behavior of those aggressors should be coded as ""Aggression, violence, and externalizing behaviors""; 2) child abuse, neglect, violence, or other actions that explicitly result in arrests, convictions, jail/ prison/ juvenile detention time--these should instead be recorded under ""Criminal justice and delinquency"""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Organizational climate', 'perceived safety; perceived fairness towards students, patients, clients; clarity of policies, etc.; the organization may be a school, hospital, prison, etc.; school climate', 'R&P for SU/MH', '"measures of workforce development--these should be coded, as applicable, as ""Behavioral health workforce development."""')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Community/ societal quality of life', 'social capital (collective or economic benefits derived from the preferential treatment and cooperation between individuals and groups); collective efficacy; social cohesion; informal social control', 'R&P for SU/MH', 'rates of other NREPP outcomes occurring in the community--these should be coded as the specific NREPP outcomes to which they apply')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Societal knowledge, attitudes, and beliefs ', 'This outcome is meant to assess measures of societal perceptions, knowledge, beliefs, attitudes, stigma, prejudice, and discrimination regarding substance use, mental illness, and behavioral healthcare.  It is intended to assess the intervention''s effects on society rather than individuals.  ', 'R&P for SU/MH', '"1) individual-level measures of knowledge, beliefs, etc. that put the individual at risk for using, abusing, or becoming dependent on substances--these should be coded as ""User/ potential user''s knowledge, attitudes, and beliefs regarding substance use;"" 2) ""Family/ caregiver knowledge, attitudes, and behaviors""--these should instead be coded under the outcome of that name"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Amphetamine/ stimulant non-medical use and disorders', 'abuse and dependence on amphetamines or stimulants (includes Ritalin, Concerta, Dexedrine, Adderall, diet pills, methamphetamine, crystal meth, speed, ice, etc.);  use of prescription amphetamines/ stimulants not prescribed to the user; greater use of prescription amphetamines/ stimulants than prescribed; withdrawal, craving, intoxication; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Cannabis (non-medical) use and disorders', 'non-medical use, abuse, dependence, and intoxication; marijuana, hashish, etc.; age at initiation ', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Cocaine use and disorders', 'use, abuse, dependence, intoxication, withdrawal; cocaine, crack, etc.; age at initiation ', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Hallucinogen use and disorders', 'use, abuse, dependence, intoxication; MDMA, LSD, acid, mushrooms, PCP, Special K, ecstasy, psilocybin, mescaline; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Opioid use and disorders', 'abuse, dependence, intoxication, withdrawal;  use of opioids not prescribed to the user; greater use of prescription opioids than prescribed; heroin, opium, prescription opioids (e.g., oxycodone, Oxycontin, Percoset, hydrocodone, Vicodin, morphine,fentanyl); craving; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Other prescription drug disorders', '"Abuse, dependence, withdrawal, intoxication; prescription drugs not falling under other specific outcomes (i.e., other than amphetamine/ stimulants; opioids; sedatives/ hypnotics/ anxiolytics); use of prescription drugs not prescribed to the user; greater use of prescription drugs than prescribed; disorders involving prescription drugs falling under multiple ""Druge use and disorder""  outcomes; disorders involving unspecified types of prescription drugs; age at initiation"', 'Substance Use', 'disorders involving prescription amphetamines/ stimulants, opioids, or sedatives/ hypnotics/ anxiolytics--these should be coded as the more specific outcomes')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Sedative, hypnotic, or anxiolytic use and disorders', 'non-medical use, abuse, or dependence on barbituates, benzodiazepines, sleeping pills, sedatives (e.g., Valium, Serepax, Ativan, Librium, Xanax, Rohypnol, GHB, etc.); withdrawal, craving, intoxication; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Alcohol use and disorders', 'alcohol use, abuse, dependence, intoxication, withdrawal; binge drinking; craving; delirium tremens; underage drinking; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Inhalant use or disorders', 'non-medical use, abuse, or dependence on inhalants such as nitrous oxide, glue, gas, paint thinner, etc.; intoxication; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Other substance use and disorders', '"use, abuse, or dependence of/ on drugs or substances not included among the other ""Drug use and disorders"" outcomes, unknown drugs or substances, or multiple drugs or substances; withdrawal, intoxication; age at initiation"', 'Substance Use', '"disorders regarding use of only one other ""Substance use"" outcomes (e.g., amphetamines/ stimulants, opioids, sedatives/ hypnotics/ anxiolytics, prescription drugs)--instead, code these as the more specific outcomes"')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Tobacco use or disorders', 'use or dependence of/ on cigarettes, cigars, chewing tobacco; withdrawal; age at initiation', 'Substance Use', '')
INSERT INTO OutcomeTaxonomy (OutcomeName, Guidelines, GroupName, NotInclude) VALUES ('Substance use-related consequences', 'being under the influence of alcohol, drugs, or other substances at school or work; smoking on school grounds; alcohol-, drug-, or substance-impaired driving; alcohol- and drug-related motor vehicle accidents, blood poisoning, drug overdose, physical injuries and deaths due to alcohol- and drug-related accidents; composite measures looking at a variety of types of problems and negative consequences of substance use; riding in a car with an intoxicated/ impaired driver', 'Substance Use', 'substance-induced diseases, which should be coded as the more specific outcome')

GO