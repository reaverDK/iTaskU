CREATE TABLE [dbo].[Tasks] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Task]        NVARCHAR (300) NOT NULL,
    [IsCompleted] BIT            NOT NULL,
    [EntryDate]   DATETIME       NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [IsBegun]     BIT            NOT NULL,
    [Author] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Table] PRIMARY KEY CLUSTERED ([Id] ASC)
);

