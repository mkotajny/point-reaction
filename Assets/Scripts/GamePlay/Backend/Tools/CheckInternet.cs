using UnityEngine;

public static class CheckInternet {
    public static bool IsConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;

        return true;
    }

}
