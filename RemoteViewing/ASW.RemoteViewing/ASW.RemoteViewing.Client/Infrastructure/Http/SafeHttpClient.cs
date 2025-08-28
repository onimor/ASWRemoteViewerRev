using ASW.Shared.Extentions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ASW.RemoteViewing.Client.Infrastructure.Http;

public class SafeHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public SafeHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new DateTimeLocalJsonConverter() }
        };
    }
    public async Task<T?> SendAsync<T>(HttpRequestMessage request,
    CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.SendAsync(request, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
    }
    public async Task<T?> GetAsync<T>(string uri,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<T>(_jsonOptions, cancellationToken);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string uri, TRequest data,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(uri, data, _jsonOptions, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
    }

    public async Task PostAsync<TRequest>(string uri, TRequest data,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsJsonAsync(uri, data, _jsonOptions, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
    }
    public async Task<TResponse?> PostAsync<TResponse>(string uri,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(uri, null, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
    }
    public async Task<byte[]> PostAsyncForFile<TRequest>(string uri, TRequest data, 
        CancellationToken cancellationToken = default)
    { 
        var response = await _httpClient.PostAsJsonAsync(uri, data, _jsonOptions, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken); 
        var bytes = await response.Content.ReadAsByteArrayAsync(cancellationToken); 
        return bytes;
    }
    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string uri, TRequest data,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(uri, data, _jsonOptions, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions, cancellationToken);
    }

    public async Task PutAsync<TRequest>(string uri, TRequest data,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PutAsJsonAsync(uri, data, _jsonOptions, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
    }

    public async Task DeleteAsync(string uri,
        CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);
        await EnsureSuccessOrThrow(response, cancellationToken);
    }

    private async Task EnsureSuccessOrThrow(HttpResponseMessage response,
        CancellationToken cancellationToken = default)
    {
        if (response.IsSuccessStatusCode)
            return;

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        ProblemDetails? problem = null;
        try
        {
            problem = JsonSerializer.Deserialize<ProblemDetails>(content, _jsonOptions);
        }
        catch
        {
            // Игнорируем — возможно, это не ProblemDetails.
        }

        var title = problem?.Title ?? "Ошибка";
        var detail = problem?.Detail ?? content;

        throw response.StatusCode switch
        {
            HttpStatusCode.BadRequest => new ValidationException(detail),
            HttpStatusCode.Unauthorized => new UnauthorizedAccessException(detail),
            HttpStatusCode.Forbidden => new AccessViolationException(detail),
            HttpStatusCode.NotFound => new Exception("Ресурс не найден"),
            _ => new Exception($"Не известная ошибка {(int)response.StatusCode}: {title}"),
        };
    }
}