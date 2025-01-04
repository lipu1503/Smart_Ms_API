namespace SmartManagement.Infrastructure.Result
{
    public record Response
    {
        public string Code { get;  }
        public string Description { get;} 
        public ResponseType Type { get;}
        public static readonly Response Success = new Response(string.Empty,string.Empty,ResponseType.Success);
        public Response(string code,string description,ResponseType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }
        public static Response Exception(string code, string description)
        {
            return new Response(code,description,ResponseType.Exception);
        }
        public static Response ResourceNotFound(string code, string description)
        {
            return new Response(code, description, ResponseType.ResourceNotFound);
        }
        public static Response InvalidInput(string code, string description)
        {
            return new Response(code, description, ResponseType.InvalidInput);
        }
        public static Response DuplicateResource(string code, string description)
        {
            return new Response(code, description, ResponseType.DuplicateResource);
        }
        public static Response BusinessRuleFailure(string code, string description)
        {
            return new Response(code, description, ResponseType.BusinessRuleFailure);
        }
        public static Response Unauthorized(string code, string description)
        {
            return new Response(code, description, ResponseType.UnAuthorized);
        }
    }
}
