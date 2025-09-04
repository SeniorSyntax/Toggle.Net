using Toggle.Net.Providers.TextFile;

namespace Toggle.Net.Tests.Stubs;

public class FileReaderStub(string[] content) : IFileReader
{
	public string[] Content()
	{
		return content;
	}
}