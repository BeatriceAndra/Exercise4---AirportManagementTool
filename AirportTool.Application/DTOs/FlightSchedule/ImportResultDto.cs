public class ImportResultDto
{
    public int Total { get; set; }
    public int Created { get; set; }
    public int Updated { get; set; }
    public List<ImportErrorDto> Errors { get; set; } = new();
}
