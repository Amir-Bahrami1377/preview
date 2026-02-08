namespace FormerUrban_Afta.DataAccess.DTOs
{
    public class TaskResult
    {
        public bool IsOk { get; set; }
        public string Message { get; set; }

        public TaskResult(bool isOk, string message)
        {
            IsOk = isOk;
            Message = message;
        }
        public TaskResult() : this(false, string.Empty) { }
    }
}
