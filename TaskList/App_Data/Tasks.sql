CREATE TABLE [dbo].[Table] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Task]        NVARCHAR (MAX) NOT NULL,
    [IsCompleted] BIT            NOT NULL,
    [EntryDate]   DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

