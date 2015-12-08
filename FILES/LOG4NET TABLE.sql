USE [dbc253a904eb3642c7964ba1ce00465ebb]
GO

/****** Object:  Table [dbo].[LOG4NET_LOG]    Script Date: 06/11/2013 04:01:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LOG4NET_LOG](
	[DATE] [date] NULL,
	[THREAD] [varchar](max) NULL,
	[LEVEL] [varchar](max) NULL,
	[LOGGER] [varchar](max) NULL,
	[MESSAGE] [varchar](max) NULL,
	[EXCEPTION] [varchar](max) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


