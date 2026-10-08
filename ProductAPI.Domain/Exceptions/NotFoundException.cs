namespace ProductAPI.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) 
        : base($"La entidad \"{name}\" con el ID ({key}) no fue encontrada.")
    {
    }
}
