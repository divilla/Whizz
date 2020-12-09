using Newtonsoft.Json.Linq;

namespace WhizzJsonRepository.Base
{
    public class JsonResponseState
    {
        public JsonResponseState(JObject request)
        {
            OriginalRequest = request;
            Request = new JObject();
            Response = new JObject
            {
                ["success"] = true,
                ["code"] = 200,
            };
            Response = new JObject {["success"] = true};
        }

        public JObject OriginalRequest { get; }
        public JObject Request { get; }
        public JObject Response { get; }
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

        public void SetBadRequestError()
        {
            SetErrorMessage(400, "Bad Request");
        }

        public void SetUnauthorizedError()
        {
            SetErrorMessage(401, "Unauthorized");
        }

        public void SetForbiddenError()
        {
            SetErrorMessage(403, "Forbidden");
        }

        public void SetNotFoundError()
        {
            SetErrorMessage(404, "Not Found");
        }

        public void SetErrorMessage(int code, string message)
        {
            Response["code"] = code;
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