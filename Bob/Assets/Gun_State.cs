using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_State : MonoBehaviour
{
    public float Full_Bullets_Amount  =  180 ;

    public float Bullets_In_Magzin = 20;

    public float Bullets_For_Reload; 

    void Start()
    {
        Bullets_For_Reload = Bullets_In_Magzin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
