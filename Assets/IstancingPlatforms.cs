using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IstancingPlatforms : MonoBehaviour {

    public GameObject PlatLeftPrefab;
    public GameObject PlatRigthPrefab;
    public GameObject firstLeft;
    public GameObject firstRight;
    public GameObject LevelBK;
    GameObject lastL;
    GameObject lastR;
    public float higth = 0.2f;
    public bool isLeft=true;
    int platN;
    public float speed;
    public float ranSpeed = 0.2f;
    public static IstancingPlatforms instance;
    public List<GameObject> platformsL;
    public List<GameObject> platformsR;

    void Awake () {
        instance = this;

    }

    private void Start()
    {
        speed = 0.5f;
        NewPlatform();
    }

    void InstanceLeftPrefab()
    {


        if (lastL)
            lastL = GameObject.Instantiate(PlatLeftPrefab, (lastL.transform.position + Vector3.up * higth * 2), PlatLeftPrefab.transform.rotation);
            else
            lastL = GameObject.Instantiate(PlatLeftPrefab, (firstLeft.transform.position + Vector3.up * higth * 2), firstLeft.transform.rotation);

        lastL.transform.SetParent(transform);
        lastL.transform.localScale = PlatLeftPrefab.transform.localScale;
        lastL.GetComponent<Animator>().SetFloat("speed", speed+Random.Range(-ranSpeed, ranSpeed));

        lastL.transform.GetComponentInChildren<Renderer>().material.color = GameManager.instance.bkColors[Random.Range(0, 20)]*0.6f;
        platN++;
        isLeft = !isLeft;
        lastL.name = "L_Pplat" + platN ;

        platformsL.Add(lastL);



    }


    void InstanceRigthPrefab()
    {


        if (lastR)
            lastR = GameObject.Instantiate(PlatRigthPrefab, (lastR.transform.position + Vector3.up * higth * 2), PlatRigthPrefab.transform.rotation);
            else
            lastR = GameObject.Instantiate(PlatRigthPrefab, (firstRight.transform.position + Vector3.up * higth * 2), firstRight.transform.rotation);

        lastR.transform.SetParent(transform);
        lastR.transform.localScale = PlatRigthPrefab.transform.localScale;
        lastR.GetComponent<Animator>().SetFloat("speed", speed + Random.Range(-ranSpeed, ranSpeed));
        lastR.transform.GetComponentInChildren<Renderer>().material.color = GameManager.instance.bkColors[Random.Range(0, 20)] * 0.6f; 
        platN++;
        isLeft = !isLeft;
        lastR.name = "R_Plat" + platN ;

        platformsR.Add(lastR);


    }



    public void NewPlatform () {

        if (!GameManager.instance.startGame) return;
        
            if (isLeft)
                InstanceLeftPrefab();
            else
                InstanceRigthPrefab();

        MoveBKG();

        speed += 0.003f;
        ranSpeed += 0.0035f;
        ranSpeed = Mathf.Clamp(0.1f, 0,6f);
    }

    void MoveBKG() {


        if (platN % 5 == 0)
            {
            LevelBK.transform.position = new Vector3(LevelBK.transform.position.x, GameManager.ballControl.transform.position.y, LevelBK.transform.position.z);
             }
            
         }


}
