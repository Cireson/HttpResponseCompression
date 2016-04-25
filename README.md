# HttpResponseCompression

[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg?maxAge=2592000)](v0.0.4)
[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg?maxAge=2592000)](v0.0.2)

## Credits
First off the Web API implementation is copied, and tweaked, from  [fabrik](https://github.com/Cireson/HttpResponseCompression/blob/master/LICENSE.md) and [Ben Foster](https://github.com/benfoster)

## Intent
We wanted a smaller library that brought in only the code needed to meet the requirement. In this case we are focusing on HTTP response compression only.

## OWIN

The OWIN implementation will compress the static files easily by configuring your app.

```
app.UseResponseCompressingMiddleware()
```

## Web API

The Web API implementation will compress all responses by configuring Web API's Message Handlers. The first handler is executed last.

```
config.MessageHandlers.Insert(0, new CompressionHandler());
```