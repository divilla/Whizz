using System;
using WhizzORM.Context;

namespace WhizzORM.JsonValidate
{
    public abstract class BaseJsonHandler
    {
        protected JsonRepository Repository { get; private set; }
        protected Func<string, string> DbCaseResolver => Repository.DbCaseResolver;
        protected Func<string, string> JsonCaseResolver => Repository.JsonCaseResolver;

        protected JsonResponseState State => _state;
        private JsonResponseState _state;

        public static BaseJsonHandler Init(JsonRepository repository, ref JsonResponseState state)
        {
            return new JsonPrepareRequestHandler()
                .SetRepository(repository)
                .SetState(ref state)
                .Handle();
        }
        
        public BaseJsonHandler Next<T>()
            where T : BaseJsonHandler, new()
        {
            if (!State.Continue) 
                return (T) this;
            
            return new T()
                .SetRepository(Repository)
                .SetState(ref _state)
                .Handle();
        }

        protected abstract BaseJsonHandler Handle();

        private BaseJsonHandler SetRepository(JsonRepository repository)
        {
            Repository = repository;
            return this;
        }
        
        private BaseJsonHandler SetState(ref JsonResponseState state)
        {
            _state = state;
            return this;
        }
    }
}
