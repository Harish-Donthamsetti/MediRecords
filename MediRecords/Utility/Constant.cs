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

		public const string WeakPassword = "The password does not meet the security requirements.";

		public const string SaveFailed = "An error occurred while saving the user to the database.";

        public const string UserNotFound = "User with ID {0} not found.";

        public const string FetchAllUsersError = "Critical failure while retrieving the user list.";

        public const string FetchUserByIdError = "Failure while retrieving details for User ID: {0}";

        public const string Unassigned = "No Role Assigned";

        public const string InvalidUserId = "Invalid ID provided.";


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
	}

}
