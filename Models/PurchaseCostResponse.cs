namespace GlobalBlue.Assignment
{
    public class PurchaseCostResponse:ResponseBase
    { 
        public string Net{ get; set; } = default!;
        public string Gross { get; set; } = default!;
        public string Vat { get; set; } = default!;
        public string VatRate { get; set; } = default!;
    }
}
