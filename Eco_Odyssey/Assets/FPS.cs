using UnityEngine;

[ExecuteInEditMode]
public class FPS : MonoBehaviour
{
    [SerializeField] private int targetFPS = 60;

    private void Start()
    {
        #if UNITY_EDITOR
        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0;
        #endif
    }
}
