namespace MarketPOS.API.Middlewares.FeaturesFunction;
public static class HelperMethod
{
    /// <summary>
    /// Converts a <see cref="ResultDto{T}"/> into a standardized <see cref="IActionResult"/> 
    /// for general API responses (e.g., queries, simple commands).
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="result">
    /// The operation result object containing success flag, message, errors, and data.
    /// </param>
    /// <param name="localizer">
    /// The localization service used to translate error or conflict messages.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing:
    ///  - <c>400 BadRequest</c> if validation errors exist or invalid Guid is returned,  
    ///  - <c>404 NotFound</c> if data is null or empty,  
    ///  - <c>409 Conflict</c> if a duplicate entry is detected,  
    ///  - <c>200 OK</c> if the operation succeeds and data is valid.  
    /// </returns>
    public static IActionResult HandleResult<T>(ResultDto<T> result, IStringLocalizer localizer)
    {
        if (result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data == null ||
            (result.Data is IEnumerable<object> list && !list.Any()))
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

        if (result.Message!.Equals(localizer["DuplicateActiveIngredinentName"]))
            return ErrorFunction.ConflictRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data is Guid guidValue && guidValue == Guid.Empty)
            return ErrorFunction.BadRequest(false, result.Message, result.Errors);

        return new OkObjectResult(result);
    }

    /// <summary>
    /// Handles a <see cref="ResultDto{T}"/> and converts it into a proper <see cref="IActionResult"/> 
    /// with standardized API responses for Create/Update scenarios.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="result">The operation result returned from the command or query.</param>
    /// <param name="actionName">The name of the action method used to generate the route for CreatedAtAction.</param>
    /// <param name="controllerName">The name of the controller where the action is defined.</param>
    /// <param name="getByIdFunc">
    /// A function used to retrieve the entity by its Guid. 
    /// Useful in update scenarios to return the updated entity.
    /// </param>
    /// <param name="localizer">The localization service for error and success messages.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> representing:
    ///  - <c>400 BadRequest</c> if validation errors exist or data is invalid,
    ///  - <c>404 NotFound</c> if the resource cannot be found,
    ///  - <c>409 Conflict</c> if a duplicate entry is detected,
    ///  - <c>201 Created</c> for successful creation,
    ///  - <c>200 OK</c> for successful update.
    /// </returns>
    public static async Task<IActionResult> HandleCreatedResult<T>(
        ResultDto<T> result,
        string actionName,
        Func<Guid, Task<object?>> getByIdFunc,
        IStringLocalizer localizer)
    {
        if (result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data == null)
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

        if (result.Message!.Equals(localizer["DuplicateActiveIngredinentName"]))
            return ErrorFunction.ConflictRequest(result.IsSuccess, result.Message, result.Errors);

        if (result.Data is Guid guidValue)
        {
            if (guidValue == Guid.Empty)
                return ErrorFunction.BadRequest(false, result.Message, result.Errors);

            var createdItem = await getByIdFunc(guidValue);

            return new CreatedAtActionResult(
                actionName,
                null,
                new { id = guidValue, Softdeleted = false },
                createdItem
            );
        }

        return new CreatedAtActionResult(
            actionName,
            null,
            null,
            result
        );
    }

    /// <summary>
    /// Handles a <see cref="ResultDto{T}"/> and converts it into a proper <see cref="IActionResult"/> 
    /// with standardized API responses.
    /// </summary>
    /// <typeparam name="T">The type of the result data.</typeparam>
    /// <param name="result">The operation result to handle.</param>
    /// <param name="getByIdFunc">
    /// Optional function to retrieve an entity by its Guid (mainly used for Create/Update scenarios).
    /// </param>
    /// <param name="localizer">The localization service for error and success messages.</param>
    /// <returns>An <see cref="IActionResult"/> that represents the standardized API response.</returns>
    public static async Task<IActionResult> ProcessResultAsync<T>(
        ResultDto<T> result,
        Func<Guid, Task<object?>> getByIdFunc,
        IStringLocalizer localizer)
    {
        // 1. لو فيه Errors
        if (result.Errors is not null)
            return ErrorFunction.BadRequest(result.IsSuccess, result.Message, result.Errors);

        // 2. لو مفيش Data
        if (result.Data == null)
            return ErrorFunction.NotFound(result.IsSuccess, result.Message, result.Errors);

        // 3. لو اسم مكرر
        if (result.Message!.Equals(localizer["DuplicateActiveIngredinentName"]))
            return ErrorFunction.ConflictRequest(result.IsSuccess, result.Message, result.Errors);

        // 4. لو Data عبارة عن Guid (يعني بيرجع Id للكيان اللي اتعدل)
        if (result.Data is Guid guidValue)
        {
            if (guidValue == Guid.Empty)
                return ErrorFunction.BadRequest(false, result.Message, result.Errors);

            var updatedItem = await getByIdFunc(guidValue);
            if (updatedItem is null)
                return ErrorFunction.NotFound(false, localizer["NotFound"], null);

            // ✅ Update = 200 OK
            return new OkObjectResult(updatedItem);
        }

        // 5. Fallback (لو Data مش Guid مثلاً)
        return new OkObjectResult(result.Data);
    }
}
