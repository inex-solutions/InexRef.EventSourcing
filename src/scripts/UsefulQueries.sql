SELECT * FROM [dbo].[EventStore-Account] WITH (NOLOCK) ORDER BY [AggregateId] DESC, [Version]
SELECT * FROM [dbo].[NaturalKeyToAggregateIdMap-Account] WITH (NOLOCK)
/*
TRUNCATE TABLE [dbo].[EventStore-Account]
TRUNCATE TABLE [dbo].[NaturalKeyToAggregateIdMap-Account]
*/