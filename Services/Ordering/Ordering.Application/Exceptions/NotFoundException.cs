namespace Ordering.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity with name \"{name}\" and key \"{key}\" wan not found ")
        {

        }
    }
}
