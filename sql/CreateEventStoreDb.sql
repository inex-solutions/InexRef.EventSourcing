/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2012
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [master]
GO
/****** Object:  Database [Rob.ValuationMonitoring]    Script Date: 28/08/2017 21:49:02 ******/
CREATE DATABASE [Rob.ValuationMonitoring]

GO
ALTER DATABASE [Rob.ValuationMonitoring] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Rob.ValuationMonitoring].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ARITHABORT OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET  MULTI_USER 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Rob.ValuationMonitoring', N'ON'
GO
USE [Rob.ValuationMonitoring]
GO
/****** Object:  UserDefinedTableType [dbo].[eventdatamodel_list_type]    Script Date: 28/08/2017 21:49:02 ******/
CREATE TYPE [dbo].[eventdatamodel_list_type] AS TABLE(
	[AggregateId] [nvarchar](255) NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[AggregateSequenceNumber] [int] NOT NULL,
	[BatchId] [uniqueidentifier] NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Metadata] [nvarchar](max) NOT NULL
)
GO
/****** Object:  Table [dbo].[EventFlow]    Script Date: 28/08/2017 21:49:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventFlow](
	[GlobalSequenceNumber] [bigint] IDENTITY(1,1) NOT NULL,
	[BatchId] [uniqueidentifier] NOT NULL,
	[AggregateId] [nvarchar](255) NOT NULL,
	[AggregateName] [nvarchar](255) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Metadata] [nvarchar](max) NOT NULL,
	[AggregateSequenceNumber] [int] NOT NULL,
 CONSTRAINT [PK_EventFlow] PRIMARY KEY CLUSTERED 
(
	[GlobalSequenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EventFlowSnapshots]    Script Date: 28/08/2017 21:49:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventFlowSnapshots](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AggregateId] [nvarchar](128) NOT NULL,
	[AggregateName] [nvarchar](128) NOT NULL,
	[AggregateSequenceNumber] [int] NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Metadata] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_EventFlowSnapshots] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReadModel-LatestUnauditedPrice]    Script Date: 28/08/2017 21:49:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReadModel-LatestUnauditedPrice](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ValuationLineId] [nvarchar](64) NOT NULL,
	[ValuationLineName] [nvarchar](254) NOT NULL,
	[ValuationLineNameEffectiveDateTime] [datetime2](7) NOT NULL,
	[UnauditedPrice] [decimal](19, 4) NOT NULL,
	[Currency] [nvarchar](3) NOT NULL,
	[EffectiveDateTime] [datetime2](7) NOT NULL,
	[CreateTime] [datetimeoffset](7) NOT NULL,
	[UpdatedTime] [datetimeoffset](7) NOT NULL,
	[SequenceNumber] [int] NOT NULL,
 CONSTRAINT [PK_ReadModel-LatestUnauditedPrice] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemaVersions]    Script Date: 28/08/2017 21:49:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemaVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScriptName] [nvarchar](255) NOT NULL,
	[Applied] [datetime] NOT NULL,
 CONSTRAINT [PK_SchemaVersions_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[SchemaVersions] ON 
GO
INSERT [dbo].[SchemaVersions] ([Id], [ScriptName], [Applied]) VALUES (1, N'EventStores.Scripts.0001 - Create table EventFlow.sql', CAST(N'2017-08-20T17:39:16.167' AS DateTime))
GO
INSERT [dbo].[SchemaVersions] ([Id], [ScriptName], [Applied]) VALUES (2, N'EventStores.Scripts.0002 - Create eventdatamodel_list_type.sql', CAST(N'2017-08-20T17:39:16.237' AS DateTime))
GO
INSERT [dbo].[SchemaVersions] ([Id], [ScriptName], [Applied]) VALUES (3, N'SnapshotStores.Scripts.0001 - Create EventFlowSnapshots.sql', CAST(N'2017-08-28T18:59:12.417' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[SchemaVersions] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_EventFlowSnapshots_AggregateId_AggregateSequenceNumber]    Script Date: 28/08/2017 21:49:02 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_EventFlowSnapshots_AggregateId_AggregateSequenceNumber] ON [dbo].[EventFlowSnapshots]
(
	[AggregateName] ASC,
	[AggregateId] ASC,
	[AggregateSequenceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [Rob.ValuationMonitoring] SET  READ_WRITE 
GO
