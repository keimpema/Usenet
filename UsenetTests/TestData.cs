using System;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace UsenetTests
{
    public class TestData
    {
        private EmbeddedFileProvider fileProvider;

        public TestData Initialize(Type type)
        {
            Assembly assembly = type.GetTypeInfo().Assembly;
            fileProvider = new EmbeddedFileProvider(assembly, "UsenetTests.testdata");
            return this;
        }

        public IFileInfo GetEmbeddedFile(string fileName)
        {
            return fileProvider.GetFileInfo(fileName);
        }
    }
}
