using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPickUp : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().GiveRock();
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (Mathf.Sin(Time.time) * 0.001f), transform.position.z);
    }
}
