using System.Collections.Immutable;
using Newtonsoft.Json.Linq;
using WhizzSchema.Entities;

namespace WhizzORM.JsonValidate
{
    public class JsonResponseState
    {
        public JsonResponseState(JObject request, ImmutableArray<ColumnEntity> columns, MandatoryColumns mandatoryColumns, ImmutableArray<UniqueIndexEntity> uniqueIndexes = default, ImmutableArray<ForeignKeyEntity> foreignKeys = default)
        {
            OriginalRequest = request;
            Request = new JObject();
            Response = new JObject
            {
                ["success"] = true,
            };
            Columns = columns;
            UniqueIndexes = uniqueIndexes;
            ForeignKeys = foreignKeys;
            MandatoryColumns = mandatoryColumns;
            Response = new JObject {["success"] = true};
        }

        public JObject OriginalRequest { get; }
        public JObject Request { get; }
        public JObject Response { get; }
        public ImmutableArray<ColumnEntity> Columns { get; }
        public ImmutableArray<UniqueIndexEntity> UniqueIndexes { get; }
        public ImmutableArray<ForeignKeyEntity> ForeignKeys { get; }
        public MandatoryColumns MandatoryColumns { get; }
        public bool Continue { get; set; } = true;
        public bool Success => (bool) Response["success"];

        public void SetData(string data)
        {
            Response["data"] = data;
        }

        public void SetRowsAffected(int rowsAffected)
        {
            Response["rowsAffected"] = rowsAffected;
        }

        public void SetErrorMessage(string message)
        {
            Response["errorMessage"] = message;

            Response["success"] = false;
        }

        public void AddError(string propertyName, string message)
        {
            if (!Response.ContainsKey("errors"))
                Response["errors"] = new JObject();
            
            if (!((JObject) Response["errors"]).ContainsKey(propertyName))
                ((JObject) Response["errors"])[propertyName] = new JArray();
            
            ((JArray) ((JObject) Response["errors"])[propertyName]).Add(message);

            Response["success"] = false;        }

        public override string ToString()
        {
            return Response.ToString();
        }
    }
}