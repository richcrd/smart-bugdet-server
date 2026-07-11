using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[ApiController]
[Route("catalog")]
public class CatalogController(ICatalogService catalogService) : ControllerBase
{
    [HttpGet("language")]
    public async Task<IActionResult> GetAllLanguages()
    {
        var languages = await catalogService.GetAllLanguages();

        var response = new Answer<List<LanguageResponse>>()
        {
            Message = "Se ha recuperado la información correctamente",
            Response = languages,
            Code = StatusCodes.Status200OK,
        };

        return Ok(response);
    }

    [HttpGet("currencies")]
    public async Task<IActionResult> GetAllCurrencies()
    {
        var currencies = await catalogService.GetAllCurrencies();

        var response = new Answer<List<CurrencyResponse>>()
        {
            Message = "Se ha recuperado la información correctamente",
            Response = currencies,
            Code = StatusCodes.Status200OK
        };

        return Ok(response);
    }
    
    [HttpGet("categories")]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await catalogService.GetAllCategories();

        var response = new Answer<List<CategoriesResponse>>()
        {
            Code = StatusCodes.Status200OK,
            Response = categories,
            Message = "Se ha recuperado la información correctamente",
        };

        return Ok(response);
    }

    [HttpGet("payment-methods")]
    public async Task<IActionResult> GetAllPaymentMethods()
    {
        var paymentMethods = await catalogService.GetAllPaymentMethods();

        var response = new Answer<List<PaymentMethodResponse>>()
        {
            Message = "Se ha recuperado la información correctamente",
            Code = StatusCodes.Status200OK,
            Response = paymentMethods
        };

        return Ok(response);
    }
}