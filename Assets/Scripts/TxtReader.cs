using System.IO;

public class TxtReader {
    public string ReadFromFile(string path) {
        string text = File.ReadAllText(path);
        return text;
    }
}
