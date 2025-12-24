using Microsoft.AspNetCore.Http;

public class UploadFileDto
{
    public IFormFile File { get; set; }
}