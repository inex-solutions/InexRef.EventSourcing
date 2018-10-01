CREATE TABLE [dbo].[NaturalKeyToAggregateIdMap-Counter] (
    [NaturalKey]  NVARCHAR (50)    NOT NULL,
    [AggregateId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_NaturalKeyToAggregateIdMap-Counter] PRIMARY KEY CLUSTERED ([NaturalKey] ASC)
);

