using Microsoft.Extensions.FileProviders;

namespace UsenetTests.TestHelpers
{
    public class TestData
    {
        private readonly EmbeddedFileProvider fileProvider = new(typeof(TestData).Assembly, "UsenetTests.testdata");

        public IFileInfo GetEmbeddedFile(string fileName)
        {
            return fileProvider.GetFileInfo(fileName);
        }
    }
}
