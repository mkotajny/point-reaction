using UnityEngine;

public class ForTesters : MonoBehaviour {

    void OnEnable()
    {
        //ActivityLogger.AddLogLine("FOR TESTERS panel has been opened");
    }
        
    public void ActivityLogPreview()
    {
        //ActivityLogger.AddLogLine("Activity log has been opened");
        ActivityLogger.PreviewLog();
    }
}
