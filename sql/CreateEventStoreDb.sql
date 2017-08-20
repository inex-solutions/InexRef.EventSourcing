USE [master]
GO

/****** Object:  Database [Rob.ValuationMonitoring]    Script Date: 20/08/2017 11:27:40 ******/
CREATE DATABASE [Rob.ValuationMonitoring]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Rob.ValuationMonitoring', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Rob.ValuationMonitoring.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Rob.ValuationMonitoring_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Rob.ValuationMonitoring_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET COMPATIBILITY_LEVEL = 120
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

ALTER DATABASE [Rob.ValuationMonitoring] SET RECOVERY FULL 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET  MULTI_USER 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Rob.ValuationMonitoring] SET  READ_WRITE 
GO

USE  [Rob.ValuationMonitoring]
CREATE TABLE [dbo].[ReadModel-ValuationLine](
	[UnauditedPrice] DECIMAL(19,4) NOT NULL,
	[Currency] NVARCHAR(3) NOT NULL,
	[PriceDateTime] DATETIME2 NOT NULL,
	[ValuationLineId] [nvarchar](64) NOT NULL,
	-- -------------------------------------------------
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[AggregateId] [nvarchar](64) NOT NULL,
	[CreateTime] [datetimeoffset](7) NOT NULL,
	[UpdatedTime] [datetimeoffset](7) NOT NULL,
	[LastAggregateSequenceNumber] [int] NOT NULL,
	CONSTRAINT [PK_ReadModel-ValuationLine] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
)

CREATE UNIQUE NONCLUSTERED INDEX [IX_ReadModel-ValuationLineAggregate_AggregateId] ON [dbo].[ReadModel-ValuationLine]
(
	[AggregateId] ASC
)
