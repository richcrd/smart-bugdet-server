using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMB.API.Contracts;
using SMB.APPLICATION.DTOs.Notifications;
using SMB.APPLICATION.Interfaces.Services;

namespace SMB.API.Controllers;

[Authorize]
[ApiController]
[Route("notifications")]
public class NotificationController(INotificationService notificationService) : ControllerBase
{
    [HttpPost("register-device")]
    public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceRequest request)
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await notificationService.RegisterDevice(userId, request);

        var result = new Answer<object?>
        {
            Message = "Dispositivo registrado correctamente",
            Response = null,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
    

    [HttpPost("test-daily-reminder")]
    public async Task<IActionResult> TestDailyReminder()
    {
        var userId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var sent = await notificationService.SendDailyReminderIfNeeded(userId);

        var result = new Answer<object?>
        {
            Message = sent
                ? "Recordatorio enviado (no habías registrado movimientos hoy)"
                : "No se envió: tienes las notificaciones desactivadas, ya registraste movimientos hoy, o no tienes dispositivos registrados",
            Response = null,
            Code = StatusCodes.Status200OK
        };

        return Ok(result);
    }
}
