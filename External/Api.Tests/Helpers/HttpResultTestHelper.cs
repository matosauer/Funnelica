using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Api.Tests.Helpers;

internal static class HttpResultTestHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static async Task<HttpResultSnapshot> InvokeAsync(IResult result)
    {
        var context = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider()
        };
        context.Response.Body = new MemoryStream();

        await result.ExecuteAsync(context);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        string? body = null;
        if (context.Response.Body.Length > 0)
        {
            using var reader = new StreamReader(context.Response.Body);
            body = await reader.ReadToEndAsync();
        }

        return new HttpResultSnapshot(context.Response.StatusCode, body);
    }

    public static T? DeserializeBody<T>(HttpResultSnapshot snapshot)
    {
        if (string.IsNullOrWhiteSpace(snapshot.Body))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(snapshot.Body, JsonOptions);
    }
}

internal sealed record HttpResultSnapshot(int StatusCode, string? Body);
