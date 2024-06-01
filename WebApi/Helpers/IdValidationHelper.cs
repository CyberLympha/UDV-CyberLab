using MongoDB.Bson;

namespace WebApi.Helpers;

public class IdValidationHelper
{
    public ApiOperationResult EnsureValidId(string id, string errorMessage = "Неверный формат id")
    {
        if (!ObjectId.TryParse(id, out _))
            return Error.BadRequest(errorMessage);
        return ApiOperationResult.Success();
    }
}