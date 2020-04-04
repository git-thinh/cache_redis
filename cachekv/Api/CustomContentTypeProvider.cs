using Microsoft.Owin.StaticFiles.ContentTypes;

namespace cachekv
{
    public class CustomContentTypeProvider : FileExtensionContentTypeProvider
    {
        public CustomContentTypeProvider()
        {
            Mappings.Add(".json", "application/json");
            Mappings.Add(".sql", "text/plain");
        }
    }
}
