using UnityEngine;
using System.Collections;

namespace Sauce3D
{
    public class ButtonOpenURL : MonoBehaviour
    {
        public string url = "http://3dsauce.com/";

        void OpenUrl()
        {
            Application.OpenURL(url);
        }
    }
}