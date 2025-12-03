using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Notifications.Commands.AddNotification;
using Snagged.Application.Catalog.Notifications.Commands.DeleteNotification;
using Snagged.Application.Catalog.Notifications.Commands.DeleteUserNotifications;
using Snagged.Application.Catalog.Notifications.Commands.MarkAsAllRead;
using Snagged.Application.Catalog.Notifications.Commands.MarkAsRead;
using Snagged.Application.Catalog.Notifications.Queries.GetNotificationById;
using Snagged.Application.Catalog.Notifications.Queries.GetUnreadCountForUser;
using Snagged.Application.Catalog.Notifications.Queries.GetUsersNotifcations;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddNotificationCommand cmd)
        {
            try
            {
                var id = await _mediator.Send(cmd);
                return Ok(new { id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var notif = await _mediator.Send(new GetNotificationByIdQuery { Id = id });
                if (notif == null) return NotFound(new { message = "Notification not found" });
                return Ok(notif);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetForUser(int userId)
        {
            try
            {
                var list = await _mediator.Send(new GetUsersNotificationsQuery { UserId = userId });
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("user/{userId}/unread-count")]
        public async Task<IActionResult> UnreadCount(int userId)
        {
            try
            {
                var count = await _mediator.Send(new GetUnreadCountQuery { UserId = userId });
                return Ok(new { unread = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            try
            {
                var ok = await _mediator.Send(new MarkAsReadCommand { Id = id });
                return ok ? Ok() : NotFound(new { message = "Notification not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("user/{userId}/read-all")]
        public async Task<IActionResult> MarkAll(int userId)
        {
            try
            {
                var count = await _mediator.Send(new MarkAsAllReadCommand { UserId = userId });
                return Ok(new { marked = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await _mediator.Send(new DeleteNotificationCommand { Id = id });
                return ok ? Ok() : NotFound(new { message = "Notification not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteForUser(int userId)
        {
            try
            {
                var count = await _mediator.Send(new DeleteUserNotificationsCommand { UserId = userId });
                return Ok(new { deleted = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }


    }
}
