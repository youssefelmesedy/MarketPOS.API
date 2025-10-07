namespace MarketPOS.Shared.Constants
{
    public static class AppMessages
    {
        // ✅ General Success Messages
        public const string Success = "Success";
        public const string Created = "Created";
        public const string Updated = "Updated";
        public const string Deleted = "Deleted";
        public const string SofteDeleted = "SofteDeleted";
        public const string Restored = "Restored";

        // ✅ Common Errors
        public const string NotFound = "NotFound";
        public const string NotFounds = "NotFounds";
        public const string ValidationFailed = "ValidationFailed";
        public const string InvalidParameterFormat = "InvalidParameterFormat";
        public const string MissingParameter = "MissingParameter";
        public const string BadRequest = "BadRequest";
        public const string InternalError = "InternalError";
        public const string IdMismatch = "IdMismatch";
        public const string InvalidIdFormat = "InvalidIdFormat";

        // ✅ Exception Middleware Titles
        public const string BusinessError = "BusinessError";
        public const string ValidationError = "ValidationError";
        public const string UnhandledError = "UnhandledError";
        public const string SomethingWentWrong = "Something went wrong.";
        public const string ModelISNotValide = "ModelISNotValide";

        // ✅ Exception Types
        public const string UnauthorizedAccessException = "UnauthorizedAccessException";
        public const string ArgumentNullException = "ArgumentNullException";
        public const string ArgumentException = "ArgumentException";
        public const string DbUpdateException = "DbUpdateException";
        public const string TaskCanceledException = "TaskCanceledException";
        public const string TimeoutException = "TimeoutException";
        public const string AutoMapperMappingException = "AutoMapperMappingException";
        public const string InvalidOperationException = "InvalidOperationException";

        // ✅ FluentValidation
        public const string PleaseReviewInput = "Please review the input data.";

        // ✅ Mapping Errors
        public const string MappingError = "MappingError";
        public const string Mappingfailed = "Mappingfailed";

        // ✅ Service-level Messages
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

        // ✅ Specific Validation Types
        public const string Invalid_Guid = "Invalid_Guid";
        public const string Invalid_PositiveInt = "Invalid_PositiveInt";
        public const string Invalid_NonEmptyString = "Invalid_NonEmptyString";
        public const string Invalid_Enum = "Invalid_Enum";
        public const string NoValues = "NoValues";

        // ✅ Auth Service Messages
        public const string UplodeImageFilde = "UplodeImageFilde";
        public const string InvalidRevokedToken = "InvalidRevokedToken";
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
        public const string LogoutSuccessful = "LogoutSuccessful";
        public const string LogoutFailed = "LogoutFailed";

        // ✅ Password & Email
        public const string PasswordResetFailed = "PasswordResetFailed";
        public const string PasswordResetSuccessful = "PasswordResetSuccessful";
        public const string OldPasswordMismatch = "OldPasswordMismatch";
        public const string PasswordChangeSuccessful = "PasswordChangeSuccessful";
        public const string EmailNotRegistered = "EmailNotRegistered";
        public const string PasswordResetEmailSent = "PasswordResetEmailSent";
        public const string InvalidResetToken = "InvalidResetToken";
    }
}