USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceQuotes_Delete_ById]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--==========================================================
-- Author: <Josh Haynes>
-- Create date: <01-25-2023>
-- Description: Update an InsuranceQuote record to Not Active
-- Code Reviewer:Naon Chun

-- MODIFIED BY: <Josh Haynes>
-- MODIFIED DATE: <01-31-2023>
-- Code Reviewer: Huan Le
-- Note:
--==========================================================


CREATE proc [dbo].[InsuranceQuotes_Delete_ById]
							 @Id int



/* --------------------------------------------------

	Declare @Id int = 0
	
	Execute dbo.InsuranceQuotes_Delete_ById 
							 @Id int
							

	Select *
	From [dbo].[InsuranceQuotes]
	Where Id = @Id

-----------------------------------------------------*/


as

Begin

	Declare @DateNow datetime2(7) = getutcdate();

	Update [dbo].[InsuranceQuotes]
	Set	   [IsActive] = 0
		  ,[DateModified] = @DateNow

	Where Id = @Id

End
GO
