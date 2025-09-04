using Toggle.Net.Providers.TextFile;

namespace Toggle.Net.Tests.Stubs;

public class FileReaderStub : IFileReader
{
	private readonly string[] _content;

	public FileReaderStub(string[] content)
	{
		_content = content;
	}

	public string[] Content()
	{
		return _content;
	}
}