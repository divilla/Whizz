using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace WhizzJsonRepository.Base
{
    public class JsonResponse
    {
        public JsonResponse()
        {
            Success = true;
            Code = 200;
            Message = "OK";
            Continue = true;
        }

        public bool Success { get; private set; }
        public short Code { get; private set; }
        public string Message { get; private set; }
        public string Data { get; private set; }
        public int? RowsAffected { get; private set; }
        public readonly Dictionary<string, List<string>> Errors = new Dictionary<string, List<string>>();
        public bool Continue { get; set; }

        public void SetData(string data)
        {
            Data = data;
        }

        public void SetRowsAffected(int rowsAffected)
        {
            RowsAffected = rowsAffected;
        }

        public void SetBadRequestError(string message = null)
        {
            SetErrorMessage(400, string.IsNullOrEmpty(message) ? "Bad Request" : message);
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

        public void SetValidationFailedError()
        {
            SetErrorMessage(422, "Validation Failed");
        }

        public void SetDatabaseException(string message)
        {
            SetErrorMessage(500, $"Database exception: {message}");
        }

        public void SetErrorMessage(short code, string message)
        {
            Success = false;
            Code = code;
            Message = message;
        }

        public void AddError(string propertyName, string message)
        {
            if (!Errors.ContainsKey(propertyName))
                Errors[propertyName] = new List<string>();
            
            Errors[propertyName].Add(message);
            SetValidationFailedError();
        }

        public override string ToString()
        {
            if (Success && RowsAffected == null)
                return Data;
            
            if (Success)
            {
                var result = new JObject();
                if (!string.IsNullOrEmpty(Data))
                    result["data"] = JObject.Parse(Data);
                if (RowsAffected != null)
                    result["rowsAffected"] = RowsAffected;

                return result.ToString();
            }
            else
            {
                var result = new JObject
                {
                    ["success"] = Success,
                    ["code"] = Code,
                    ["message"] = Message,
                };
                if (Errors.Count > 0)
                    result["errors"] = new JObject(Errors);

                return result.ToString();
            }
        }
    }
}