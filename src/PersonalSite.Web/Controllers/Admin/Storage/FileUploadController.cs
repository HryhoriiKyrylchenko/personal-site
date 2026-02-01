namespace PersonalSite.Web.Controllers.Admin.Storage;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = "Admin", Policy = "PasswordChanged")]
public class FileUploadController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger<FileUploadController> _logger;

    public FileUploadController(IStorageService storageService, ILogger<FileUploadController> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost]
    [RequestSizeLimit(10_000_000)] // 10MB max size
    public async Task<IActionResult> Upload(
        [FromForm] IFormFile? file,
        [FromQuery] UploadFolder folder = UploadFolder.Uploads,
        CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is missing.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".pdf" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("Unsupported file type.");

        try
        {
            await using var stream = file.OpenReadStream();

            var fileName = $"{file.FileName}";
            
            var normalizedFolder = folder.ToString().ToLowerInvariant();
            
            var url = await _storageService.UploadFileAsync(
                stream, 
                fileName, 
                file.ContentType, 
                normalizedFolder, 
                cancellationToken);

            return Ok(new { url });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File upload failed.");
            return StatusCode(500, "File upload failed.");
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFile([FromQuery] string fileUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return BadRequest("File URL is required.");

        try
        {
            await _storageService.DeleteFileAsync(fileUrl, cancellationToken);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "File deletion failed.");
            return StatusCode(500, "File deletion failed.");
        }
    }

}