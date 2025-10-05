namespace MarketPOS.Shared.Constants
{
    public static class Messages
    {
        // ✅ General Success Messages
        public static class Successfully
        {
            public const string Success = "Success";
            public const string Created = "Created";
            public const string Updated = "Updated";
            public const string Deleted = "Deleted";
            public const string SofteDeleted = "SofteDeleted";
            public const string Restored = "Restored";
        }

        // ✅ Common Errors
        public static class Error
        {
            public const string MappingError = "MappingError";
            public const string NotFound = "NotFound";
            public const string NotFounds = "NotFounds";
            public const string ValidationFailed = "ValidationFailed";
            public const string InvalidParameterFormat = "InvalidParameterFormat";
            public const string MissingParameter = "MissingParameter";
            public const string BadRequest = "BadRequest";
            public const string InternalError = "InternalError";
            public const string IdMismatch = "IdMismatch";
        }

        // ✅ Exception Middleware Titles
        public static class ExceptionTitles
        {
            public const string BusinessError = "BusinessError";
            public const string ValidationError = "ValidationError";
            public const string UnhandledError = "UnhandledError";
            public const string SomethingWentWrong = "Something went wrong.";
            public const string ModelISNotValide = "ModelISNotValide";
        }

        // ✅ Exception Types
        public static class ExceptionTypes
        {
            public const string UnauthorizedAccessException = "UnauthorizedAccessException";
            public const string ArgumentNullException = "ArgumentNullException";
            public const string ArgumentException = "ArgumentException";
            public const string DbUpdateException = "DbUpdateException";
            public const string TaskCanceledException = "TaskCanceledException";
            public const string TimeoutException = "TimeoutException";
            public const string AutoMapperMappingException = "AutoMapperMappingException";
            public const string InvalidOperationException = "InvalidOperationException";
        }

        // ✅ FluentValidation
        public static class Validation
        {
            public const string PleaseReviewInput = "Please review the input data.";
        }

        // ✅ Service-level Logging / Exception Messages
        public static class Service
        {
            public const string Mappingfailed = "Mappingfailed";
            public const string GetPagedFailed = "GetPagedFailed";
            public const string GetAllFailed = "GetAllFailed";
            public const string GetByIdFailed = "GetByIdFailed";
            public const string GetByNameFailed = "GetByNameFailed";
            public const string FindFailed = "FindFailed";
            public const string CreateFailed = "CreateFailed";
            public const string UpdateFailed = "UpdateFailed";
            public const string DeleteFailed = "DeleteFailed";
            public const string SofteDeletedFailed = "SofteDeletedFailed";
            public const string RestoreFailed = "RestoreFailed";
            public const string DuplicateCategoryName = "DuplicateCategoryName";
            public const string DuplicateBarcode = "DuplicateBarcode";
            public const string DuplicateActiveIngredinentName = "DuplicateActiveIngredinentName";
            public const string DuplicateWareHouseName = "DuplicateWareHouseName";
            public const string DuplicateProductName = "DuplicateProductName";
        }

        // ✅ Specific Validation Types
        public static class ValidationRules
        {
            public const string Invalid_Guid = "Invalid_Guid";
            public const string Invalid_PositiveInt = "Invalid_PositiveInt";
            public const string Invalid_NonEmptyString = "Invalid_NonEmptyString";
            public const string Invalid_Enum = "Invalid_Enum";
            public const string NoValues = "NoValues";
        }

        // ✅ Auth Service Messages
        public static class Auth
        {
            public const string EmailAlreadyRegistered = "EmailAlreadyRegistered";
            public const string UsernameAlreadyTaken = "UsernameAlreadyTaken";
            public const string ProfileImageRequired = "ProfileImageRequired";
            public const string RegistrationFailed = "RegistrationFailed";
            public const string RegistrationSuccessful = "RegistrationSuccessful";
            public const string InvalidCredentials = "InvalidCredentials";
            public const string LoginSuccessful = "LoginSuccessful";
            public const string InvalidToken = "InvalidToken";
            public const string UserNotFound = "UserNotFound";
            public const string InactiveToken = "InactiveToken";
            public const string TokenRefreshed = "TokenRefreshed";
        }
    }
}
