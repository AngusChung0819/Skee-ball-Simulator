using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeeBall : MonoBehaviour {

    // Game Manager
    GameManager gameManager;

    // GameObjects 
    public GameObject skeeball;

    // Power Control Variables
    public Scrollbar powerBar;
    public bool loadingPower = false;
    public float loadSpeed = 2.0f;
    public float timeBegin = 0;
    public float maxForce = 1550;
    public float minForce = 950;
    public float force;

    // Ball Control Variables
    public Rigidbody rb;
    public Vector3 tempSpeed;
    public bool released = false;
    public float lifeTime = 5;
    public GameObject newBallPos;
    public float newBallDist = 35f;

    // Scoring Variables
    public int getScore;

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
                // Ball is not released yet
                TouchControl();
            } else {
                // Ball is released
                BallControl();
            }
            
            // loading Power for ball release
            if (loadingPower) {
                LoadPower();
            }
        }
    }

    // Load Power for ball release force
    private void LoadPower() {
        powerBar.size = 1 - Mathf.Abs(Mathf.Cos((Time.time - timeBegin) * loadSpeed));
        force = (maxForce - minForce) * powerBar.size + minForce;
    }

    // Trigger to get score
    private void OnTriggerEnter(Collider other) {
        Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Hole") {
            // Ball drops into the hole
            GetComponent<SphereCollider>().enabled = false;
            // Get score
            getScore = int.Parse(other.gameObject.transform.GetComponentInChildren<TextMesh>().text);
            gameManager.score += getScore;
            // Get a new ball
            NewBall();
        }
    }

    // Pause the ball
    public void PauseBall() {
        tempSpeed = rb.velocity;
        rb.velocity = new Vector3(0, 0, 0);
        rb.useGravity = false;
    }

    // Resume the ball from Pausing
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
                    // Start loading power for ball releasing
                    timeBegin = Time.time;
                    loadingPower = true;
                    break;
                case TouchPhase.Moved:
                    // Move the ball horizontally (in x axis) with the touch position
                    float dist = Distance(Camera.main.transform.position, newBallPos.transform.position);
                    Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, dist);
                    Vector3 objPos = Camera.main.ScreenToWorldPoint(touchPosition);
                    // fix the position of y, z axis
                    objPos.y = newBallPos.transform.position.y;
                    objPos.z = newBallPos.transform.position.z;
                    transform.position = objPos;
                    break;
                case TouchPhase.Ended:
                    // Ball releasing
                    loadingPower = false;
                    released = true;
                    rb.AddForce(0, 0, force);
                    StartCoroutine(NewBall(lifeTime));
                    break;
            }
        }
    }

    // Distance calculation of two vector3
    private float Distance(Vector3 v1, Vector3 v2) {
        return Mathf.Sqrt(Mathf.Pow(v1.x - v2.x, 2) + Mathf.Pow(v1.y - v2.y, 2) + Mathf.Pow(v1.z - v2.z, 2));
    }

    // Determine when to give a new ball
    private void BallControl() {
        float dist = Distance(Camera.main.transform.position, transform.position);
        if (dist >= newBallDist) {
            NewBall();
        }
    }

    // give a new ball and remove the old ball
    private void NewBall() {
        Instantiate(skeeball, newBallPos.transform.position, newBallPos.transform.rotation);
        Destroy(gameObject);
    }

    // give a new ball and remove the old ball after a given time delay
    private IEnumerator NewBall(float second) {
        yield return new WaitForSeconds(second); 
        if (gameObject != null) {
            NewBall();
        }
    }

}
