using UnityEngine;
using UnityEngine.Advertisements;

public class VideoAD : MonoBehaviour
{
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");


                GameManager.instance.totalPoints += 50;
                PlayerPrefs.SetInt(GameManager.AppName + "totalPoint", GameManager.instance.totalPoints);
                GameManager.instance.totalCoinsText.text = GameManager.instance.totalPoints.ToString();
                GameManager.instance.totalCoinsText2.text = GameManager.instance.totalPoints.ToString();
                GameManager.instance.totalCoinsText3.text = GameManager.instance.totalPoints.ToString();
                GameManager.instance.AdButton.SetActive(false);

                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}