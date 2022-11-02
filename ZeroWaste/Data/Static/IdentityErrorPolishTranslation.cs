namespace ZeroWaste.Data.Static
{
    public static class IdentityErrorPolishTranslation
    {
        public static string TranslateErrorCodeToPolish(this string Code)
        {
            switch (Code)
            {
                case "PasswordToShort":
                    return "Hasło musi składać się conajmniej z 6 znaków.";
                case "PasswordRequiresNonAlphanumeric":
                    return "Hasło musi zawierać conajmniej jeden znak specjalny.";
                case "PasswordRequiresDigit":
                    return "Haslo musi zawierać conajmniej jedną cyfrę.";
                case "PasswordMismatch":
                    return "Obecne hasło jest nieprawidłowe.";
                case "PasswordRequiresUpper":
                    return "Hasło musi zawierać conajmniej jedną wielką literę.";
                case "PasswordRequiresLower":
                    return "Haslo musi zawierać conajmniej jedną małą literę.";
                default:
                    return Code;
            }
        }
    }
}
