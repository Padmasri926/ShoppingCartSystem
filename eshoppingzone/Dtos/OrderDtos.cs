namespace EshoppingZone.Dtos
{
    public record PlaceOrderResponse(int OrderId, decimal TotalAmount, string Status);
}
