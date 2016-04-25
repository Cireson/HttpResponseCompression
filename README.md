# HttpResponseCompression

## Nuget Packages
### Cireson.HttpResponseCompression.Owin
[![NuGet](https://img.shields.io/nuget/v/Cireson.HttpResponseCompression.Owin.svg?maxAge=2592000)](https://www.nuget.org/packages/Cireson.HttpResponseCompression.Owin/)

###Cireson.HttpResponseCompression.WebApi
[![NuGet](https://img.shields.io/nuget/v/Cireson.HttpResponseCompression.WebApi.svg?maxAge=2592000)](https://www.nuget.org/packages/Cireson.HttpResponseCompression.WebApi/)

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