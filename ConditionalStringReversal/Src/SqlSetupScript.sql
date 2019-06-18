IF NOT EXISTS (SELECT * FROM sys.tables WHERE NAME = 'StringToReverse')
BEGIN
	CREATE TABLE dbo.StringToReverse
	(
		Id int identity NOT NULL,
		DataValue nvarchar(4000),
		CONSTRAINT PK_StringToRemove_ PRIMARY KEY CLUSTERED
		(
			Id ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	);
END

---loaders
INSERT INTO [MortgageConnectDB].dbo.[StringToReverse] (DataValue)
VALUES ('AB1;C2*D');

SELECT * FROM [MortgageConnectDB].dbo.[StringToReverse];

