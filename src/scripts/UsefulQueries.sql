SELECT * FROM [Rob.EventStore].[dbo].[EventStore-Account] WITH (NOLOCK) ORDER BY [AggregateId] DESC, [Version]
SELECT * FROM [Rob.EventStore].[dbo].[NaturalKeyToAggregateIdMap-Account] WITH (NOLOCK)
/*
TRUNCATE TABLE [Rob.EventStore].[dbo].[EventStore-Account]
TRUNCATE TABLE [Rob.EventStore].[dbo].[NaturalKeyToAggregateIdMap-Account]
*/