using FolderComparer.Business.Data;

namespace FolderComparer.Business.Interfaces
{
    public interface IFileDataCreator
    {
        FileData MakeFileDataFromLocalPath(string path);
    }
}