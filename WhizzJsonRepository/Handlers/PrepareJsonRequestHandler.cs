using System.Linq;
using WhizzJsonRepository.Base;

namespace WhizzJsonRepository.Handlers
{
    public class PrepareJsonRequestHandler : QueryJsonHandler
    {
        protected override QueryJsonHandler Handle()
        {
            foreach (var (key, value) in State.OriginalRequest)
            {
                var newKey = ToDbCase(key);
                if (ColumnNames.Contains(newKey))
                    State.Request[newKey] = value;
            }

            return this;
        }
    }
}