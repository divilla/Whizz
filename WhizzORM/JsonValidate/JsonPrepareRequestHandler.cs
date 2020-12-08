using System.Linq;

namespace WhizzORM.JsonValidate
{
    public class JsonPrepareRequestHandler : BaseJsonHandler
    {
        protected override BaseJsonHandler Handle()
        {
            foreach (var (key, value) in State.OriginalRequest)
            {
                var newKey = DbCaseResolver(key);
                if (State.Columns.Any(q => q.ColumnName == key))
                    State.Request[newKey] = value;
            }

            return this;
        }
    }
}