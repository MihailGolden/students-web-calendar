using System.Text.RegularExpressions;

namespace WebCalendar.Domain.Aggregate.Calendar
{
    public static class CalendarValidation
    {
        public static bool ValidateTitle(string title)
        {
            return string.IsNullOrEmpty(title) || !Regex.IsMatch(title, Constants.TITLE_VALIDATION_REGEX)
                || title.Length > Constants.MAX_TITLE_LENGTH;
        }
        public static bool ValidateDescription(string description)
        {
            return string.IsNullOrEmpty(description) || !Regex.IsMatch(description, Constants.TITLE_VALIDATION_REGEX)
                || description.Length > Constants.MAX_DESCRIPTION_LENGTH;
        }
    }
}
