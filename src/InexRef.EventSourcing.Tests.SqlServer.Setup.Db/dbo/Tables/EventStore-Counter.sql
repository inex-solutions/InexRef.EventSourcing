CREATE TABLE [dbo].[EventStore-Counter] (
    [AggregateId]   UNIQUEIDENTIFIER NOT NULL,
    [Version]       BIGINT           NOT NULL,
    [EventDateTime] DATETIME2 (7)    NOT NULL,
	[SourceCorrelationId] NVARCHAR(255) NOT NULL,
    [Payload]       NVARCHAR (MAX)   NOT NULL
);

