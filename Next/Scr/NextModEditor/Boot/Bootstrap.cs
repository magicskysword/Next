using UnityEngine;

namespace SkySwordKill.NextModEditor.Boot;

public class Bootstrap : MonoBehaviour
{
    public const string EDITOR_VERSION = "0.2.0";
    private void Start()
    {
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = Screen.currentResolution.refreshRate;

        // try
        // {
        //     ModMgr.I.Init();
        // }
        // catch (Exception e)
        // {
        //     Debug.LogException(e);
        // }
    }
}