namespace MediRecords.Utility
{
	public static class Constant
	{
		public const string Admin = "Admin";

		public const string RegisterSuccess = "User Registered Succesfully";

		public const string RequestNull = "Registration request cannot be null.";

		public const string RequiredFields = "Required fields are missing or invalid.";

		public const string EmailExists = "A user with this email already exists.";

		public const string InternalError = "An internal server error occurred.";

		public const string InvalidEmailFormat = "The email address provided is not in a valid format.";

		public const string WeakPassword = "The password should contain atleast 8 characters which consists 1 uppercase, 1 lowercase, 1 number and 1 special character";

		public const string SaveFailed = "An error occurred while saving the user to the database.";

		public const string UserNotFound = "User with ID {0} not found.";

		public const string FetchAllUsersError = "Critical failure while retrieving the user list.";

		public const string FetchUserByIdError = "Failure while retrieving details for User ID: {0}";

		public const string Unassigned = "No Role Assigned";

		public const string InvalidUserId = "Invalid ID provided.";

		public const string InvalidRoleId = "Invalid RoleID.";

		public const string Active = "Active";

		public const string Inactive = "Inactive";

		public const string DeleteSucess = "User deleted Successfully";
		
		public static class UserUpdate
		{
			public const string UpdateUserRequest = "Update request cannot be null.";
			public const string InvalidUserId = "Invalid UserID.";
			public const string UserNotFound = "User not found.";
			public const string NameRequired = "Name is required.";
			public const string PhoneRequired = "Phone number is required.";
			public const string InvalidRoleId = "Invalid RoleID.";
			public const string UpdateFailed = "User update failed.";
		}
		public static class Messages
		{
			public const string InvalidRequest = "Invalid request.";
			public const string SomethingWentWrong = "Something went wrong. Please try again.";
			public const string Success = "Operation completed successfully.";
			public const string UserNotFound = "No account found with that email address.";
			public const string PasswordMismatch = "New Password and Confirm Password do not match.";
			public const string WeakPassword = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
			public const string PasswordUpdated = "Password updated successfully.";
			public const string EmailRequired = "Email is required.";
			public const string EmailInvalid = "Invalid email format.";
			public const string NewPasswordRequired = "New password is required.";
			public const string ConfirmPasswordRequired = "Confirm password is required.";
		}

		public static class EncounterMessages
		{
			public const string InvalidProviderId = "Invalid provider ID.";
			public const string InvalidEncounterId = "Invalid encounter ID.";
			public const string InvalidStatus = "Invalid encounter status value.";
			public const string EncounterNotFound = "Encounter not found.";
			public const string EncounterLocked = "This encounter is locked and cannot be modified.";
			public const string StatusUpdated = "Encounter status updated successfully.";
			public const string SomethingWentWrong = "Something went wrong. Please try again.";
		}
	}

}
