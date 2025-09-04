using System.IO;

namespace Toggle.Net.Providers.TextFile;

public class FileReader(string path) : IFileReader
{
	public string[] Content() => 
		File.ReadAllLines(path);
}