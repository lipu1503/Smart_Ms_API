namespace SmartManagement.Infrastructure.Result
{
    public class Result
    {
        public bool IsSucess { get; set; }

        public bool IsFailure => !IsSucess;
        public Response Response { get; set; }
        public Result(bool isSuccess, Response response)
        {
            if ((isSuccess && response != Response.Success) || (!isSuccess && response == Response.Success))
            {
                throw new ArgumentException("Invalid combination of isSuccess and Response Typr in result mode.");
            }
        }
        public static Result Success() => new Result(isSuccess: true, Response.Success);

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, isSuccess: true, Response.Success);
        }
        public static Result Failure(Response response)
        {
            return new Result(isSuccess: false, response);
        }
    }
    public class Result<TValue> : Result
    {
        public Result(TValue? value, bool isSuccess, Response response) : base(isSuccess, response)
        {
            _value = value;
        }

        public readonly TValue? _value;
        public TValue? Value => _value;

    }
}
