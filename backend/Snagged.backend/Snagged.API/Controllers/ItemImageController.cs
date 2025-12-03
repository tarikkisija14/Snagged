using MediatR;
using Microsoft.AspNetCore.Mvc;
using Snagged.Application.Catalog.ItemImages.Commands.AddItemImage;
using Snagged.Application.Catalog.ItemImages.Commands.DeleteAllItemImages;
using Snagged.Application.Catalog.ItemImages.Commands.DeleteItemImage;
using Snagged.Application.Catalog.ItemImages.Commands.SetMainImage;
using Snagged.Application.Catalog.ItemImages.Commands.UpdateItemImage;
using Snagged.Application.Catalog.ItemImages.Commands.UploadImage;
using Snagged.Application.Catalog.ItemImages.Queries.GetAllImages;
using Snagged.Application.Catalog.ItemImages.Queries.GetItemImages;

namespace Snagged.API.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("images/all-list")]
        public async Task<IActionResult> GetAllImages()
        {
            try
            {
                var result = await _mediator.Send(new GetAllImagesQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{itemId}/images")]
        public async Task<IActionResult> GetImages(int itemId)
        {
            try
            {
                var result = await _mediator.Send(new GetItemImagesQuery { ItemId = itemId });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("add-url")]
        public async Task<IActionResult> AddImage(int itemId, AddItemImageCommand command)
        {
            try
            {
                command.ItemId = itemId;
                var id = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetImages), new { itemId }, null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("images/{imageId}/set-main")]
        public async Task<IActionResult> SetMainImage(int imageId)
        {
            try
            {
                var ok = await _mediator.Send(new SetMainImageCommand { ImageId = imageId });
                if (!ok) return NotFound(new { message = "Image not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("images/{imageId}/update")]
        public async Task<IActionResult> UpdateImage(int imageId, UpdateItemImageCommand cmd)
        {
            try
            {
                cmd.Id = imageId;
                var ok = await _mediator.Send(cmd);
                if (!ok) return NotFound(new { message = "Image not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("upload")]
         public async Task<IActionResult> UploadImage(int itemId, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { message = "No files provided" });

            try
            {
                var command = new UploadImageCommand
                {
                    ItemId = itemId,
                    FilesBytes = new List<byte[]>(),
                    FileNames = new List<string>()
                };

                foreach (var file in files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    command.FilesBytes.Add(ms.ToArray());
                    command.FileNames.Add(file.FileName);
                }

                var results = await _mediator.Send(command);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }


        [HttpDelete("images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteItemImageCommand { Id = imageId });
                if (!result) return NotFound(new { message = "Image not found" });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{itemId}/images/delete-all")]
        public async Task<IActionResult> DeleteAll(int itemId)
        {
            try
            {
                var count = await _mediator.Send(new DeleteAllItemImagesCommand { ItemId = itemId });
                return Ok(new { deleted = count });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

    }
}
