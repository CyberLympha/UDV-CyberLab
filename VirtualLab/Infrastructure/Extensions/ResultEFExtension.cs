using FluentResults;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.ApiResult;

namespace ProxmoxApi;

public static class ResultEfExtension
{
    public static Result<TEntity> AssertExistsOrFail<TEntity>(this TEntity? entity ) // а нужен ли он?? проще ли читается?
    {
        return entity != null ? 
            Result.Ok(entity) :
            Result.Fail(ApiResultError.NotFound(typeof(TEntity).Name));
    } 
}

