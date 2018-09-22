using UnityEngine;
public class TextListItemSprite : MonoBehaviour
{
    public Sprite FACEBOOK  = null;
    public Sprite LIKE      = null;
    public Sprite YES       = null;
    public Sprite CUP       = null;
    public Sprite ACCOUNT   = null;
    public Sprite FLASH     = null;

    /**
        * On Destroy
        */
    public void OnDestroy()
    {
        this.FACEBOOK   = null;
        this.LIKE       = null;
        this.YES        = null;
        this.CUP        = null;
        this.ACCOUNT    = null;
        this.FLASH      = null;
    }
}