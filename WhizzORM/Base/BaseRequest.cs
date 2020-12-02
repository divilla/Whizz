using System;
using MediatR;
using WhizzORM.Context;
using WhizzORM.Interfaces;

namespace WhizzORM.Base
{
    public class BaseRequest<TResponse> : IRequest<TResponse>
        where TResponse : class, new()
    {
        // public BaseRequest(ref DbContext context, ref IMediator mediator)
        // {
        //     Context = context;
        //     
        // }

        public IDbContext Context { get; set; }
        public IMediator Mediator { get; set; }

        public void Send()
        {
            Mediator.Send(this);
        }
    }
}