using FluentValidation.Results;

namespace NoteService.API.Error;

public static class ErrorFormat
{
    public static string[] Deserialize(IEnumerable<ValidationFailure> failures) =>
        failures.Select(x => x.ErrorMessage).ToArray();
}
