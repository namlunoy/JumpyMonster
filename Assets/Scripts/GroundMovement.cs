using UnityEngine;
using System.Collections;

public class GroundMovement : MonoBehaviour
{
    private float speed = 0;
    public int numberOfPiece;
    private RunnerController runner;
    private GameController controller;
    void Start()
    {

    }

    void Update()
    {
        if (controller == null)
        {
            controller = GameController.Instance;
            if (controller != null)
                speed = controller.SpeedGround;
        }

        if (runner == null)
        {
            runner = controller.RunnerController;
        }
        else if (runner.IsAlive)
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D turnpoint)
    {
        //Quay lai cuoi
        if (turnpoint.tag == "TurnPoint")
        {
            Vector3 nextPos = transform.position;
            nextPos.x += transform.collider2D.bounds.size.x * numberOfPiece;
            this.transform.position = nextPos;
        }
    }
}
