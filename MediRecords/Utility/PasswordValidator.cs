using System;
using System.Text.RegularExpressions;

namespace MediRecords.Utility;

public class PasswordValidator
{   
    public static void Validate(string password)
    {
        if (password.Length < 8)
            throw new ArgumentException("Password must be at least 8 characters long");

        if (!Regex.IsMatch(password, "[A-Z]"))
            throw new ArgumentException("Password must contain at least one uppercase letter");

        if (!Regex.IsMatch(password, "[0-9]"))
            throw new ArgumentException("Password must contain at least one number");
    }
}
