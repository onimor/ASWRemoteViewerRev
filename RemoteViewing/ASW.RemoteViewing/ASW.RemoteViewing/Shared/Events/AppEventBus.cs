namespace ASW.RemoteViewing.Shared.Events;

public class AppEventBus
{
    // === Пост изменён ===
    public delegate void PostChangedHandler(Guid postId);
    public event PostChangedHandler? OnPostChanged;
    public void RaisePostChanged(Guid postId)
        => OnPostChanged?.Invoke(postId);

    // === Удалена машина ===
    public delegate void CarDeletedHandler(Guid carId);
    public event CarDeletedHandler? OnCarDeleted;
    public void RaiseCarDeleted(Guid carId)
        => OnCarDeleted?.Invoke(carId);

    // === Удалённое взвешивание ===
    public delegate void RemoteWeighingHandler(Guid postId, bool isRemoteWeightHappened);
    public event RemoteWeighingHandler? OnRemoteWeighing;
    public void RaiseRemoteWeighing(Guid postId, bool happened)
        => OnRemoteWeighing?.Invoke(postId, happened);

    // === Пользователь вошёл ===
    public delegate void UserLoggedInHandler(Guid userId, string username);
    public event UserLoggedInHandler? OnUserLoggedIn;
    public void RaiseUserLoggedIn(Guid userId, string username)
        => OnUserLoggedIn?.Invoke(userId, username);

    // === Настройки изменены ===
    public delegate void SettingsUpdatedHandler(string key, string? newValue);
    public event SettingsUpdatedHandler? OnSettingsUpdated;
    public void RaiseSettingsUpdated(string key, string? newValue)
        => OnSettingsUpdated?.Invoke(key, newValue);
}

