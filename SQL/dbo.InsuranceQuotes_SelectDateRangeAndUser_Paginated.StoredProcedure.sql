USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceQuotes_SelectDateRangeAndUser_Paginated]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--==========================================================
-- Author: <Josh Haynes>
-- Create date: <02-8-2023>
-- Description: SelectByEmail for InsuranceQuotes
-- Code Reviewer: Huan Le

-- MODIFIED BY: <Josh Haynes>
-- MODIFIED DATE: <02-16-2023>
-- Code Reviewer: "Keith Jackson"
-- Note: Removed "Changed Email to user query."
--==========================================================

CREATE proc [dbo].[InsuranceQuotes_SelectDateRangeAndUser_Paginated]
								 @DateRangeStart datetime2(7)
								,@DateRangeEnd datetime2(7)
								,@User nvarchar(255)
								,@PageIndex int
								,@PageSize int
								
/*-----------------------------------------------------------

	Declare @DateRangeStart datetime2(7) = '2023-01-30'
			,@DateRangeEnd datetime2(7) = '2023-02-20'
			,@User nvarchar(255) = 'Pham'
			,@PageIndex int = 0
			,@PageSize int = 5

	Execute [dbo].[InsuranceQuotes_SelectDateRangeAndUser_Paginated]
								 @DateRangeStart
								,@DateRangeEnd
								,@User
								,@PageIndex 
								,@PageSize
								

	Select *
	From [dbo].[InsuranceQuotes]
	

-----------------------------------------------------------*/

As


Begin

	Declare @Offset int = @PageIndex * @PageSize

	Select						      iq.[Id]
									,it.[Id] as InsuranceTypeId
									,it.[Name] as InsuranceTypeName
									,i.Id AS InsuranceId
									,i.[Name] AS InsuranceName
									,iq.[CoverageStartDate]
									,iq.[CoverageEndDate]
									,c.[Code] as CitizenshipCode
									,c.[Name] as CitizenshipName
									,c.[Flag] as CitizenshipFlag
									,iq.[Age]
									,sm.[Code] as MailingAddressCode
									,sm.[Name] as MailingAddressName
									,st.[Code] as TravelDestinationCode
									,st.[Name] as TravelDestinationName
									,pr.[Id] as PolicyRangeId
									,pr.[Name] as PolicyRange
									,iq.[IsArrivedInUSA]
									,vt.[Id] as VisaTypeId
									,vt.[Name] as VisaTypeName
									,iq.[DateCreated]
									,iq.[DateModified]
									,uc.[Id] as CreatedById
									,uc.[FirstName] as CreatedByFirstName
									,uc.[Mi] as CreatedByMiddleInitial
									,uc.[LastName] as CreatedByLastName
									,uc.[AvatarUrl] as CreatedByAvatarUrl
									,uc.[Email] as CreatedByEmail
									,um.Id AS ModifiedByUserId
									,um.FirstName as ModifiedByFirstName
									,um.Mi AS ModifiedByMiddleInitial
									,um.LastName AS ModifiedByLastName
									,um.AvatarUrl AS ModifiedByAvatarUrl
									,um.Email AS ModifiedByClientEmail
									,TotalCount = COUNT(1) OVER()

	From [dbo].[InsuranceQuotes] As iq 
									INNER JOIN dbo.Insurances as i ON i.Id = iq.InsuranceId
									INNER JOIN  [dbo].[InsuranceTypes] as it
											on i.InsuranceTypeId = it.Id
									   inner join [dbo].[Countries] as c
											on iq.Citizenship = c.Id
									   inner join [dbo].[States] as sm
											on iq.MailingAddress = sm.Id
									   inner join [dbo].[States] as st
											on iq.TravelDestination = st.Id
									   inner join [dbo].[PolicyRange] as pr
											on iq.PolicyRangeId = pr.Id
									   inner join [dbo].[VisaTypes] as vt
											on iq.VisaTypeId = vt.Id
									   inner join [dbo].[Users] as uc
											on iq.CreatedBy = uc.Id
									   inner join [dbo].[Users] as um
											on iq.ModifiedBy = um.Id
	Where (iq.DateCreated Between @DateRangeStart and @DateRangeEnd) 
		and (uc.FirstName LIKE '%' + @User + '%' Or uc.LastName LIKE '%' + @User + '%' Or uc.Email LIKE '%' + @User + '%') 
	Order By uc.Id
	Offset @Offset Rows
	Fetch Next @PageSize Rows ONLY



End
GO
