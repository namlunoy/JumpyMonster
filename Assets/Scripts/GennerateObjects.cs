using UnityEngine;
using System.Collections;

public class GennerateObjects : MonoBehaviour
{
    public float minTime;
    public float maxTime;

    private float SpeedGround { get { return GameController.Instance.SpeedGround; } }

    public Transform[] spawnPositions;
    private Vector3[] positions;
    public GameObject[] objects;
    private RunnerController runner;
    void Start()
    {
        positions = new Vector3[spawnPositions.Length];
        for (int i = 0; i < positions.Length; i++)
            positions[i] = new Vector3(this.transform.position.x,
                              spawnPositions[i].position.y, 0);

    }

    void Update()
    {
        if (runner == null)
        {
            runner = GameController.Instance == null ? null : GameController.Instance.RunnerController;
            if (runner != null)
                StartCoroutine(Gennerate());
        }
    }

    IEnumerator Gennerate()
    {
        while (runner.IsAlive)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            GameObject obj = (GameObject)Instantiate(objects[Random.Range(0, objects.Length)],
                          positions[Random.Range(0, positions.Length)],
                          Quaternion.identity);
            Movement mov = obj.GetComponent<Movement>();
            if (mov != null)
            {
                if (mov.gameObject.tag == "Star")
                    mov.SetSpeed(Random.Range(SpeedGround + 1, SpeedGround + 2));
                else if (mov.gameObject.tag == "Bird")
                    mov.SetSpeed(Random.Range(SpeedGround + 5, SpeedGround + 8));
                else if (mov.gameObject.tag == "Clound")
                    mov.SetSpeed(GameController.Instance.SpeedSky);    
            }
        }
    }
}
