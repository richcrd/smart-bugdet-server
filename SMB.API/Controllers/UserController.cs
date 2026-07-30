using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Catalog;
using SMB.APPLICATION.DTOs.User;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("user")]
public class UserController(IUserService userService, ICatalogService catalogService) : ControllerBase
{
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = User.Identity?.Name;

        var result = new Answer<object>
        {
            Message = "Usuario autenticado",
            Response = new
            {
                UserId = userId,
                UserName = userName
            },
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await userService.UpdateProfile(userId, request);

        return Ok(new Answer<object?>
        {
            Message = "Perfil actualizado correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await userService.ChangePassword(userId, request);

        return Ok(new Answer<object?>
        {
            Message = "Contraseña actualizada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var profile = await userService.GetProfile(userId);

        return Ok(new Answer<UserProfileResponse>
        {
            Message = "Perfil obtenido correctamente",
            Response = profile,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var categories = await catalogService.GetUserCategories(userId);

        return Ok(new Answer<List<CategoriesResponse>>
        {
            Message = "Categorías obtenidas correctamente",
            Response = categories,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var category = await catalogService.CreateCategory(userId, request);

        return StatusCode(StatusCodes.Status201Created, new Answer<CategoriesResponse>
        {
            Message = "Categoría creada correctamente",
            Response = category,
            Code = StatusCodes.Status201Created
        });
    }
    
    [HttpPut("categories/{id:long}")]
    public async Task<IActionResult> UpdateCategory(long id, [FromBody] UpdateCategoryRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var category = await catalogService.UpdateCategory(id, userId, request);

        return Ok(new Answer<CategoriesResponse>
        {
            Message = "Categoría actualizada correctamente",
            Response = category,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpDelete("categories/{id:long}")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await catalogService.DeleteCategory(id, userId);

        return Ok(new Answer<object?>
        {
            Message = "Categoría eliminada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpPost("subcategories")]
    public async Task<IActionResult> CreateSubcategory([FromBody] CreateSubcategoryRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var subcategory = await catalogService.CreateSubcategory(userId, request);

        return StatusCode(StatusCodes.Status201Created, new Answer<SubcategoriesResponse>
        {
            Message = "Subcategoría creada correctamente",
            Response = subcategory,
            Code = StatusCodes.Status201Created
        });
    }
    
    [HttpPut("subcategories/{id:long}")]
    public async Task<IActionResult> UpdateSubcategory(long id, [FromBody] UpdateSubcategoryRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var subcategory = await catalogService.UpdateSubcategory(id, userId, request);

        return Ok(new Answer<SubcategoriesResponse>
        {
            Message = "Subcategoría actualizada correctamente",
            Response = subcategory,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpDelete("subcategories/{id:long}")]
    public async Task<IActionResult> DeleteSubcategory(long id)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await catalogService.DeleteSubcategory(id, userId);

        return Ok(new Answer<object?>
        {
            Message = "Subcategoría eliminada correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpGet("payment-methods")]
    public async Task<IActionResult> GetPaymentMethods()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var paymentMethods = await catalogService.GetUserPaymentMethods(userId);

        return Ok(new Answer<List<UserPaymentMethodResponse>>
        {
            Message = "Métodos de pago obtenidos correctamente",
            Response = paymentMethods,
            Code = StatusCodes.Status200OK
        });
    }
    
    [HttpPost("payment-methods")]
    public async Task<IActionResult> LinkPaymentMethod([FromBody] LinkPaymentMethodRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var paymentMethod = await catalogService.LinkPaymentMethod(userId, request);

        return StatusCode(StatusCodes.Status201Created, new Answer<UserPaymentMethodResponse>
        {
            Message = "Método de pago vinculado correctamente",
            Response = paymentMethod,
            Code = StatusCodes.Status201Created
        });
    }
    
    [HttpPut("payment-methods/{id:long}")]
    public async Task<IActionResult> UpdatePaymentMethodAlias(long id, [FromBody] UpdatePaymentMethodAliasRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var paymentMethod = await catalogService.UpdatePaymentMethodAlias(id, userId, request);

        return Ok(new Answer<UserPaymentMethodResponse>
        {
            Message = "Alias actualizado correctamente",
            Response = paymentMethod,
            Code = StatusCodes.Status200OK
        });
    }

    [HttpDelete("payment-methods/{id:long}")]
    public async Task<IActionResult> UnlinkPaymentMethod(long id)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await catalogService.UnlinkPaymentMethod(id, userId);

        return Ok(new Answer<object?>
        {
            Message = "Método de pago desvinculado correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        });
    }
}