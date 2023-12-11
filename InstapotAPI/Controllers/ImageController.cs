using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using InstapotAPI.Entity;
using InstapotAPI.Infrastructure.Repositories;

namespace InstapotAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImageController : ControllerBase
{
    private readonly ImageRepository _imageRepo;
    private readonly CommentRepository _commentRepo;
    public ImageController(ImageRepository imageRepo, CommentRepository commentRepo)
    {
        _imageRepo = imageRepo;
        _commentRepo = commentRepo;
    }
    [HttpGet]
    [Route("ImagesFromUser{id}")]
    public async Task<ActionResult<List<Image>>> GetImagesFromUser(int id)
    {
        var images = await _imageRepo.GetImagesFromUser(id);
        if (images == null) return NotFound();
        return Ok(images);
    }
    [HttpGet]
    [Route("ImageFlowForUser{id}")]
    public async Task<ActionResult<List<Image>>> GetImageFlow(int id)
    {
        var images = await _imageRepo.GetImageFlow(id);
        if (images == null) return NotFound();
        return Ok(images);
    }
    [HttpGet]
    [Route("GetImage{id}")]
    public async Task<ActionResult<Image>> GetImage(int id)
    {
        var image = await _imageRepo.GetImage(id);
        if (image == null) return NotFound();
        return Ok(image);
    }
    [HttpPost]
    [Route("PostImage{userId}/{path}/{description}")]
    public async Task<ActionResult<Image>> PostImage(int userID, string path, string desc)
    {
        var newImage = new Image() { Path = path, Description = desc, UserID = userID, Comments = new List<int>(), Title = "", LikedBy = new List<int>(), isPublished = true, CreatedDate = DateTime.UtcNow};
        var createdImage = await _imageRepo.CreateNewImage(newImage);

        if(createdImage == null) return BadRequest();

        return Ok(createdImage);
    }
    [HttpPost]
    [Route("PostComment{imageId}/{userId}/{comment}")]
    public async Task<ActionResult<Comment>> PostComment(int userId, int imageId, string comment)
    {
        var newComment = new Comment() { ImageID = imageId, UserID = userId, Text = comment, CreatedDate = DateTime.UtcNow };
        var createdComment = await _commentRepo.CreateComment(newComment);
        if(createdComment == null) return BadRequest();
        return Ok(createdComment);
    }
    [HttpPut]
    [Route("Like{imageId}/{userId}")]
    public async Task<ActionResult<bool>> LikeImage(int imageId, int userId)
    {
        var image = await _imageRepo.GetImage(imageId);
        if(image == null) return BadRequest();
        if (image.LikedBy.Contains(userId))
        {
            await _imageRepo.RemoveLike(imageId, userId);
            return Ok(false);
        }
        await _imageRepo.AddLike(imageId, userId);
        return Ok(true);
    }
}
