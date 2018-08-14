using UnityEngine;

public static class CheckInternet {
    public static bool IsConnected()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("debug: NOT connected with Internet .");
            
            return false;
        }
        else
        {
            Debug.Log("debug: Connected with Internet .");
            return true;
        }
    }

}
