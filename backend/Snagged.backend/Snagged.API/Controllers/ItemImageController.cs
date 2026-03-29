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
    public class ItemImageController(IMediator mediator) : ControllerBase
    {
        [HttpGet("images/all-list")]
        public async Task<IActionResult> GetAllImages()
        {
            var result = await mediator.Send(new GetAllImagesQuery());
            return Ok(result);
        }

        [HttpGet("{itemId:int}/images")]
        public async Task<IActionResult> GetImages(int itemId)
        {
            var result = await mediator.Send(new GetItemImagesQuery { ItemId = itemId });
            return Ok(result);
        }

        [HttpPost("add-url")]
        public async Task<IActionResult> AddImage([FromBody] AddItemImageCommand command)
        {
            var id = await mediator.Send(command);
            return CreatedAtAction(nameof(GetImages), new { itemId = command.ItemId }, new { id });
        }

        [HttpPut("images/{imageId:int}/set-main")]
        public async Task<IActionResult> SetMainImage(int imageId)
        {
            var ok = await mediator.Send(new SetMainImageCommand { ImageId = imageId });
            if (!ok) return NotFound(new { message = "Image not found" });
            return NoContent();
        }

        [HttpPut("images/{imageId:int}/update")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromBody] UpdateItemImageCommand command)
        {
            command.Id = imageId;
            var ok = await mediator.Send(command);
            if (!ok) return NotFound(new { message = "Image not found" });
            return NoContent();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(int itemId, [FromForm] List<IFormFile> files)
        {
            if (files is null || files.Count == 0)
                return BadRequest(new { message = "No files provided." });

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

            var results = await mediator.Send(command);
            return Ok(results);
        }

        [HttpDelete("images/{imageId:int}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var deleted = await mediator.Send(new DeleteItemImageCommand { Id = imageId });
            if (!deleted) return NotFound(new { message = "Image not found" });
            return NoContent();
        }

        [HttpDelete("{itemId:int}/images/delete-all")]
        public async Task<IActionResult> DeleteAllImages(int itemId)
        {
            var count = await mediator.Send(new DeleteAllItemImagesCommand { ItemId = itemId });
            return Ok(new { deleted = count });
        }
    }
}