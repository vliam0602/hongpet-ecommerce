namespace HongPet.SharedViewModels.ResponseModel
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } = false;
        public string? ErrorMessage { get; set; }
        public object? Data { get; set; }
    }
}
