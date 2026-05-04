namespace MediRecords.Utility
{
	public static class Constant
	{
		public const string FrontDesk = "FrontDesk";

		public const string Nurse = "Nurse";

		public const string Physician = "Physician";

		public const string LabTech = "LabTech";

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

		public const string PatientIdRequired = "PatientId is doesnot exiat";
		public const string ProviderIdRequired = "ProviderId is doesnot exist";

		// Validation & Error Messages
		public const string RequestCannotBeNull = "Request body cannot be null.";
		public const string InvalidEncounterId = "Provide a valid EncounterId.";
		public const string EncounterNotFound = "The specified Encounter does not exist.";
		public const string EncounterClosed = "Cannot add imaging orders to a closed encounter.";

		// Success & General Responses
		public const string OrderCreated = "Order created successfully.";
		public const string InternalServerError = "An unexpected error occurred. Please try again later.";

		public const string OrderNotFound = "The specified Imaging Order does not exist.";

		// Report Messages
		public const string ReportCreated = "Imaging report created successfully.";
		public const string ReportAlreadyExists = "A report has already been submitted for this order.";
		public const string InvalidFindings = "Findings must contain at least one result.";

		public const string ProblemCreated = "Problem Added Successfully";

		public const string AllergyCreated = "Allergy Created Successfully";

		public const string MedicalHistoryCreated = "Medical History Created Successfully";
		
		public const string AllergyExists = "Allergy already exists for this patient.";

		public const string MedicalHistoryExists = "Medical history entry already exists.";

		public const string AllergenRequired = "Allergen is required.";

		public const string DiagnosisRequired = "Diagnosis is required.";

		public const string ConditionRequired = "Condition is required.";

		public const string StartDateValidation = "Start date cannot be in the future.";

		public const string EndDateValidation = "End date cannot be earlier than start date.";

		public const string ExceedLength = "Notes exceed allowed length.";

		public const string Notes = "Notes are required";

		public const string StudyType = "StudyType Required";

		public const string ImagingOrderAlreadyExists = "Imaging Order Already Exists";

		//Procdure code Constants

		public const string ProcedureNotFound = "procedure does not found";

		public const string ProcedureFound = "procedure already exists";

		public const string InvalidProcedureCode = "Please enter valid procedure code";

		public const string Description = "Please give the description";

		public const string InvalidPrice = "Price cannot be negative or zero";

		public const string ProcedureCreated = "Procedure Created Successfully";

		public const string ProcdureUpdated = "Procedure Updated Successfully";

		public static class UserUpdate
		{
			public const string UpdateUserRequest = "Update request cannot be null.";
			public const string InvalidUserId = "Invalid UserID.";
			public const string UserNotFound = "User not found.";
			public const string UserStatus = "User not found or User is already deleted";
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

		public static class PatientMessages
		{
			public const string RequestNull = "Request body cannot be null.";
			public const string RequiredFields = "Required Field are missing";
			public const string NameRequired = "Patient name is required.";
			public const string GenderRequired = "Gender is required.";
			public const string PhoneNoRequired = "Phone number is required.";
			public const string DOBRequired = "Date of birth is required.";
			public const string InvalidDOB = "Date of birth cannot be a future date.";
			public const string InvalidId = "Invalid PatientId";
			public const string PatientNotFound = "Patient not found";
			public const string ProviderNotFound = "Primary provider does not exist.";
			public const string DuplicatePatient = "Patient already exists with the given phone number and date of birth.";
			public const string InvalidProviderId = "The providerId is not a physician.";
			public const string NoSuchPrimaryProvider = "This patient does not have a primary provider assigned.";
			public const string UnauthorizedAccess = "You are not authorized to access this patient's records.";
		}

		public static class SOAPNoteMessages
		{
			public const string InvalidEncounterId = "Invalid encounter ID.";
			public const string EncounterNotFound = "Encounter not found.";
			public const string EncounterLocked = "This encounter is locked. SOAP note cannot be saved.";
			public const string HPIRequired = "HPI (History of Present Illness) is required.";
			public const string SOAPNoteSaved = "SOAP note saved as draft successfully.";
			public const string SOAPNoteSigned = "SOAP note signed and locked successfully.";
			public const string SOAPNoteNotFound = "SOAP note not found.";
			public const string SOAPNoteLocked = "This SOAP note is signed and locked. It cannot be modified.";
			public const string SomethingWentWrong = "Something went wrong. Please try again.";
		}

		public static class ImagingMessages
		{
			public const string InvalidImagingOrderId = "Invalid imaging order ID.";
			public const string ImagingOrderNotFound = "Imaging order not found.";
			public const string NoReportsFound = "No reports found for this imaging order.";
			public const string SomethingWentWrong = "Something went wrong. Please try again.";
		}
	}
}