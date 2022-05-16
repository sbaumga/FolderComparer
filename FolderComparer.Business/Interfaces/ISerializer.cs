namespace FolderComparer.Business.Interfaces
{
    public interface ISerializer
    {
        T Deserialize<T>(string data);

        string Serialize<T>(T data);
    }
}