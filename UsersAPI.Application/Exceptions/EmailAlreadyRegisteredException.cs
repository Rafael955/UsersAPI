namespace UsersAPI.Application.Exceptions
{
    public class EmailAlreadyRegisteredException : Exception
    {
        public override string Message { get; }

        public EmailAlreadyRegisteredException()
        {
            Message = $"O email informado já está cadastrado. Tente outro.";
        }

        public EmailAlreadyRegisteredException(string message)
        {
            Message = message;
        }
    }
}
