using Microsoft.JSInterop;

namespace BlazingPizza.Client;

public static class JSRuntimeExtensions
{
	public static ValueTask<bool> Confirm(this IJSRuntime jsRuntime, string message)
	{
		return jsRuntime.InvokeAsync<bool>("confirm", message);
	}

	// Notification methods
	public static ValueTask ShowToast(this IJSRuntime jsRuntime, string title, string message, string type = "success", int duration = 5000)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showToast", title, message, type, duration);
	}

	public static ValueTask ShowSuccess(this IJSRuntime jsRuntime, string title, string message, int duration = 5000)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showSuccess", title, message, duration);
	}

	public static ValueTask ShowError(this IJSRuntime jsRuntime, string title, string message, int duration = 7000)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showError", title, message, duration);
	}

	public static ValueTask ShowWarning(this IJSRuntime jsRuntime, string title, string message, int duration = 6000)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showWarning", title, message, duration);
	}

	public static ValueTask ShowInfo(this IJSRuntime jsRuntime, string title, string message, int duration = 5000)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showInfo", title, message, duration);
	}

	public static ValueTask ShowNotification(this IJSRuntime jsRuntime, object notification)
	{
		return jsRuntime.InvokeVoidAsync("blazingPizzaNotifications.showNotification", notification);
	}
}
