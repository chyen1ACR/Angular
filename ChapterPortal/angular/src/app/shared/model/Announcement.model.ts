export class Announcementmodel
{
    public Text :string;
    public PlainText :string
    public ChpaterId:Array<ChapterLookUp>;
    public CreatedBy : number
    public StartDate :string
    public EndDate :string
    public ModifiedBy :number
    public ModifiedDate :string
}

export class ChapterLookUp{
  public  ChapterId:string;
  public  Name :string;
}

export class ChapterOfficer{
FullName:string;
PositionDescription: string;
PositionBeginDate: string;
PositionEndDate:string;
VotingStatus:string;
PrimaryEmail:string;
PrimaryPhone:string;
AdditionalComments:string;
CustomerId: string;
}

export class FileModelDto

    {

        public FileID:number

        public Name:string

        public FilePath:string

        public FolderID:number

    }



    export class FolderModelDto

    {

        public folderID:number

        public folderName:string

        public parentID:number

        public level:number

        public  files:Array<FileModelDto>
        public  childFolders:Array<FolderModelDto>

    }