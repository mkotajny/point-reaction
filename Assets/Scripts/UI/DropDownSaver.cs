using UnityEngine;
using UnityEngine.UI;

public class DropDownSaver : MonoBehaviour {

    public string keyName;
    public Text keyValue;
    public Dropdown dropDown; 

    public void Start()
    {
        GetDropdownValue();
    }

    public void SetDropdownValue()
    {
        PlayerPrefs.SetString(keyName, keyValue.text);
    }

    public void GetDropdownValue()
    {
        string dropDownValueText = PlayerPrefs.GetString(keyName);
        int dropDownValue;
        int.TryParse(dropDownValueText, out dropDownValue);
        dropDown.value = dropDownValue - 1;
    }
}
