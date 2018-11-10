namespace InexRef.EventSourcing.Contracts
{
    public interface IOperationContextInternal
    {
        void SetIsLoading(bool isLoading);
    }
}