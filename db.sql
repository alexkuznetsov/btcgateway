USE [btcgatewayapi]
GO
/****** Object:  Table [dbo].[clients]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[clients](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NULL,
	[client_id] [nvarchar](256) NOT NULL,
	[passwhash] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_clients] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[hot_wallets]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hot_wallets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime] NOT NULL,
	[updated_at] [datetime] NULL,
	[address] [nvarchar](250) NOT NULL,
	[amount] [decimal](18, 10) NOT NULL,
	[rpc_address] [nvarchar](256) NULL,
	[rpc_username] [nvarchar](256) NULL,
	[rpc_password] [nvarchar](32) NULL,
	[passphrase] [nvarchar](256) NULL,
	[tx_count] [int] NOT NULL,
 CONSTRAINT [PK_hot_wallets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[income_tx]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[income_tx](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NULL,
	[wallet_id] [int] NOT NULL,
	[tx_hash] [ntext] NOT NULL,
	[address] [nvarchar](256) NOT NULL,
	[amount] [decimal](18, 10) NOT NULL,
	[confirmation] [int] NOT NULL,
	[view_cnt] [int] NOT NULL,
	[tx_id] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_income_tx] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[income_tx_hist]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[income_tx_hist](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime] NULL,
	[income_tx_id] [int] NOT NULL,
 CONSTRAINT [PK_income_tx_hist] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[outcome_tx]    Script Date: 10.04.2019 23:35:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[outcome_tx](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[updated_at] [datetime2](7) NULL,
	[wallet_id] [int] NOT NULL,
	[tx_hash] [ntext] NOT NULL,
	[address] [nvarchar](256) NOT NULL,
	[amount] [decimal](18, 10) NOT NULL,
	[state] [nvarchar](12) NOT NULL,
	[fee] [decimal](18, 10) NOT NULL,
	[confirmation] [int] NOT NULL,
	[tx_id] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_outcome_tx] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_income_tx]    Script Date: 10.04.2019 23:35:14 ******/
CREATE NONCLUSTERED INDEX [IX_income_tx] ON [dbo].[income_tx]
(
	[tx_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_outcome_tx]    Script Date: 10.04.2019 23:35:14 ******/
CREATE NONCLUSTERED INDEX [IX_outcome_tx] ON [dbo].[outcome_tx]
(
	[tx_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[clients] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[hot_wallets] ADD  CONSTRAINT [DF_hot_wallets_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[hot_wallets] ADD  DEFAULT ((0)) FOR [tx_count]
GO
ALTER TABLE [dbo].[income_tx] ADD  CONSTRAINT [DF__income_tx__creat__52593CB8]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[income_tx] ADD  CONSTRAINT [DF__income_tx__confi__534D60F1]  DEFAULT ((0)) FOR [confirmation]
GO
ALTER TABLE [dbo].[income_tx] ADD  CONSTRAINT [DF__income_tx__view___5FB337D6]  DEFAULT ((0)) FOR [view_cnt]
GO
ALTER TABLE [dbo].[income_tx_hist] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[income_tx_hist] ADD  DEFAULT (NULL) FOR [updated_at]
GO
ALTER TABLE [dbo].[outcome_tx] ADD  CONSTRAINT [DF_outcome_tx_created_at]  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[outcome_tx] ADD  CONSTRAINT [DF__outcome_t__confi__5AEE82B9]  DEFAULT ((0)) FOR [confirmation]
GO
ALTER TABLE [dbo].[income_tx]  WITH CHECK ADD  CONSTRAINT [FK_income_tx_hot_wallets] FOREIGN KEY([wallet_id])
REFERENCES [dbo].[hot_wallets] ([id])
GO
ALTER TABLE [dbo].[income_tx] CHECK CONSTRAINT [FK_income_tx_hot_wallets]
GO
ALTER TABLE [dbo].[income_tx_hist]  WITH CHECK ADD  CONSTRAINT [FK_income_tx_hist_income_tx] FOREIGN KEY([income_tx_id])
REFERENCES [dbo].[income_tx] ([id])
GO
ALTER TABLE [dbo].[income_tx_hist] CHECK CONSTRAINT [FK_income_tx_hist_income_tx]
GO
ALTER TABLE [dbo].[outcome_tx]  WITH CHECK ADD  CONSTRAINT [FK_outcome_tx_hot_wallets] FOREIGN KEY([wallet_id])
REFERENCES [dbo].[hot_wallets] ([id])
GO
ALTER TABLE [dbo].[outcome_tx] CHECK CONSTRAINT [FK_outcome_tx_hot_wallets]
GO

--- Insert client for authentication
--- rex / 1qaz!QAZ

SET IDENTITY_INSERT [dbo].[clients] ON 
GO
INSERT [dbo].[clients] ([id], [created_at], [updated_at], [client_id], [passwhash]) VALUES (1, CAST(N'2019-04-08T18:31:18.7866667' AS DateTime2), NULL, N'rex', N'ACAtzWwMbSeKmzfKNcL5SNAy09YbZbEJ4oelQWY+x/O9ai8czW0Dx79HplyLTKGr0A==')
GO
SET IDENTITY_INSERT [dbo].[clients] OFF
GO
