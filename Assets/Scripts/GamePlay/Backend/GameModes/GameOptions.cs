using UnityEngine;

public static class GameOptions {
    static bool _vibrate;

    static public bool Vibrate { get { return _vibrate; } }

    static public void LoadOptions()
    {
        _vibrate = !(PlayerPrefs.GetString("Vibrate") == "0");
    }
}
