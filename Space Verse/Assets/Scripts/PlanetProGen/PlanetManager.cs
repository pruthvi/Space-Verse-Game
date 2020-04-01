using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{

    void Start()
    {
        // CheckLocation();
    }


    void CheckLocation()
    {
        float radius = this.gameObject.transform.localScale.x * 10;
        Collider[] hitColliders = Physics.OverlapSphere(this.gameObject.transform.position, radius);
        if (hitColliders.Length > 0)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Planet")
        {
            Destroy(this.gameObject);
        }
    }
}
