namespace PrintEditionsApp.Models
{
    public interface IStorable
    {
        void SaveToFile(string filePath);
    }
}
