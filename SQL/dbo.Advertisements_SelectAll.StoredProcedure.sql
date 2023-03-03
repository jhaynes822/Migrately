USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Advertisements_SelectAll]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Josh Haynes>
-- Create date: <02-15-2023>
-- Description:	Select All
-- Code Reviewer:

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer: 
-- Note: 
-- =============================================
CREATE proc [dbo].[Advertisements_SelectAll]


/*****************Test Code*********************


Execute [dbo].[Advertisements_SelectAll]



***********************************************/
As

Begin

SELECT		adv.Id
			,adv.AttorneyProfileId
			,adv.AdTierId
			,ap.Id
			,ap.PracticeName
			,l.Id
			,lt.Id
			,lt.[Name]
			,l.LineOne
			,l.LineTwo
			,l.City
			,l.Zip
			,l.StateId
			,s.Name
			,l.Latitude
			,l.Longitude
			,l.DateCreated
			,l.DateModified
			,l.CreatedBy
			,l.ModifiedBy
			,ap.Bio
			,ap.Phone
			,ap.Email
			,ap.Website
			,Languages = (
						  SELECT la.Id, la.Code, la.Name
						  FROM dbo.AttorneyLanguages as al 
							   inner join dbo.Languages as la
								  on al.LanguageId = la.Id
						  WHERE al.AttorneyProfileId = ap.Id
						  FOR JSON AUTO
							)
			,ap.DateCreated
			,ap.DateModified
			,adv.Title
			,adv.AdMainImage
			,adv.Details
			,adv.DateCreated
			,adv.DateModified
			,adv.DateStart
			,adv.DateEnd
  FROM [dbo].[Advertisements] as adv inner join [dbo].[AttorneyProfiles] as ap
		ON adv.AttorneyProfileId = ap.Id
	inner join [dbo].[Locations] as l
		ON ap.LocationId = l.Id
	inner join [dbo].[LocationTypes] as lt
		ON l.LocationTypeId = lt.Id
	inner join [dbo].[States] as s
		ON l.StateId = s.Id
	

End


GO
