# Usenet

A library for working with [Usenet](https://en.wikipedia.org/wiki/Usenet). 
It offers 
an [NNTP](https://en.wikipedia.org/wiki/Network_News_Transfer_Protocol) client, 
an [NZB](https://en.wikipedia.org/wiki/NZB) file parser, 
and a [yEnc](https://en.wikipedia.org/wiki/YEnc) decoder.

It is mainly focused on keeping memory usage low. Server responses can be enumerated as they come in. 
yEnc-encoded messages will be decoded streaming.

[![keimpema MyGet Build Status](https://www.myget.org/BuildSource/Badge/keimpema?identifier=5a545640-4681-43a6-8c40-3f7bec5f2006)](https://www.myget.org/)

## Getting Started ##
Install Nuget package:
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
    .AddGroup("alt.test.clienttest")
    .AddGroup("alt.test")
    .AddLine("This is a message with id " + messageId)
    .AddLine("with multiple lines")
    .Build();

// post
client.Post(newArticle);
```
Parse an Nzb file, download, decode and write the parts streaming to a file:
```csharp
NzbDocument nzbDocument = NzbParser.Parse(File.ReadAllText(nzbPath));

foreach (NzbSegment segment in file.Segments)
{
    // retrieve article form Usenet server
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
            stream.Seek(header.PartOffset, SeekOrigin.Begin);
            yencStream.CopyTo(stream);
        }
    }
}

```
Close connection:
```csharp
client.Quit();
```

