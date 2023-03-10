USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceTypes_SelectAll]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: <Josh Haynes>
-- Create date: <01-26-2023>
-- Description: Insurance Quotes Update
-- Code Reviewer: Naon Chun

-- MODIFIED BY:
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE proc [dbo].[InsuranceTypes_SelectAll]

/*----------------------------------------------

	Execute [dbo].[InsuranceTypes_SelectAll]

----------------------------------------------*/

as

Begin

	Select   [Id]
			,[Name]
	From [dbo].[InsuranceTypes]

End
GO
