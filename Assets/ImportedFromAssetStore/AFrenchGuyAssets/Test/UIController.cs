using System.Collections.Generic;
using UnityEngine;
using Afrenchguy;

public class UIController : MonoBehaviour
{
    /**
     * The list animation
     */
    public TextsListAnimation Animation = null;

   public TextListItemSprite AllSprites = null;

    // Use this for initialization
    void Start()
    {
        List<TextListItem> items = new List<TextListItem>();

        items.Add( new TextListItem( "Join us on Facebook",/*this.AllSprites.FACEBOOK*/ null) );
        items.Add( new TextListItem( "Watch an ad and get rewarded", this.AllSprites.YES ) );
        items.Add( new TextListItem( "Do you like it?", this.AllSprites.LIKE ) );
        items.Add( new TextListItem( "A new msg without icon", null ) );
        items.Add( new TextListItem( null, this.AllSprites.ACCOUNT ) );
        items.Add( new TextListItem( "Get a cup by winning a lot of games", this.AllSprites.CUP ) );

        this.Animation.Init( items );
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
