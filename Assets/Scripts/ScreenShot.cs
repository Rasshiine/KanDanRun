using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    public static int cnt = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            ScreenCapture.CaptureScreenshot("image_SS" + cnt.ToString() + ".png");
            Debug.Log("image_SS" + cnt.ToString() + ".png");
            cnt++;
        }
    }
}
