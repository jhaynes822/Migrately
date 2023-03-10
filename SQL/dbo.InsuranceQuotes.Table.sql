USE [Migrately]
GO
/****** Object:  Table [dbo].[InsuranceQuotes]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InsuranceQuotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceId] [int] NOT NULL,
	[CoverageStartDate] [datetime2](7) NOT NULL,
	[CoverageEndDate] [datetime2](7) NOT NULL,
	[Citizenship] [int] NULL,
	[Age] [int] NOT NULL,
	[MailingAddress] [int] NULL,
	[TravelDestination] [int] NULL,
	[PolicyRangeId] [int] NULL,
	[IsArrivedInUSA] [bit] NULL,
	[VisaTypeId] [int] NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[DateModified] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_InsuranceQuotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[InsuranceQuotes] ADD  CONSTRAINT [DF_InsuranceQuotes_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[InsuranceQuotes] ADD  CONSTRAINT [DF_InsuranceQuotes_DateModified]  DEFAULT (getutcdate()) FOR [DateModified]
GO
ALTER TABLE [dbo].[InsuranceQuotes] ADD  CONSTRAINT [DF_InsuranceQuotes_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[InsuranceQuotes]  WITH CHECK ADD  CONSTRAINT [FK_InsuranceQuotes_Countries] FOREIGN KEY([Citizenship])
REFERENCES [dbo].[Countries] ([Id])
GO
ALTER TABLE [dbo].[InsuranceQuotes] CHECK CONSTRAINT [FK_InsuranceQuotes_Countries]
GO
ALTER TABLE [dbo].[InsuranceQuotes]  WITH CHECK ADD  CONSTRAINT [FK_InsuranceQuotes_Insurances] FOREIGN KEY([InsuranceId])
REFERENCES [dbo].[Insurances] ([Id])
GO
ALTER TABLE [dbo].[InsuranceQuotes] CHECK CONSTRAINT [FK_InsuranceQuotes_Insurances]
GO
ALTER TABLE [dbo].[InsuranceQuotes]  WITH CHECK ADD  CONSTRAINT [FK_InsuranceQuotes_PolicyRange] FOREIGN KEY([PolicyRangeId])
REFERENCES [dbo].[PolicyRange] ([Id])
GO
ALTER TABLE [dbo].[InsuranceQuotes] CHECK CONSTRAINT [FK_InsuranceQuotes_PolicyRange]
GO
ALTER TABLE [dbo].[InsuranceQuotes]  WITH CHECK ADD  CONSTRAINT [FK_InsuranceQuotes_VisaTypes] FOREIGN KEY([VisaTypeId])
REFERENCES [dbo].[VisaTypes] ([Id])
GO
ALTER TABLE [dbo].[InsuranceQuotes] CHECK CONSTRAINT [FK_InsuranceQuotes_VisaTypes]
GO
