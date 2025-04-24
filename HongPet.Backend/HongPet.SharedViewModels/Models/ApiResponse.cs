namespace HongPet.SharedViewModels.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? ErrorMessage { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
