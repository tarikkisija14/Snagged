using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.Notifications.Commands.AddNotification;
using Snagged.Application.Catalog.Notifications.Commands.DeleteNotification;
using Snagged.Application.Catalog.Notifications.Commands.DeleteUserNotifications;
using Snagged.Application.Catalog.Notifications.Commands.MarkAsAllRead;
using Snagged.Application.Catalog.Notifications.Commands.MarkAsRead;
using Snagged.Application.Catalog.Notifications.Queries.GetNotificationById;
using Snagged.Application.Catalog.Notifications.Queries.GetUnreadCountForUser;
using Snagged.Application.Catalog.Notifications.Queries.GetUsersNotifcations;
using Snagged.Application.Common.Interfaces;

namespace Snagged.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController(IMediator mediator, ICurrentUserService currentUser) : ControllerBase
    {
        
        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(AddNotificationCommand cmd)
        {
            var id = await mediator.Send(cmd);
            return Ok(new { id });
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var notif = await mediator.Send(new GetNotificationByIdQuery { Id = id });
            if (notif == null) return NotFound(new { message = "Notification not found" });

            
            if (notif.UserId != currentUser.UserId)
                return Forbid();

            return Ok(notif);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var list = await mediator.Send(new GetUsersNotificationsQuery { UserId = currentUser.UserId });
            return Ok(list);
        }

        [HttpGet("my/unread-count")]
        public async Task<IActionResult> MyUnreadCount()
        {
            var count = await mediator.Send(new GetUnreadCountQuery { UserId = currentUser.UserId });
            return Ok(new { unread = count });
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            try
            {
                var ok = await mediator.Send(new MarkAsReadCommand { Id = id });
                return ok ? Ok() : NotFound(new { message = "Notification not found" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("my/read-all")]
        public async Task<IActionResult> MarkAll()
        {
            var count = await mediator.Send(new MarkAsAllReadCommand { UserId = currentUser.UserId });
            return Ok(new { marked = count });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var ok = await mediator.Send(new DeleteNotificationCommand { Id = id });
                return ok ? Ok() : NotFound(new { message = "Notification not found" });
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("my")]
        public async Task<IActionResult> DeleteMyNotifications()
        {
            var count = await mediator.Send(new DeleteUserNotificationsCommand { UserId = currentUser.UserId });
            return Ok(new { deleted = count });
        }
    }
}