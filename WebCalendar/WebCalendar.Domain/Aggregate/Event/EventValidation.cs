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
            if (description == null)
                return false;

            return !Regex.IsMatch(description, Constants.DESCRIPTION_VALIDATION_REGEX)
                       || description.Length > Constants.MAX_DESCRIPTION_LENGTH;
        }
    }
}
