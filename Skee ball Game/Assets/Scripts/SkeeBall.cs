using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeeBall : MonoBehaviour {

    GameManager gameManager;

    public GameObject newBallPos;
    public GameObject skeeball;

    public bool released = false;

    public Scrollbar powerBar;
    public bool loadingPower = false;
    public float loadSpeed = 2.0f;
    public float timeBegin = 0;
    public float maxForce = 1550;
    public float minForce = 950;
    public float force = 1000;
    public Rigidbody rb;
    public Vector3 tempSpeed;
    public int getScore;

    public float lifeTime = 5;

    // Start is called before the first frame update
    void Start() {
        gameManager = gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>(); ;
        gameManager.skeeball = this.gameObject;
        rb = GetComponent<Rigidbody>();
        powerBar.size = 0;
        released = false;
        GetComponent<SphereCollider>().enabled = true;
    }

    // Update is called once per frame
    void Update() {
        if (!released) {
            if (Input.GetKeyDown("a")) {
                timeBegin = Time.time;
                loadingPower = true;
            }
            if (Input.GetKeyUp("a")) {
                loadingPower = false;
                released = true;
                rb.AddForce(0, 0, force);
                //Destroy(this.gameObject, lifeTime);
            }
        }


        if (loadingPower) {
            LoadPower();
        }


    }

    private void LoadPower() {
        powerBar.size = 1 - Mathf.Abs(Mathf.Cos((Time.time - timeBegin) * loadSpeed));
        force = (maxForce - minForce) * powerBar.size + minForce;
    }


    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Hole") {
            GetComponent<SphereCollider>().enabled = false;
            getScore = int.Parse(other.gameObject.transform.GetComponentInChildren<TextMesh>().text);
            gameManager.score += getScore;
            Instantiate(skeeball, newBallPos.transform.position, newBallPos.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void PauseBall() {
        tempSpeed = rb.velocity;
        rb.velocity = new Vector3(0, 0, 0);
        rb.useGravity = false;
    }

    public void ResumeBall() {
        rb.velocity = tempSpeed;
        rb.useGravity = true;
    }

}
