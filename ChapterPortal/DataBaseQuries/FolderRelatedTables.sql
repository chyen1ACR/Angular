CREATE TABLE Folders (

       FolderID [int] IDENTITY(1,1) NOT NULL,

       FolderName [nvarchar](250) NOT NULL,

       ParentID INT,

       [Level] SMALLINT NOT NULL,

       CreatedBy VARCHAR(50),

       CreatedDate datetime NOT NULL,

       ModifiedBy VARCHAR(50),

       [ModifiedDate] [datetime] NOT NULL,

CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED

(

       FolderID ASC

)

) ON [PRIMARY]

GO

CREATE TABLE Files (

       FileID [int] IDENTITY(1,1) NOT NULL,

       Name varchar(250) NOT NULL,

       FilePath VARCHAR(5000),

       FileType SMALLINT,

       FolderID INT NOT NULL,

       CreatedBy VARCHAR(50),

       CreatedDate datetime NOT NULL,

       ModifiedBy VARCHAR(50),

       [ModifiedDate] [datetime] NOT NULL,

CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED

(

       FileID ASC

),

CONSTRAINT [FK_Files_Folders] FOREIGN KEY (FolderID)

REFERENCES Folders(FolderID)

) ON [PRIMARY]

GO

CREATE TABLE ChildFolders (

       ID [int] IDENTITY(1,1) NOT NULL,

       FolderID INT NOT NULL,

       ChildFolderID INT NOT NULL,     

CONSTRAINT [PK_ChildFolders] PRIMARY KEY CLUSTERED

(

       ID ASC

),

CONSTRAINT [FK_ChildFolders_Folders] FOREIGN KEY (FolderID)

REFERENCES Folders(FolderID),

CONSTRAINT [FK_ChildFolders_ChildFolder] FOREIGN KEY (ChildFolderID)

REFERENCES Folders(FolderID)

) ON [PRIMARY]

GO