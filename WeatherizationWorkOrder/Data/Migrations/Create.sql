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
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](255) NOT NULL,
	[Deleted] [bit] NOT NULL,
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[WO_USER] ADD  CONSTRAINT [DF_WO_USER_Deleted]  DEFAULT ((0)) FOR [Deleted]
GO

CREATE TABLE [dbo].[WORK_ORDER](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Consumer] [nvarchar](255) NULL,
	[PreparedBy] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[PreparedDate] [datetime] NULL,
	[WorkDate] [datetime] NULL,
	[LastModifiedBy] [nvarchar](255) NULL,
	[LastModified] [datetime] NULL,
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WO_MATERIAL](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[WorkOrderId] [int] NULL,
	[InventoryItemId] [int] NULL,
	[UnitsUsed] [decimal](18, 2) NULL,
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WO_LABOR](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[WorkOrderId] [int] NOT NULL,
	[Resource] [nvarchar](255) NULL,
	[Cost] [decimal](18, 2) NULL,
	[Hours] [decimal](18, 2) NULL,
) ON [PRIMARY]
GO