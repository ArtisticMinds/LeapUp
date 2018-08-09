using System.Collections;
using UnityEngine;
using System.IO;

public class NativeShare : MonoBehaviour
{
    public GameObject CanvasShareObj;

    private bool isProcessing = false;
    private bool isFocus = false;


    private void Start()
    {
        CanvasShareObj.SetActive(false);
    }
    public void ShareBtnPress()
    {
        if (!isProcessing)
        {
            CanvasShareObj.SetActive(true);
            StartCoroutine(ShareScreenshot());
        }
    }

    IEnumerator ShareScreenshot()
    {
        isProcessing = true;

        yield return new WaitForEndOfFrame();



        ScreenCapture.CaptureScreenshot("screenshot.png", 2);
        string destination = Path.Combine(Application.persistentDataPath, "screenshot.png");

        yield return new WaitForSecondsRealtime(0.3f);

        if (!Application.isEditor)
        {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"),
                uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"),
                "Can you beat my score?");
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            if (Application.systemLanguage.ToString() == "English")
            {
                AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
                    intentObject, "Share your new score");
                currentActivity.Call("startActivity", chooser);
            }
            if (Application.systemLanguage.ToString() == "Italian")
            {
                AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser",
    intentObject, "Condividi il punteggio");
                currentActivity.Call("startActivity", chooser);
            }

            yield return new WaitForSecondsRealtime(1);
        }

        StartCoroutine(Close()); //Chiudi comunque dopo 5 secondi (per sicurezza)


        yield return new WaitUntil(() => isFocus);
        CanvasShareObj.SetActive(false);
        isProcessing = false;


    }


    IEnumerator Close()
    {
        yield return new WaitForSecondsRealtime(5f);
        CanvasShareObj.SetActive(false);
        isProcessing = false;
    }

        private void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
}