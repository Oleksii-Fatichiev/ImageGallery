namespace ImageGallery.Contracts.Constants
{
    public static class Constants
    {
        public static string[] ImageFormats { get; } = { "png", "jpeg"};

        public static class ContentTypes
        {
            public const string JSON = "application/json";
        }

        public static class SwaggerOptions
        {
            public const string TITLE = "ImageGallery API";
        }

        public static class ApiVertions
        {
            public const string V_1_0 = "1.0";
        }
    }
}
