CREATE TABLE	dbo.Category
(		CategoryID						int IDENTITY(1,1)			NOT NULL
	,	CategoryName					varchar(100)				NOT NULL
	,	CategoryDescription				varchar(max)				NULL
	,	CSSClass						varchar(100)				NULL
	,	DisplayOrderID					int							NOT NULL
	,	IsAcive							bit							NOT NULL
	,	CONSTRAINT	PK_Category										PRIMARY KEY(CategoryID)
	,	CONSTRAINT	AK_Category_CategoryName						UNIQUE(CategoryName)
)
go

select * From category

CREATE TABLE	dbo.[Transaction]
(		TransactionID					int IDENTITY(1,1)			NOT NULL
	,	TransactionDate					date						NOT NULL
	,	CategoryID						int							NOT NULL
	,	PayeeName						varchar(100)				NOT NULL
	,	TransactionAmount				money						NOT NULL
	,	TransactionMemo					varchar(max)				NULL
	,	CreateDate						datetime					NOT NULL
	,	UpdateDate						datetime					NOT NULL
	,	CONSTRAINT	PK_Transaction									PRIMARY KEY NONCLUSTERED(TransactionID)
	,	CONSTRAINT	AK_Transaction_TransactionDate_TransactionID	UNIQUE CLUSTERED(TransactionDate DESC, TransactionID DESC)
	,	CONSTRAINT	FK_Transaction_Category							FOREIGN KEY(CategoryID) REFERENCES dbo.Category
)
go

--Supporting FK_Transaction_Category
CREATE INDEX			IX_Transaction_CategoryID ON dbo.[Transaction](CategoryID)
go
