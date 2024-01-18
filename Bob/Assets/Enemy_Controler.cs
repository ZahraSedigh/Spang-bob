using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controler : MonoBehaviour
{
    public int laif = 4;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);

            laif -= 1;

            if (laif == 0)
            {
                Destroy(this.gameObject);

            }

            Debug.Log(this.name + " Het");
        }
    }



}
