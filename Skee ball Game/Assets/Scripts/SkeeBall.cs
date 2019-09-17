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
    public float newBallDist = 35f;

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
        if (gameManager.gameStage == GameManager.GameStage.Gameplay) {
            if (!released) {
                TouchControl();
            } else {
                BallControl();
            }
       
            if (loadingPower) {
                LoadPower();
            }
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

    // Touch Control
    public void TouchControl() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase) {
                case TouchPhase.Began:
                    timeBegin = Time.time;
                    loadingPower = true;
                    break;

                case TouchPhase.Moved:
                    float dist = Distance(Camera.main.transform.position, newBallPos.transform.position);
                    Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, dist);
                    Vector3 objPos = Camera.main.ScreenToWorldPoint(touchPosition);
                    objPos.y = newBallPos.transform.position.y;
                    objPos.z = newBallPos.transform.position.z;
                    transform.position = objPos;

                    break;
                case TouchPhase.Ended:
                    loadingPower = false;
                    released = true;
                    rb.AddForce(0, 0, force);
                    StartCoroutine(NewBall(lifeTime));
                    break;
            }
        }
    }

    private float Distance(Vector3 v1, Vector3 v2) {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.y - v2.y, 2) + Mathf.Pow(v1.z - v2.z, 2));
    }

    private void BallControl() {
        float dist = Distance(Camera.main.transform.position, transform.position);
        if (dist >= newBallDist) {
            NewBall();
        }
    }

    private void NewBall() {
        Instantiate(skeeball, newBallPos.transform.position, newBallPos.transform.rotation);
        Destroy(gameObject);
    }

    private IEnumerator NewBall(float second) {
        yield return new WaitForSeconds(second); 
        if (gameObject != null) {
            NewBall();
        }
    }

}
