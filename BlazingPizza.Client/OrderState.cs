using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace BlazingPizza.Client;

public class OrderState
{
    public Order Order { get; set; } = new Order();

    [JsonIgnore]
    public IJSRuntime JSRuntime { get; set; }

    [JsonIgnore]
    public bool ShowingConfigureDialog { get; private set; }
    [JsonIgnore]
    public RefillCard? ConfiguringRefillCard { get; private set; }

    public void AddRefillCard(RefillCard card, int quantity)
    {
        var existing = Order.Cards.FirstOrDefault(c => c.RefillCardId == card.id);
        if (existing != null)
        {
            existing.Quantity += quantity;
        }
        else
        {
            Order.Cards.Add(new RefillCardOrder
            {
                RefillCardId = card.id,
                ProductName = card.ProductName,
                Quantity = quantity,
                UnitPrice = card.price
            });
        }
        SaveStateToStorage(JSRuntime);
    }

    public void RemoveRefillCard(int refillCardId)
    {
        Order.Cards.RemoveAll(c => c.RefillCardId == refillCardId);
        SaveStateToStorage(JSRuntime);
    }

    public void ResetOrder()
    {
        Order = new Order();
        SaveStateToStorage(JSRuntime);
    }

    public async Task GetStateFromLocalStorage(IJSRuntime jsRuntime)
    {
        var locallyStoredState = await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "blazingPizza.orderState");
        if (!string.IsNullOrEmpty(locallyStoredState))
        {
            var deserializedState =
                JsonSerializer.Deserialize<OrderState>(locallyStoredState, new JsonSerializerOptions { IncludeFields = true });
            if (deserializedState != null)
            {
                Order = deserializedState.Order;
            }
        }
    }

    public async Task SaveStateToStorage(IJSRuntime jsRuntime)
    {
        var stateAsJson = JsonSerializer.Serialize(this, new JsonSerializerOptions { IncludeFields = true });
        await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "blazingPizza.orderState", stateAsJson);
    }

    public void ShowConfigureRefillCardDialog(RefillCard card)
    {
        ConfiguringRefillCard = card;
        ShowingConfigureDialog = true;
    }

    public void CancelConfigureRefillCardDialog()
    {
        ConfiguringRefillCard = null;
        ShowingConfigureDialog = false;
    }

    public void ConfirmConfigureRefillCardDialog(int quantity = 1)
    {
        if (ConfiguringRefillCard is not null)
        {
            AddRefillCard(ConfiguringRefillCard, quantity);
            ConfiguringRefillCard = null;
        }
        ShowingConfigureDialog = false;
    }
}