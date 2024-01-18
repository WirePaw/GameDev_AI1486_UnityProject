using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Will_spawn_at_start : MonoBehaviour
{
    public GameObject ObjectToSpawn;
    void Start()
    {
        if(ObjectToSpawn != null )
        {
            Instantiate(ObjectToSpawn, transform.position, transform.rotation);
        }
    }

}
