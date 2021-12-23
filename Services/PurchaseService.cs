namespace GlobalBlue.Assignment.Services
{
    public class PurchaseService : IPurchaseService
    {
        public async Task<ResponseBase> CalculatePurchaseCostAsync(PurchaseCostRequest purchaseCostRequest)
        {
            // Validate inputs           
            var inputValidationResult = await Task.Run(() => ValidateInput(purchaseCostRequest));

            // return error response in case of validation errors
            if (inputValidationResult.InputValidationErrors.Count() > 0)
            {
                return ValidationResult(inputValidationResult.InputValidationErrors);
            }

            // calculate cost
            var cost = await Task.Run(() => CalculateCost(
                inputValidationResult.Amount, inputValidationResult.VatRate, inputValidationResult.AmountType
            ));

            return new PurchaseCostResponse
            {
                VatRate = cost.VatRate,
                Gross = cost.Gross,
                Success = true,
                Net = cost.Net,
                Vat = cost.Vat
            };
        }


        #region Private Methods

        /// <summary>
        /// Method to return validation error response
        /// </summary>
        /// <param name="inputValidationResult">dictionary containing validation errors</param>
        /// <returns></returns>
        private static ResponseBase ValidationResult(Dictionary<string, string> inputValidationResult)
        {
            var errors = new List<Error>();
            foreach (var error in inputValidationResult)
            {
                errors.Add(new Error
                {
                    ErrorType = error.Key,
                    ErrorMessage = error.Value
                });
            }

            return new ValidationErrorResponse
            {
                Success = false,
                Errors = errors
            };
        }

        /// <summary>
        /// Validates input parameters for purchase cost request
        /// </summary>
        /// <param name="purchaseCostRequest">Request parameters</param>
        /// <returns>A struct containing dictionary of validation errors,amount,amount type</returns>
        private CostInputValidationResult ValidateInput(PurchaseCostRequest purchaseCostRequest)
        {
            Dictionary<string, string> inputValidationDictionary = new Dictionary<string, string>();
            CostInputValidationResult costInputValidationResult = new CostInputValidationResult();

            List<PurchaseAmountType> purchaseAmountTypes = new List<PurchaseAmountType>
            {
                new PurchaseAmountType{Amount= purchaseCostRequest.Net,AmountType= PurchaseConstants.NetAmount},
                new PurchaseAmountType{Amount= purchaseCostRequest.Gross,AmountType= PurchaseConstants.GrossAmount},
                new PurchaseAmountType{Amount= purchaseCostRequest.Vat,AmountType= PurchaseConstants.VatAmount}
            };

            // find list of items which are not null or empty
            var nonEmptyPurchaseAmountTypes = purchaseAmountTypes.Where(s => !string.IsNullOrEmpty(s.Amount)).ToList();

            // no amount
            if (!nonEmptyPurchaseAmountTypes.Any())
            {
                inputValidationDictionary.Add(PurchaseConstants.MissingAmountKey, PurchaseConstants.MissingAmountMessage);
            }
            // more than one amount
            else if (nonEmptyPurchaseAmountTypes?.Count() > 1)
            {
                inputValidationDictionary.Add(PurchaseConstants.AdditionalAmountKey,PurchaseConstants.AdditionalAmountMessage);
            }
            else
            {
                var amount = nonEmptyPurchaseAmountTypes?.FirstOrDefault().Amount;
                decimal.TryParse(amount, out decimal decimalAmount);
                
                // invalid amount
                if (decimalAmount <= 0)
                {
                    inputValidationDictionary.Add(PurchaseConstants.InvalidAmountKey,PurchaseConstants.InvalidAmountMessage);
                }
                else
                {
                    costInputValidationResult.Amount = decimalAmount;
                    costInputValidationResult.AmountType = nonEmptyPurchaseAmountTypes?.FirstOrDefault().AmountType;
                }
            }

            // missing vat rate
            if (string.IsNullOrEmpty(purchaseCostRequest.VatRate))
            {
                inputValidationDictionary.Add(PurchaseConstants.MissingVatRateKey, PurchaseConstants.MissingVatRateMessage);
            }
            else
            {
                decimal.TryParse(purchaseCostRequest.VatRate, out decimal rate);

                // invalid vat rate
                if (rate <= 0)
                {
                    inputValidationDictionary.Add(PurchaseConstants.InvalidVatRateKey, PurchaseConstants.InvalidVatRateMessage);
                }
                else
                {
                    costInputValidationResult.VatRate = rate;
                }            
        }

        costInputValidationResult.InputValidationErrors = inputValidationDictionary;
            return costInputValidationResult;

        }
       
        private Cost CalculateCost(decimal? amount, decimal? vatRate, string? amountType)
    {
        if (amount == null)
        {
            throw new ArgumentNullException(nameof(amount));
        }
        if (vatRate == null)
        {
            throw new ArgumentNullException(nameof(vatRate));
        }
        if (string.IsNullOrEmpty(amountType))
        {
            throw new ArgumentNullException(nameof(amountType));
        }


        Cost cost;
        if (amountType == PurchaseConstants.GrossAmount)
        {
            cost = CostHelper.CalculateCostFromGross(amount.Value, vatRate.Value);
        }
        else if (amountType == PurchaseConstants.VatAmount)
        {
            cost = CostHelper.CalculateCostFromVat(amount.Value, vatRate.Value);
        }
        else if (amountType == PurchaseConstants.NetAmount)
        {
            cost = CostHelper.CalculateCostFromNet(amount.Value, vatRate.Value);
        }
        else
        {
            throw new Exception(PurchaseConstants.InvalidAmountKey);
        }

        return cost;
    }
        #endregion

        // temporary structs used in private methods
        #region Private Structs
        private struct CostInputValidationResult
        {
            public Dictionary<string, string> InputValidationErrors { get; set; }
            public decimal? Amount { get; set; }
            public decimal? VatRate { get; set; }
            public string? AmountType { get; set; }
        }
        private struct PurchaseAmountType
        {
            public string? Amount { get; set; }
            public string? AmountType { get; set; }
        }
        #endregion

    }
}
