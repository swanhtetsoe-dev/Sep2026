USE [Sep2026Db]
GO
/****** Object:  Table [dbo].[Sales]    Script Date: 10/17/2026 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sales](
	[SaleId] [int] IDENTITY(1,1) NOT NULL,
	[WalletId] [int] NOT NULL,
	[ProductCode] [nvarchar](20) NOT NULL,
	[WalletName] [nvarchar](100) NOT NULL,
	[SoldQty] [int] NOT NULL,
	[TotalAmount] [decimal](18, 2) NOT NULL,
	[SaleDate] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SaleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wallets]    Script Date: 10/17/2026 8:16:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wallets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [nvarchar](20) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Brand] [nvarchar](100) NOT NULL,
	[Gender] [tinyint] NOT NULL,
	[Color] [nvarchar](50) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockQuantity] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[ProductCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sales] ADD  DEFAULT (getdate()) FOR [SaleDate]
GO
ALTER TABLE [dbo].[Wallets] ADD  DEFAULT ((0)) FOR [StockQuantity]
GO
ALTER TABLE [dbo].[Wallets] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Sales]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Wallets] FOREIGN KEY([WalletId])
REFERENCES [dbo].[Wallets] ([Id])
GO
ALTER TABLE [dbo].[Sales] CHECK CONSTRAINT [FK_Sales_Wallets]
GO
