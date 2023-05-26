namespace GhibliUniverse.Core.Domain.Models.Exceptions;

public class ModelNotFoundException : Exception
{
    public ModelNotFoundException(Guid modelId)
        : base($"The model you are trying to perform an operation on does not exist. Model Id: {modelId}")
    {
        
    }
}