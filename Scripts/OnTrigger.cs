using UnityEngine;

public class OnTrigger : MonoBehaviour
{
    bool isCan = true;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
            isCan = false;
    }

    void FixedUpdate()
    {
        isCan = true;
    }

    void LateUpdate()
    {
        GunPhisics.isCan = isCan;
    }
}