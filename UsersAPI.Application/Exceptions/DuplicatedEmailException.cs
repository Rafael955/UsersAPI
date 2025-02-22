namespace UsersAPI.Application.Exceptions
{
    public class DuplicatedEmailException : Exception
    {
        public override string Message { get; }
        
        public DuplicatedEmailException(string message)
        {
            Message = message;
        }
    }
}
