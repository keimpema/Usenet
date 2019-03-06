# Usenet

A library for working with [Usenet](https://en.wikipedia.org/wiki/Usenet). It offers:
* an [NNTP](https://en.wikipedia.org/wiki/Network_News_Transfer_Protocol) client
* an [NZB](https://en.wikipedia.org/wiki/NZB) file parser, builder and writer
* a [yEnc](https://en.wikipedia.org/wiki/YEnc) encoder and decoder

It is mainly focused on keeping memory usage low. Server responses can be enumerated as they come in. 
Binary messages will be encoded to yEnc format streaming and yEnc-encoded data will be decoded to binary data streaming.

The NNTP client is compliant with [RFC 2980](https://tools.ietf.org/html/rfc2980), [RFC 3977](https://tools.ietf.org/html/rfc3977), [RFC 4643](https://tools.ietf.org/html/rfc4643) and [RFC 6048](https://tools.ietf.org/html/rfc6048).

[![Build status](https://ci.appveyor.com/api/projects/status/5kp5xbcr03xwpb6t?svg=true)](https://ci.appveyor.com/project/keimpema/usenet)

## Getting Started ##
Install [Nuget](https://www.nuget.org/packages/Usenet) package:
```
PM> Install-Package Usenet
```

## Examples ##
Connect to Usenet server:
```csharp
var client = new NntpClient(new NntpConnection());
await client.ConnectAsync(hostname, port, useSsl);
```
Authenticate:
```csharp
client.Authenticate(username, password)
```
Retrieve article:
```csharp
NntpArticleResponse response = client.Article(messageId);
if (response.Success) {
    foreach (string line in response.Article.Body) {
        ...
    }
}
```
Build an article and post to server:
```csharp
string messageId = $"{Guid.NewGuid()}@example.net";

NntpArticle newArticle = new NntpArticleBuilder()
    .SetMessageId(messageId)
    .SetFrom("Randomposter <randomposter@example.net>")
    .SetSubject("Random test post #1")
    .AddGroups("alt.test.clienttest", "alt.test")
    .AddLine("This is a message with id " + messageId)
    .AddLine("with multiple lines")
    .Build();

client.Post(newArticle);
```
Parse an NZB file, download, decode and write the parts streaming to a file:
```csharp
NzbDocument nzbDocument = NzbParser.Parse(File.ReadAllText(nzbPath));

foreach (NzbFile file in nzbDocument.Files)
{
    foreach (NzbSegment segment in file.Segments)
    {
        // retrieve article from Usenet server
        NntpArticleResponse response = client.Article(segment.MessageId);

        // decode the yEnc-encoded article
        using (YencStream yencStream = YencStreamDecoder.Decode(response.Article.Body))
        {
            YencHeader header = yencStream.Header;

            if (!File.Exists(header.FileName))
            {
                // create file and pre-allocate disk space for it
                using (FileStream stream = File.Create(header.FileName))
                {
                    stream.SetLength(header.FileSize);
                }
            }
            using (FileStream stream = File.Open(
                header.FileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                // copy incoming parts to file
                stream.Position = header.PartOffset;
                yencStream.CopyTo(stream);
            }
        }
    }
}
```
Build an NZB document and write to file:
```csharp
IFileProvider fileProvider = new PhysicalFileProvider(Path.GetFullPath("testdata"));

NzbBuilder builder = new NzbBuilder()
    .AddGroups("alt.test.clienttest")
    .SetMessageBase("random.local")
    .SetPartSize(50_000)
    .SetPoster("random poster <random.poster@random.com>")
    .AddMetaData("title", "Testing upload Pictures.rar");

foreach (string fileName in fileNames)
{
    builder.AddFile(fileProvider.GetFileInfo(fileName));
}

NzbDocument nzbDocument = builder.Build();

using (FileStream file = File.Open("Pictures.nzb"))
using (var writer = new StreamWriter(file, UsenetEncoding.Default))
{
    await writer.WriteNzbDocumentAsync(nzbDocument);
}

```
Encode the files from an NZB document in yEnc format and upload streaming:
```csharp
foreach (NzbFile file in nzbDocument.Files)
{
    // open file stream
    IFileInfo fileInfo = fileProvider.GetFileInfo(file.FileName);
    using (Stream stream = fileInfo.CreateReadStream())
    {
        foreach (NzbSegment segment in file.Segments)
        {
            stream.Position = segment.Offset;

            // encode in yEnc format
            IEnumerable<string> encodedBody = YencEncoder.Encode(new YencHeader(
                file.FileName, file.Size, 128, segment.Number, file.Segments.Count,
                segment.Size, segment.Offset), stream);

            // create article
            NntpArticle article = new NntpArticleBuilder()
                .AddGroups(file.Groups)
                .SetMessageId(segment.MessageId)
                .SetFrom(file.Poster)
                .SetSubject(segment.MessageId)
                .SetBody(encodedBody)
                .Build();

            // post article
            client.Post(article);
        }
    }
}
```
Close connection:
```csharp
client.Quit();
```

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

This project was heavily inspired by [Kristian Hellang](https://github.com/khellang)'s work:
* [khellang/yEnc](https://github.com/khellang/yEnc)
* [khellang/nntplib.net](https://github.com/khellang/nntplib.net)
* [khellang/Nzb](https://github.com/khellang/Nzb)