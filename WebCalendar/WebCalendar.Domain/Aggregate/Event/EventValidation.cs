using System.Text.RegularExpressions;

namespace WebCalendar.Domain.Aggregate.Event
{
    public static class EventValidation
    {
        public static bool ValidateTitle(string title)
        {
            return string.IsNullOrEmpty(title) || !Regex.IsMatch(title, Constants.TITLE_VALIDATION_REGEX)
                || title.Length > Constants.MAX_TITLE_LENGTH;
        }
        public static bool ValidateDescription(string description)
        {
            return string.IsNullOrEmpty(description) || !Regex.IsMatch(description, Constants.TITLE_VALIDATION_REGEX);
        }
    }
}
