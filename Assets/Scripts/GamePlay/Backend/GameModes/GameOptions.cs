using UnityEngine;

public static class GameOptions {
    static public bool Vibrate { get; private set; }

    static public void LoadOptions()
    {
        Vibrate = (PlayerPrefs.GetString("Vibrate") == "1");
    }
}
