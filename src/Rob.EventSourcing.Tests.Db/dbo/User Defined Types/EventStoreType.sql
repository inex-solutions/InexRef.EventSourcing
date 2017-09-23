CREATE TYPE [dbo].[EventStoreType] AS TABLE (
    [AggregateId]   UNIQUEIDENTIFIER NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventDateTime] DATETIME2 (7)    NOT NULL,
    [Payload]       NVARCHAR (MAX)   NOT NULL);

