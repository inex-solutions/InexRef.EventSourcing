CREATE TABLE [dbo].[NaturalKeyToAggregateIdMap-Account] (
    [NaturalKey]  NVARCHAR (50)    NOT NULL,
    [AggregateId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_NaturalKeyToAggregateIdMap-Account] PRIMARY KEY CLUSTERED ([NaturalKey] ASC)
);

