namespace WebCalendar.Domain
{
    public static class Constants
    {
        public const int MAX_TYPE_LENGTH = 60;
        public const int MAX_TITLE_LENGTH = 80;
        public const int MAX_DESCRIPTION_LENGTH = 120;
        public const string TITLE_VALIDATION_REGEX = /*@"([ '-]?\p{L})+$"*/ @".*";
    }
    public enum Color { yellow = 1, green, blue, red }

}
