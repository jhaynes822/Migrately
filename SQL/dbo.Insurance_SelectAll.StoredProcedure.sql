USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Insurance_SelectAll]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create   proc [dbo].[Insurance_SelectAll]

/*------------------------------------------

	Execute [dbo].[Insurance_SelectAll]

------------------------------------------*/

as

Begin

	Select   i.[Id]
			,it.[Id] as InsuranceTypeId
			,it.[Name] as InsuranceTypeName
			,i.[Name]
			,i.[DateCreated]

	From [dbo].[Insurances] as i inner join [dbo].[InsuranceTypes] as it
				on it.Id = i.InsuranceTypeId

End
GO
