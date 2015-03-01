using UnityEngine;
using System.Collections;

public class CheckPoint_Down : MonoBehaviour {
    BoxCollider2D collider;
    void Start()
    {
        collider = transform.parent.gameObject.GetComponent<BoxCollider2D>();
    }
    void OnTriggerEnter2D(Collider2D runner)
    {
        if (runner.tag == "Player")
            collider.isTrigger = true;
    }
}
