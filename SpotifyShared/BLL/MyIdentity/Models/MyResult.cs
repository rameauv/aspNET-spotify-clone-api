using System.Globalization;

namespace Spotify.Shared.MyIdentity.Models;

public class MyResult
{
    private static readonly MyResult _success = new MyResult { Succeeded = true };
    private readonly List<MyError> _errors = new List<MyError>();

    public bool Succeeded { get; protected set; }

    public IEnumerable<MyError> Errors => _errors;

    public static MyResult Success => _success;

    public static MyResult Failed(params MyError[]? errors)
    {
        var result = new MyResult { Succeeded = false };
        if (errors != null)
        {
            result._errors.AddRange(errors);
        }

        return result;
    }

    public override string ToString()
    {
        return Succeeded
            ? "Succeeded"
            : string.Format(CultureInfo.InvariantCulture, "{0} : {1}", "Failed",
                string.Join(",", Errors.Select(x => x.Code).ToList()));
    }
}