public record struct Cost
{
    public string Net { get; set; }
    public string Gross { get; set; }
    public string Vat { get; set; }
    public string VatRate { get; set; }
}