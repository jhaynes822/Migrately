USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceQuotes_Insert]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Josh Haynes>
-- Create date: <01-26-2023>
-- Description: Insurance Quotes Update
-- Code Reviewer: Naon Chun

-- MODIFIED BY: <Josh Haynes>
-- MODIFIED DATE: <01-31-2023>
-- Code Reviewer: Huan Le
-- Note:
-- =============================================

CREATE proc [dbo].[InsuranceQuotes_Insert]
					 
					
					@InsuranceId int
					
					,@CoverageStartDate datetime2
					,@CoverageEndDate datetime2	
					,@Citizenship int	
					,@Age int	
					,@MailingAddress int
					,@TravelDestination int
					,@PolicyRangeId int
					,@IsArrivedInUSA bit
					,@VisaTypeId int
					,@CreatedBy int
					,@ModifiedBy int
					,@Id int OUTPUT

/*------------------------------------------------

	Declare			@InsuranceId int = 1
					,@CoverageStartDate datetime2 = '2023-01-01'
					,@CoverageEndDate datetime2	= '2024-01-01'
					,@Citizenship int = 1
					,@Age int = 5
					,@MailingAddress int = 1
					,@TravelDestination int = 1
					,@PolicyRangeId int = 1
					,@IsArrivedInUSA bit = 1
					,@VisaTypeId int = 1
					,@CreatedBy int = 1
					,@ModifiedBy int = 1
					,@Id int = 0

	Execute [dbo].[InsuranceQuotes_Insert]
					 @InsuranceTypeId
					
					,@InsuranceId
					
					,@CoverageStartDate 
					,@CoverageEndDate 
					,@Citizenship 
					,@Age 
					,@MailingAddress 
					,@TravelDestination 
					,@PolicyRangeId 
					,@IsArrivedInUSA 
					,@VisaTypeId 
					,@CreatedBy 
					,@ModifiedBy
					,@Id OUTPUT

	Select *
	From [dbo].[InsuranceQuotes]
	

	Select*
	From dbo.Insurances

	Select*
	From dbo.InsuranceTypes

--------------------------------------------------*/

as

Begin

	Declare @DateTime datetime2(7) = GETUTCDATE();

	Declare @IsActive bit = 1;



	Insert Into [dbo].[InsuranceQuotes]
				([InsuranceId] 
				,[CoverageStartDate] 
				,[CoverageEndDate]  
				,[Citizenship] 	
				,[Age]
				,[MailingAddress]
				,[TravelDestination]
				,[PolicyRangeId]
				,[IsArrivedInUSA]  
				,[VisaTypeId] 
				,[DateCreated] 
				,[CreatedBy] 
				,[ModifiedBy]
				,[IsActive])

	Values		(@InsuranceId
				,@CoverageStartDate 
				,@CoverageEndDate
				,@Citizenship
				,@Age
				,@MailingAddress
				,@TravelDestination 
				,@PolicyRangeId 
				,@IsArrivedInUSA 
				,@VisaTypeId
				,@DateTime
				,@CreatedBy
				,@ModifiedBy 
				,@IsActive)

	Set @Id = SCOPE_IDENTITY()

End
GO
