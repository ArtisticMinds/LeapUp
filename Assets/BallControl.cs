using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallControl : MonoBehaviour {

    public Rigidbody rig;
    public float jumpForce=3;
    public bool aTouch;
    bool jumped;
    public bool oNground;
    int points;
    public bool dead;
    public PhysicMaterial normalPhisMat;
    public PhysicMaterial onDeadPhisMat;
    static Transform lastPlatTouched;
    int left;
    AudioSource jumpAudio;

    void Awake () {
        points = 0;

        // GetComponent<SphereCollider>().material = normalPhisMat;
        jumpAudio = GetComponent<AudioSource>();
    }


    private void Start()
    {
        GameManager.instance.pointText.text = "0";
        dead = false;
        GameManager.instance.deadPanel.SetActive(false);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("platform"))
            oNground = false;
    }

    void ControlsUpdate()
    {
        aTouch = false;
        aTouch = Input.GetMouseButton(0);



    }

    public void Thouch() {


        if (oNground)
        {
            if (IstancingPlatforms.instance.isLeft) left = -1; else left = 1;
            transform.SetParent(null);
            rig.AddForce((Vector3.up * jumpForce * 10)+ (Vector3.left* left));
            rig.AddRelativeTorque(new Vector3(Random.Range(0.01f, -0.01f), Random.Range(0.01f, -0.01f), Random.Range(0.05f, -0.05f)));
            jumped = true;
            jumpAudio.Play();
            IstancingPlatforms.instance.NewPlatform();
            oNground = false;
            points++;
            GameManager.instance.pointText.text = points.ToString();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag.Equals("platform"))
        {
            
            if (other.transform.Equals(lastPlatTouched) && jumped) {
                print("DEAD for lastPlatTouched");
                Dead();
                return;
            }
            jumped = false;
            oNground = true;
            lastPlatTouched = other.transform;
            transform.SetParent(other.transform.parent.parent);
            return;
        }

        if (other.tag.Equals("deadTrigger"))
        {
            print("DEAD");
            Dead();
        }


            
    }


    void Dead() {

        dead = true;

        //GetComponent<SphereCollider>().material = onDeadPhisMat;
        GameManager.instance.deadPanel.SetActive(true);

        if (points>PlayerPrefs.GetInt(GameManager.AppName + "bestPoint"))
        {
            PlayerPrefs.SetInt(GameManager.AppName + "bestPoint",points);

        }
        GameManager.instance.totalPoints = PlayerPrefs.GetInt(GameManager.AppName + "totalPoint")+ points;
        PlayerPrefs.SetInt(GameManager.AppName + "totalPoint", GameManager.instance.totalPoints);
        GameManager.instance.totalCoinsText.text = GameManager.instance.totalPoints.ToString();
        GameManager.instance.totalCoinsText2.text = GameManager.instance.totalPoints.ToString();
        GameManager.instance.totalCoinsText3.text = GameManager.instance.totalPoints.ToString();


        GameManager.instance.EndGame(points);
        GameManager.instance.startGame = false;


        //foreach (GameObject plats in IstancingPlatforms.instance.platforms)
        //{
        //  Destroy(plats);
        //}
    }

    void Update()
    {


        if (!GameManager.instance.startGame) return;
        if (oNground && !dead) rig.angularVelocity = Vector3.zero;


    }


}
