USE [Migrately]
GO
/****** Object:  Table [dbo].[Insurances]    Script Date: 2/27/2023 9:33:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Insurances](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[InsuranceTypeId] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Insurances] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Insurances] ADD  CONSTRAINT [DF_Insurances_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Insurances]  WITH CHECK ADD  CONSTRAINT [FK_Insurances_InsuranceTypes] FOREIGN KEY([InsuranceTypeId])
REFERENCES [dbo].[InsuranceTypes] ([Id])
GO
ALTER TABLE [dbo].[Insurances] CHECK CONSTRAINT [FK_Insurances_InsuranceTypes]
GO
