namespace EshoppingZone.Dtos
{
    public record MakePaymentRequest(int OrderId, string PaymentMode, decimal Amount, int? WalletId);
}