using WhizzORM.Base;

namespace WhizzORM.Requests
{
    public class GetAllRequest<TResponse> : BaseRequest<TResponse>
        where TResponse : class, new()
    {
    }
}