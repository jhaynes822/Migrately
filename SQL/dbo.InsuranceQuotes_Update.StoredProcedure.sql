USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceQuotes_Update]    Script Date: 2/27/2023 9:33:33 AM ******/
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

CREATE proc [dbo].[InsuranceQuotes_Update]
					 @Id int
					,@InsuranceId int
					,@CoverageStartDate datetime2
					,@CoverageEndDate datetime2	
					,@Citizenship int	
					,@Age int	
					,@MailingAddress int
					,@TravelDestination int
					,@PolicyRangeId int
					,@IsArrivedInUSA bit
					,@VisaTypeId int
					,@ModifiedBy int
					,@IsActive bit

/*------------------------------------------------
	Declare			@Id int
					,@InsuranceId int
					,@CoverageStartDate datetime2
					,@CoverageEndDate datetime2	
					,@Citizenship int	
					,@Age int	
					,@MailingAddress int
					,@TravelDestination int
					,@PolicyRangeId int
					,@IsArrivedInUSA bit
					,@VisaType int
					,@ModifiedBy int
					,@IsActive bit

	Execute [dbo].[InsuranceQuotes_Update]

	Select *
	From [dbo].[InsuranceQuotes]
	Where [Id] = @Id

--------------------------------------------------*/

as

Begin

	

	Update [dbo].[InsuranceQuotes]
		SET 	 [InsuranceId] = @InsuranceId
				,[CoverageStartDate] = @CoverageStartDate 
				,[CoverageEndDate] = @CoverageEndDate 
				,[Citizenship] = @Citizenship	
				,[Age] = @Age
				,[MailingAddress] = @MailingAddress
				,[TravelDestination] = @TravelDestination 
				,[PolicyRangeId] = @PolicyRangeId 
				,[IsArrivedInUSA]  = @IsArrivedInUSA 
				,[VisaTypeId] = @VisaTypeId
				,[DateModified] = GETUTCDATE()
				,[ModifiedBy] = @ModifiedBy
				,[IsActive] = @IsActive 
	Where [Id] = @Id

End
GO
