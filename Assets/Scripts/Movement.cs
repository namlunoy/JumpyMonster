using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private float speed = 0;
    private RunnerController runner;

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    void Update()
    {
       if(audio != null)
           audio.volume = Config.Sound_On ? 1 : 0;
        if(runner == null)
        {
            runner = GameController.Instance == null ? null : GameController.Instance.RunnerController;
        }
        else if (runner.IsAlive)
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TurnPoint")
            Destroy(this.gameObject);
        else if (other.tag == "Player" && this.gameObject.tag == "Star")
        {
            audio.Play();
            this.gameObject.GetComponent<SpriteRenderer>().sprite = null;
            Destroy(this.gameObject, 0.5f);
        }
    }
}
