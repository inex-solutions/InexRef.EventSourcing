
CREATE PROCEDURE [dbo].[usp_InsertAccountEvents]
	@aggregateId [uniqueidentifier],
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
	SELECT @errorMessage = FORMATMESSAGE('Concurrency error saving aggregate %s (expected version %I64d, actual %I64d)', convert(nvarchar(36), @aggregateId), @expectedVersion, @currentLatestVersion);
	THROW 51000, @errorMessage, 1
	ROLLBACK TRAN
END

COMMIT TRAN

