using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShot : MonoBehaviour
{
    [SerializeField] GameObject sphere;
    [SerializeField] Transform setPos;

    private float shotSpeed = 300;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z"))
        {
            GameObject shotSphere = Instantiate(sphere, setPos.transform.position, Quaternion.identity);
            Rigidbody rb = shotSphere.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shotSpeed);
        }
    }
}
