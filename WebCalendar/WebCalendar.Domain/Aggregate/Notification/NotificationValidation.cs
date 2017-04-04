using System.Text.RegularExpressions;

namespace WebCalendar.Domain.Aggregate.Notification
{
    public static class NotificationValidation
    {
        public static bool ValidateType(string type)
        {
            return string.IsNullOrEmpty(type) || !Regex.IsMatch(type, Constants.TITLE_VALIDATION_REGEX)
                || type.Length > Constants.MAX_TITLE_LENGTH;
        }
    }
}
