using UnityEngine;
namespace Afrenchguy
{
    public class TextListItem
    {
        /********************************************************************************/

        /**
         * The text
         */
        public string Text = null;

        /**
         * The item type
         */
        public Sprite Sprite = null;

        /********************************************************************************/

        /**
         * Constructor
         */
        public TextListItem(string text, Sprite sprite)
        {
            this.Text   = text;
            this.Sprite = sprite;
        }
    }
}