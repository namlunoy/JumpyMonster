using UnityEngine;
using System.Collections;

public class ObstacleController : MonoBehaviour
{
    private float speed = 0;
    private RunnerController runner;
    void Start()
    {
        GameObject r = (GameObject)GameObject.FindGameObjectWithTag("Player");
        runner = r.GetComponent<RunnerController>();

        GameObject o = GameObject.FindGameObjectWithTag("GameController");
        speed = o.GetComponent<GameController>().SpeedGround;
    }
    void Update()
    {
        if (runner.IsAlive)
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TurnPoint" && this.transform)
        {
            if (this.transform.parent)
                Destroy(this.transform.parent.gameObject);
            else Destroy(this.gameObject);
        }
        else if (other.tag == "Runner")
        {
            if (other.rigidbody2D.velocity.y < 0)
                this.collider2D.isTrigger = false;
        }
    }
}
