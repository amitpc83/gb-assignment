namespace GlobalBlue.Assignment.Services
{
    public interface IPurchaseService
    {
        Task<ResponseBase> CalculatePurchaseCostAsync(PurchaseCostRequest purchaseCostRequest);
    }
}
