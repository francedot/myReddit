using Xamarin.Forms;

namespace myReddit.Controls
{
    public class FontAwesomeIcon : Label
    {
        public const string Typeface = "FontAwesome";

        public FontAwesomeIcon()
        {
            FontFamily = Typeface;
        }

    }
}