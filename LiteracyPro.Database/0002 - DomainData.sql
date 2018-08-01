--------------------------------------------------
--	Category
--------------------------------------------------
SET IDENTITY_INSERT dbo.Category ON
INSERT INTO	dbo.Category
(		CategoryID
	,	CategoryName
	,	CategoryDescription
	,	CSSClass
	,	DisplayOrderID
	,	IsAcive
)
SELECT	CategoryID				=	1
	,	CategoryName			=	'Food'
	,	CategoryDescription		=	NULL
	,	CSSClass				=	'Cat_Food'
	,	DisplayOrderID			=	10
	,	IsAcive					=	1
UNION ALL
SELECT	CategoryID				=	2
	,	CategoryName			=	'Entertainment'
	,	CategoryDescription		=	NULL
	,	CSSClass				=	'Cat_Entertainment'
	,	DisplayOrderID			=	20
	,	IsAcive					=	1
UNION ALL
SELECT	CategoryID				=	3
	,	CategoryName			=	'Home Expenses'
	,	CategoryDescription		=	NULL
	,	CSSClass				=	'Cat_Home'
	,	DisplayOrderID			=	30
	,	IsAcive					=	1
UNION ALL
SELECT	CategoryID				=	4
	,	CategoryName			=	'Medical'
	,	CategoryDescription		=	NULL
	,	CSSClass				=	'Cat_Medical'
	,	DisplayOrderID			=	40
	,	IsAcive					=	1
UNION ALL
SELECT	CategoryID				=	5
	,	CategoryName			=	'Travel'
	,	CategoryDescription		=	NULL
	,	CSSClass				=	'Cat_Travel'
	,	DisplayOrderID			=	50
	,	IsAcive					=	1
SET IDENTITY_INSERT dbo.Category OFF
go

--------------------------------------------------
--	Transaction
--------------------------------------------------

SET IDENTITY_INSERT dbo.[Transaction] ON
INSERT INTO dbo.[Transaction]
(		TransactionID
	,	TransactionDate
	,	CategoryID
	,	PayeeName
	,	TransactionAmount
	,	TransactionMemo
	,	CreateDate
	,	UpdateDate
)
SELECT	TransactionID		=	1
	,	TransactionDate		=	'7/27/2018'
	,	CategoryID			=	1
	,	PayeeName			=	'New Seasons Market'
	,	TransactionAmount	=	42.95
	,	TransactionMemo		=	'Weekly groceries'
	,	CreateDate			=	getdate()
	,	UpdateDate			=	getdate()
UNION ALL
SELECT	TransactionID		=	2
	,	TransactionDate		=	'7/29/2018'
	,	CategoryID			=	2
	,	PayeeName			=	'Regal Cinemas'
	,	TransactionAmount	=	82.95
	,	TransactionMemo		=	'Movie ticket and 1 popcorn'
	,	CreateDate			=	getdate()
	,	UpdateDate			=	getdate()
UNION ALL
SELECT	TransactionID		=	3
	,	TransactionDate		=	'7/29/2018'
	,	CategoryID			=	5
	,	PayeeName			=	'Delta Airlines'
	,	TransactionAmount	=	482.37
	,	TransactionMemo		=	'Vacation to Pittsburgh'
	,	CreateDate			=	getdate()
	,	UpdateDate			=	getdate()
