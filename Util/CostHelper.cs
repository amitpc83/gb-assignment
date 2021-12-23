namespace GlobalBlue.Assignment
{
    public static class CostHelper
    {
        public static Cost CalculateCostFromNet(decimal amount, decimal vatRate)
        {
            if (vatRate == 0)
            {
                throw new ArgumentException("vat rate can not be zero");
            }
            Cost cost = new Cost
            {
                Net = amount.ToString()
            };
            vatRate = (vatRate / 100);

            cost.Gross = Round((amount * (1 + vatRate)));
            cost.Vat = Round((amount * vatRate));
            cost.VatRate = (vatRate*100).ToString();

            return cost;
        }
        public static Cost CalculateCostFromVat(decimal vat, decimal vatRate)
        {
            if (vatRate == 0)
            {
                throw new ArgumentException("vat rate can not be zero");
            }

            vatRate = (vatRate / 100);
            Cost cost = new Cost
            {
                Vat = vat.ToString()
            };

            cost.Net = Round((vat / vatRate));
            cost.Gross = Round((vat * ((1 + vatRate) / vatRate)));
            cost.VatRate = (vatRate * 100).ToString();
            return cost;
        }
        public static Cost CalculateCostFromGross(decimal gross, decimal vatRate)
        {
            if(vatRate == 0)
            {
                throw new ArgumentException("vat rate can not be zero");
            }
            vatRate = (decimal)vatRate / 100;
            Cost cost = new Cost
            {
                Gross = gross.ToString(),
            };

            cost.Net = Round((gross / (1 + vatRate)));
            cost.Vat = Round((gross * (vatRate / (1 + vatRate))));
            cost.VatRate = (vatRate * 100).ToString();

            return cost;

        }
        public static string Round(decimal amount)
        {
            var input = amount * 100;
            input = Math.Round(input, 0, MidpointRounding.AwayFromZero);
            return string.Format("{0:0.00}", (input / 100));
        }
    }
}
