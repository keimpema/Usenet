using System.IO;
using System.Reflection;

namespace TestClient.Util
{
    public class TestData
    {
        public string Directory { get; set; }

        public TestData()
        {
            Directory = GetTestDataDirectory();
            if (Directory == null)
            {
                throw new DirectoryNotFoundException("Missing testdata directory");
            }
        }

        private static string GetTestDataDirectory()
        {
            string workingDirectory = typeof(TestData).GetTypeInfo().Assembly.Location;
            while (workingDirectory != null)
            {
                string testDataDirectory = Path.Combine(workingDirectory, "testdata");
                if (System.IO.Directory.Exists(testDataDirectory))
                {
                    return testDataDirectory;
                }
                workingDirectory = Path.GetDirectoryName(workingDirectory);
            }
            return null;
        }
    }
}
