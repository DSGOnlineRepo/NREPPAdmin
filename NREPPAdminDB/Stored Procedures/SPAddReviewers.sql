CREATE PROCEDURE [dbo].[SPAddReviewer]
	@Id NVARCHAR(128),  
    @UserId NVARCHAR(128) , 
	@FirstName NVARCHAR(250) , 
    @LastName NVARCHAR(250) , 
    @Degree VARCHAR(35) , 
    @ReviewerType VARCHAR(35) ,        
    @HomeAddressLine1 VARCHAR(100) , 
    @HomeAddressLine2 VARCHAR(100) , 
	@HomeCity VARCHAR(30) ,
    @HomeState CHAR(2) , 
    @HomeZip VARCHAR(11) , 
	@PhoneNumber VARCHAR(15) , 
    @FaxNumber VARCHAR(15) , 
    @Email VARCHAR(100) , 
    @Employer VARCHAR(40) , 
    @Department VARCHAR(30) , 
    @WorkAddressLine1 VARCHAR(100) , 
	@WorkAddressLine2 VARCHAR(100) , 
    @WorkCity VARCHAR(30) ,
	@WorkState CHAR(2) ,
    @WorkZip VARCHAR(11) , 
    @WorkPhoneNumber VARCHAR(15) , 
    @WorkFaxNumber VARCHAR(15) , 
    @WorkEmail NCHAR(100) , 
    @ExperienceSummary VARCHAR(MAX) , 
	@IsActive BIT
AS

INSERT INTO [dbo].[Reviewers]
           ([Id]
           ,[UserId]
           ,[FirstName]
           ,[LastName]
           ,[Degree]
           ,[ReviewerType]
           ,[HomeAddressLine1]
           ,[HomeAddressLine2]
           ,[HomeCity]
           ,[HomeState]
           ,[HomeZip]
           ,[PhoneNumber]
           ,[FaxNumber]
           ,[Email]
           ,[Employer]
           ,[Department]
           ,[WorkAddressLine1]
           ,[WorkAddressLine2]
           ,[WorkCity]
           ,[WorkState]
           ,[WorkZip]
           ,[WorkPhoneNumber]
           ,[WorkFaxNumber]
           ,[WorkEmail]
           ,[ExperienceSummary]
           ,[IsActive])
     VALUES
           (NewId()
           ,@UserId
           ,@FirstName
           ,@LastName
           ,@Degree
           ,@ReviewerType
           ,@HomeAddressLine1
           ,@HomeAddressLine2
           ,@HomeCity
           ,@HomeState
           ,@HomeZip
           ,@PhoneNumber
           ,@FaxNumber
           ,@Email
           ,@Employer
           ,@Department
           ,@WorkAddressLine1
           ,@WorkAddressLine2
           ,@WorkCity
           ,@WorkState
           ,@WorkZip
           ,@WorkPhoneNumber
           ,@WorkFaxNumber
           ,@WorkEmail
           ,@ExperienceSummary
           ,@IsActive)

RETURN 0