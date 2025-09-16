namespace MarketPOS.API.Middlewares.FuatuersFunction
{
    public static class ErrorFunction
    {
        /// <summary>
        /// Creates a <see cref="NotFoundObjectResult"/> with the specified success status, message, and error details.
        /// </summary>
        /// <param name="IsSuccees">Indicates whether the operation was successful. Defaults to <see langword="false"/>.</param>
        /// <param name="Message">An optional message describing the result of the operation.</param>
        /// <param name="Errors">An optional object containing details about any errors that occurred.</param>
        /// <returns>A <see cref="NotFoundObjectResult"/> containing a <see cref="ResultDto{T}"/> object with the specified
        /// success status, message, and error details.</returns>
        public static NotFoundObjectResult NotFound(bool IsSuccees = false, string? Message = null, object? Errors = null) 
        {
            return new NotFoundObjectResult(new ResultDto<object>
            {
                IsSuccess = IsSuccees,
                Message = Message,
                Errors = Errors
            });
        }

        /// <summary>
        /// Creates a <see cref="BadRequestObjectResult"/> containing a standardized response structure.
        /// </summary>
        /// <param name="IsSuccees">Indicates whether the operation was successful. Defaults to <see langword="false"/>.</param>
        /// <param name="Message">An optional message providing additional context about the result. Can be <see langword="null"/>.</param>
        /// <param name="Errors">An optional object containing details about the errors. Can be <see langword="null"/>.</param>
        /// <returns>A <see cref="BadRequestObjectResult"/> containing a <see cref="ResultDto{T}"/> with the specified details.</returns>
        public static BadRequestObjectResult BadRequest(bool IsSuccees = false, string? Message = null, object? Errors = null)
        {
            return new BadRequestObjectResult(new ResultDto<object>
            {
                IsSuccess = IsSuccees,
                Message = Message,
                Errors = Errors
            });
        }
        /// <summary>
        /// Creates a <see cref="ConflictObjectResult"/> representing a conflict response with the specified success
        /// status, message, and errors.
        /// </summary>
        /// <param name="IsSuccees">Indicates whether the operation was successful. Defaults to <see langword="false"/>.</param>
        /// <param name="Message">An optional message describing the result of the operation. Can be <see langword="null"/>.</param>
        /// <param name="Errors">An optional object containing details about the errors. Can be <see langword="null"/>.</param>
        /// <returns>A <see cref="ConflictObjectResult"/> containing a <see cref="ResultDto{T}"/> with the specified success
        /// status, message, and errors.</returns>
        public static ConflictObjectResult ConflictRequest(bool IsSuccees = false, string? Message = null, object? Errors = null)
        {
            return new ConflictObjectResult(new ResultDto<object>
            {
                IsSuccess = IsSuccees,
                Message = Message,
                Errors = Errors
            });
        }
    }
}
