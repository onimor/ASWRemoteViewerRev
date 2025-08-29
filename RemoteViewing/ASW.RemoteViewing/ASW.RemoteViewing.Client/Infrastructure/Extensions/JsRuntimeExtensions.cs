using Microsoft.JSInterop;

namespace ASW.RemoteViewing.Client.Infrastructure.Extensions;

public static class JsRuntimeExtensions
{
    public static async Task SaveAsExcelAsync(this IJSRuntime js, string fileName, byte[]? bytes)
    {
        await js.InvokeVoidAsync("saveExcelFromByteArray", fileName, bytes);
    }
}
