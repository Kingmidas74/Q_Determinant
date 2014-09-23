namespace Converters
{
    public interface IConverter
    {
        void ParseDocument(string filePath);
        void ParseString(string originalString);
        void SaveToFile(string filePath);
        string GetAsString();
    }
}
