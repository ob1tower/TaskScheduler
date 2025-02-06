using FluentValidation.Results;

namespace AuthService.API.Error;

public static class ErrorFormat
{
    public static string[] Deserialize(IEnumerable<ValidationFailure> failures) =>
        failures.Select(x => x.ErrorMessage).ToArray();
}
