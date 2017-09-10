USE [master]
GO
/****** Object:  Database [Rob.EventStore]    Script Date: 10/09/2017 21:27:21 ******/
CREATE DATABASE [Rob.EventStore]
GO
ALTER DATABASE [Rob.EventStore] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Rob.EventStore].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Rob.EventStore] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Rob.EventStore] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Rob.EventStore] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Rob.EventStore] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Rob.EventStore] SET ARITHABORT OFF 
GO
ALTER DATABASE [Rob.EventStore] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Rob.EventStore] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Rob.EventStore] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Rob.EventStore] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Rob.EventStore] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Rob.EventStore] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Rob.EventStore] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Rob.EventStore] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Rob.EventStore] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Rob.EventStore] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Rob.EventStore] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Rob.EventStore] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Rob.EventStore] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Rob.EventStore] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Rob.EventStore] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Rob.EventStore] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Rob.EventStore] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Rob.EventStore] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Rob.EventStore] SET  MULTI_USER 
GO
ALTER DATABASE [Rob.EventStore] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Rob.EventStore] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Rob.EventStore] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Rob.EventStore] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Rob.EventStore', N'ON'
GO
USE [Rob.EventStore]
GO
/****** Object:  UserDefinedTableType [dbo].[EventStoreType]    Script Date: 10/09/2017 21:27:21 ******/
CREATE TYPE [dbo].[EventStoreType] AS TABLE(
	[AggregateId] [nvarchar](64) NOT NULL,
	[Version] [bigint] NOT NULL,
	[EventDateTime] [datetime2](7) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL
)
GO
/****** Object:  Table [dbo].[EventStore-Account]    Script Date: 10/09/2017 21:27:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventStore-Account](
	[AggregateId] [nvarchar](64) NOT NULL,
	[Version] [bigint] NOT NULL,
	[EventDateTime] [datetime2](7) NOT NULL,
	[Payload] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[usp_InsertAccountEvents]    Script Date: 10/09/2017 21:27:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_InsertAccountEvents]
	@aggregateId [nvarchar](64),
    @eventsToInsert EventStoreType READONLY,
	@expectedVersion BIGINT

AS   

BEGIN TRAN

DECLARE @currentLatestVersion BIGINT

SELECT @currentLatestVersion = ISNULL(MAX([Version]),0) FROM [dbo].[EventStore-Account] WHERE [AggregateId] = @aggregateId 

IF (@expectedVersion = @currentLatestVersion)
BEGIN
	INSERT INTO [dbo].[EventStore-Account] ([AggregateId], [Version], [EventDateTime], [Payload])  
	SELECT * FROM @eventsToInsert
END
ELSE
BEGIN
	DECLARE @errorMessage NVARCHAR(255)
	SELECT @errorMessage = FORMATMESSAGE('Concurrency error saving aggregate %s (expected version %I64d, actual %I64d)', @aggregateId,  @expectedVersion, @currentLatestVersion);
	THROW 51000, @errorMessage, 1
	ROLLBACK TRAN
END

COMMIT TRAN
GO
USE [master]
GO
ALTER DATABASE [Rob.EventStore] SET  READ_WRITE 
GO
