namespace Regard.Endpoint
{
    internal interface IEventValidator
    {
        bool IsValid(string eventPayload);
    }
}