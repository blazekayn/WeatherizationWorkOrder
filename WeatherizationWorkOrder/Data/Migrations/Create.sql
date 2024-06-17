CREATE TABLE [dbo].[INVENTORY_ITEM](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Description] [nvarchar](255) NOT NULL,
	[Cost] [decimal](18, 2) NOT NULL,
	[Units] [nvarchar](50) NOT NULL,
	[StartingAmount] [decimal](18, 2) NOT NULL,
	[Remaining] [decimal](18, 2) NULL,
	[PurchaseDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[LastModifiedBy] [nvarchar](255) NOT NULL
) ON [PRIMARY]
GO

------------------------------------------------

CREATE TABLE [dbo].[WO_USER](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_WO_USER] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WO_USER] ADD  CONSTRAINT [DF_WO_USER_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO
