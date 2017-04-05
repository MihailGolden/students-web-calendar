﻿namespace WebCalendar.Domain
{
    public static class Constants
    {
        public const int MAX_TITLE_LENGTH = 80;
        public const int MAX_DESCRIPTION_LENGTH = 120;
        public const string TITLE_VALIDATION_REGEX = @"([ '-]?\p{L})+$";
    }
}