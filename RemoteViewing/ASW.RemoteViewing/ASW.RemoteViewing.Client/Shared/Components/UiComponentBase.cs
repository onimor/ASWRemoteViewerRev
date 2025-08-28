using ASW.Shared.Extentions;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace ASW.RemoteViewing.Client.Shared.Components;

public abstract class UiComponentBase : ComponentBase
{
    [Inject] protected ISnackbar SnackbarService { get; set; } = default!;
    protected void ShowError(string message) => SnackbarService.Add(message, Severity.Error);
    protected void ShowSuccess(string message) => SnackbarService.Add(message, Severity.Success);
    protected bool Processing { get; set; }
 
    protected async Task RunSafe(Func<Task> action, string? errorMessage = null, bool showProcessing = false)
    {
        if (showProcessing) StartProcessing();
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            HandleException(ex, errorMessage);
        }
        finally
        {
            if (showProcessing) StopProcessing();
        }
    }

    protected async Task<T?> RunSafe<T>(Func<Task<T?>> action, string? errorMessage = null, bool showProcessing = false)
    {
        if (showProcessing) StartProcessing();
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            HandleException(ex, errorMessage);
            return default;
        }
        finally
        {
            if (showProcessing) StopProcessing();
        }
    }

    private void StartProcessing()
    {
        Processing = true;
        StateHasChanged();
    }

    private void StopProcessing()
    {
        Processing = false;
        StateHasChanged();
    }
     
    private void HandleException(Exception ex, string? errorMessage = null)
    {
        ShowError(ex is ValidationException
            ? ex.Message
            : string.IsNullOrEmpty(errorMessage) ? "Что-то пошло не так" : errorMessage);
    }
}
