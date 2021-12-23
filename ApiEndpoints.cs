using GlobalBlue.Assignment.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace GlobalBlue.Assignment
{
    /// <summary>
    /// A class containing the HTTP endpoints for the Purchase API.
    /// </summary>
    public static class ApiEndpoints
    {
        /// <summary>
        /// Adds the services to Purchase API 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddPurchaseApi(this IServiceCollection services)
        {
            services.AddScoped<IPurchaseService, PurchaseService>();
            return services;
        }

        public static IEndpointRouteBuilder MapPurchaseApiRoutes(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/purchasecost",CalculateCost);            
            return endpointRouteBuilder;
        }


        private static async Task<IResult> CalculateCost(
            string? net_amount,
            string? gross_amount,
            string? vat_amount,
            string? vat_rate,
            IPurchaseService purchaseService)
        {
            try
            {
                var result = await purchaseService.CalculatePurchaseCostAsync(new PurchaseCostRequest
                {
                    Gross = gross_amount,
                    Vat = vat_amount,
                    Net = net_amount,
                    VatRate = vat_rate
                });

                if (result.Success)
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.BadRequest(result);
                }
            }
            catch (Exception ex)
            {                
                return Results.Problem("An error occured while processing your request.");
            }
        }
       
    }
}
