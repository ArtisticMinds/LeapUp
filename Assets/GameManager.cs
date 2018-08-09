using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour
{


    public static string AppName = "LeapUp";
    public Text pointText;
    public Text sharePointText;
    public static BallControl ballControl;
    public GameObject deadPanel;
    public GameObject newBallsPanel;
    public GameObject creditsPanel;
    public GameObject fadeOutPanel;
    public static GameManager instance;
    public GameObject[] ballPrefabs;
    public string selectedBallString;
    public GameObject selectedBall;
    public int bestPoint;
    public bool startGame;
    public GameObject mainMenu;
    public GameObject Level;
   // public Text endGamePointsText;
    public Text endGamePointsText2;
    public Text endGameBestPointsText;
    public Text endGameBestPointsText2;
    public int totalPoints;
    public Text totalCoinsText;
    public Text totalCoinsText2;
    public Text totalCoinsText3;
    public Text buildVersionText;
    public Image colorImage;
    public Light dirLight;
    public Color[] bkColors;
    Color col;
    Color targetCol;
    int i;
    public Behaviour HQEffect;
    public GameObject selectionImage;
    public Button[] ballsButtons;
    public GameObject noCoinsMessage;
    float myTimer;
    public static AudioSource audioMusic;
    public AudioClip[]musics;
    static int restartInt; //Quante volte è stato restartato
    static bool isRestart;
    public GameObject AdButton;

    public GameObject pausePanel;
    public Text pauseTimeUI;
    int pause = 3;
    float pauseTimer;
    bool startPauseTimer;

    void Awake()
    {
        if(!instance)instance = this;
        //PlayerPrefs.DeleteAll();
        Level.SetActive(false);
        startGame = false;
        audioMusic = GetComponent<AudioSource>();
        mainMenu.SetActive(true);
        deadPanel.SetActive(false);
        newBallsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        noCoinsMessage.SetActive(false);
        fadeOutPanel.SetActive(false);
        pausePanel.SetActive(false);
        audioMusic.clip = musics[Random.Range(0, musics.Length)];
        Screen.sleepTimeout = (int)0f;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
      //  Screen.SetResolution(640, 480, true);
        Application.targetFrameRate = 60;

    }

    void Start()
    {
        System.GC.Collect();
        if (FindObjectOfType<BallControl>())
            Destroy(FindObjectOfType<BallControl>().gameObject);

        col = bkColors[0];
        UpdatePoints();

        //carica palla scelta
        if (PlayerPrefs.HasKey(AppName + "selectedBallString"))
        {
            selectedBallString = PlayerPrefs.GetString(AppName + "selectedBallString");
            foreach (Button bb in ballsButtons)
            {


                if (PlayerPrefs.HasKey(AppName + bb.name))
                    bb.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                else
                    bb.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.1f, 0.2f);


                if (bb.name.Equals(selectedBallString))
                    selectionImage.transform.position = bb.transform.position;

            }

        }
        else
        {
            selectedBallString = "Ball01";
            PlayerPrefs.SetString(AppName + "selectedBallString", selectedBallString);
            PlayerPrefs.SetInt(AppName + "Ball01", 1);

            foreach (Button bb in ballsButtons)
            {


                if (PlayerPrefs.HasKey(AppName + bb.name))
                    bb.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                else
                    bb.GetComponent<Image>().color = new Color(0.2f, 0.2f, 0.1f, 0.2f);


                if (bb.name.Equals(selectedBallString))
                    selectionImage.transform.position = bb.transform.position;

            }
        }

        if (Random.Range(0, 3) == 0)
        AdButton.SetActive(false);

        foreach (GameObject balls in ballPrefabs)
        {
            if (balls.name.Equals(selectedBallString))
            {
                selectedBall = Instantiate(balls);
            }
        }


        selectedBall.gameObject.SetActive(false);


        ballControl = selectedBall.GetComponent<BallControl>();
        FindObjectOfType<SmootFollow>().target = selectedBall.transform;

        buildVersionText.text = "v." + (Application.version);

        ballControl.dead = false;



        if (PlayerPrefs.HasKey(AppName + "HI"))
            {
            if (PlayerPrefs.GetInt(AppName + "HI").Equals(1))
            {
                HQEffect.enabled = true;
                dirLight.shadows = LightShadows.Hard;
            }
            else
            {
                HQEffect.enabled = false;
                dirLight.shadows = LightShadows.None;

            }
            }
            else
            {
            if (!isRestart)
                StartCoroutine(RescaleQuality());
            }



        if (isRestart) //Se è un restart esegui il gioco all'avvio
        {
            restartInt++;


            if (restartInt == 3 || restartInt == 5 || restartInt > 6)
            {
                Advertisement.Show();
            }

            StartGame();

            print(restartInt);
        }


        audioMusic.Play();

    }


   public void ShowVideoForReward() { }

    IEnumerator RescaleQuality()
    {
        yield return new WaitForSeconds(3);

        if (ExplosionsFPS.fps > 40)
        {
            HQEffect.enabled = true;
            dirLight.shadows = LightShadows.Hard;
            PlayerPrefs.SetInt(AppName + "HI", 1);
        }
        else
        {
            HQEffect.enabled = false;
            dirLight.shadows = LightShadows.None;
            PlayerPrefs.SetInt(AppName + "HI", 0);
        }
    }

 


        public void Restart()
    {
        fadeOutPanel.SetActive(true);
        isRestart = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    IEnumerator Reload() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame(int points)
    {
        startGame = false;
        endGamePointsText2.text = points.ToString();
        sharePointText.text = points.ToString();
        endGameBestPointsText.text= PlayerPrefs.GetInt(AppName + "bestPoint").ToString();       
        DisableLevel();
    }

    void DisableLevel()
    {
   
        Level.SetActive(false);
       // selectedBall.gameObject.SetActive(false);
       Destroy(selectedBall);
    }
     void StartGame()
    {
        ballControl.dead = false;
        startGame = true;
        mainMenu.SetActive(false);
        Level.SetActive(true);
        selectedBall.gameObject.SetActive(true);
        isRestart = false;
        targetCol = bkColors[i];
    }


    public void QuitGame() {
        Application.Quit();
    }

    public void Thouch()
    {
        if (Time.timeScale > 0)
            ballControl.Thouch();
    }

    public void ReturnToMenu()
    {
        startGame = false;
        mainMenu.SetActive(true);
        Level.SetActive(false);
       // selectedBall.gameObject.SetActive(false);
        mainMenu.SetActive(true);
        deadPanel.SetActive(false);
        isRestart = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            Time.timeScale = 0;
            pausePanel.SetActive(true);
            pauseTimeUI.gameObject.SetActive(false);
        }
        else {
            pauseTimer = 4;
            pauseTimeUI.text = "3";
            startPauseTimer = true;
            pauseTimeUI.gameObject.SetActive(true);

        }
    }



    public void OpenCredits()
    {
       creditsPanel.SetActive(true);
    }

    void UpdatePoints() {


        if (PlayerPrefs.HasKey(AppName + "totalPoint"))
            totalPoints = PlayerPrefs.GetInt(AppName + "totalPoint");

        if (PlayerPrefs.HasKey(AppName + "bestPoint"))
            bestPoint = PlayerPrefs.GetInt(AppName + "bestPoint");

        
        endGameBestPointsText2.text = bestPoint.ToString();




        totalCoinsText.text = totalPoints.ToString();
        totalCoinsText2.text = totalPoints.ToString();
        totalCoinsText3.text = totalPoints.ToString();

    }

    private void OnApplicationQuit()
    {
        isRestart = false;
    }


    public void OpenNewBallsPanel()
    {
        newBallsPanel.SetActive(true);
    }

    void ChangeTargetColor()
    {
        myTimer += Time.deltaTime;

        col = Color.Lerp(col, targetCol, Time.deltaTime * 0.15f);
        colorImage.color= dirLight.color = col;


        if (myTimer >= 5)
        {
            if (i == bkColors.Length - 1)
                i = 0;
            else
                i++;

            targetCol = bkColors[i];

            myTimer = 0;
        }

    }




    public void BallSelection(GameObject ball)
    {
        if (PlayerPrefs.HasKey(AppName + ball.name)) //Se acquistata
        {

            selectionImage.transform.position = ball.transform.position;
            selectedBallString = ball.name;
            PlayerPrefs.SetString(AppName + "selectedBallString", selectedBallString);


        }
        else
        {

            TryToUlockBall(ball);

        }


    }

    void TryToUlockBall(GameObject ball)
    {
        if (totalPoints >= 500)
        {
            PlayerPrefs.SetInt(AppName + ball.name, 1);
            ball.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            totalPoints -= 500;
            PlayerPrefs.SetInt(AppName + "totalPoint", totalPoints);
            totalCoinsText.text = totalPoints.ToString();
            totalCoinsText2.text = totalPoints.ToString();
            totalCoinsText3.text = totalPoints.ToString();
            BallSelection(ball);
        }
        else {
            noCoinsMessage.SetActive(true);
        }
    }









    void Update() {
        if (ballControl.dead)
            GameManager.audioMusic.volume -= 0.1f * Time.deltaTime;
        else
        ChangeTargetColor();

        if (startPauseTimer)
        {

            pauseTimer -= Time.unscaledDeltaTime;
            if (pauseTimer < 3.2f)
                pauseTimeUI.text = Mathf.RoundToInt(pauseTimer).ToString();


            if (pauseTimer<=0) //endPause
            {
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                startPauseTimer = false;
                pauseTimer = 4;
            }
        }
    }









}
