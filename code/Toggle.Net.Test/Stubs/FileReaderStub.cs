using Toggle.Net.Providers.TextFile;

namespace Toggle.Net.Test.Stubs;

public class FileReaderStub(string[] content) : IFileReader
{
	public string[] Content() => content;
}