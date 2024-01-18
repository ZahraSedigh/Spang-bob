using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player_Actions : MonoBehaviour
{


    [Header("Player camera : \n")]
    public GameObject Camera_Object;

    [Header("Player hand set : \n")]

    public GameObject[] Player_Bodys;

    [Header("Player Movment : \n")]

    public float Walking_Speed = 10;

    public float Run_Speed = 2;

    public float Jumping_Power = 10;

    public float Looking_Speed = 3;


    [Header("Player State : \n")]

    public bool Walk, Run, Jump;

    private Animator Player_Anim;



    [Header("Wepens List : \n")]

    public List<Gun_State> Wepens;

    public int Selected_Wepen;


    [Header("Bullet config : \n")]

    public GameObject Bullet;

    public GameObject[] Bullet_Points;

    public ParticleSystem[] Shoot_Particel;


    [Header("Shooting Setting : \n")]

    public bool No_Ammo = false;



    [Header("Ui Items : \n")]

    public Text Ammo_Text;

    public Image laif_Line;




    [Header("Player Health state : \n")]

    public float Player_Laif = 5;

    [Header("Sounds : \n")]

    public AudioSource Shoot_Sound;




    private float Player_Full_Laif;


    void Start()
    {
        Player_Full_Laif = 1 / Player_Laif;

        Time.timeScale = 1;

        laif_Line.fillAmount = 1;

        Player_Anim = Player_Bodys[Selected_Wepen].GetComponent<Animator>();

        Seting_Ammo_Text();


    }

    public void Get_Back_Call_Keys()
    {

      
        if (Input.GetKeyDown(KeyCode.R))
        {
            No_Ammo = true;
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cheng_Wepen();
        }

    }

    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (No_Ammo == false)
            {

                Player_Anim.SetTrigger("Shoot");

                Shoot_Sound.Play();

                Shoot_Particel[Selected_Wepen].Play();

                GameObject Bullet_Prifab = Instantiate(Bullet, Bullet_Points[Selected_Wepen].transform.position, Bullet_Points[Selected_Wepen].transform.rotation);

                Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin -= 1;

                Seting_Ammo_Text();

                if (Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin == 0)
                {
                    No_Ammo = true;
                    Reload();
                }


                Destroy(Bullet_Prifab, 0.35f);
            }

        }
    }


    public void Mouse_Looke()
    {

        float MouseX = Input.GetAxisRaw("Mouse X");
        float MouseY = Input.GetAxisRaw("Mouse Y");



        Vector3 Rotate = new Vector3(0, MouseX, 0) * Looking_Speed;

        Vector3 Rotate_camera = new Vector3(-MouseY, 0, 0) * Looking_Speed;

        Player_Bodys[Selected_Wepen].transform.Rotate(Rotate_camera);


        transform.Rotate(Rotate);

        Player_Bodys[Selected_Wepen].transform.eulerAngles = new Vector3(Player_Bodys[Selected_Wepen].transform.eulerAngles.x, Player_Bodys[Selected_Wepen].transform.eulerAngles.y, 0);


    }

    public void Cheng_Wepen()
    {
        if (Selected_Wepen == 0)
        {
            Selected_Wepen = 1;

            Player_Bodys[0].SetActive(false);

            Player_Bodys[1].SetActive(true);


        }
        else
        {
            Selected_Wepen = 0;

            Player_Bodys[1].SetActive(false);

            Player_Bodys[0].SetActive(true);
        }


    }

    public void Player_Walking()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Run = true;
            Walk = false;
            Walking_Speed *= Run_Speed;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Run = false;
            Walking_Speed /= Run_Speed;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * Jumping_Power, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }




        float Horizontal = Input.GetAxis("Horizontal");

        float Vertical = Input.GetAxis("Vertical");

        Vector3 Player_Derection = new Vector3(Horizontal, 0, Vertical);

        Walk = true;

        if (Player_Derection != Vector3.zero)
        {
            if (Walk == true)
            {
                Player_Anim.SetBool("Walk", true);
            }
            else
            {
                Player_Anim.SetBool("Walk", false);
            }

            if (Run == true)
            {
                Player_Anim.SetBool("Walk", false);

                Player_Anim.SetBool("Run", true);

            }
            else
            {
                Player_Anim.SetBool("Run", false);
            }


        }
        else
        {
            Player_Anim.SetBool("Walk", false);
            Player_Anim.SetBool("Run", false);
        }


        transform.Translate(Player_Derection * Walking_Speed * Time.deltaTime);


    }


    void Update()
    {

        Shoot();

        Mouse_Looke();

        Player_Walking();

        Get_Back_Call_Keys();


    }



    public void Reload()
    {

        Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount += Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin;

        Player_Anim.SetTrigger("Reload");

        if (Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount == Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload)
        {
            Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount = 0;

            Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin = Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload;


            Seting_Ammo_Text();
            Thread.Sleep(1000);

            No_Ammo = false;
        }

        if (Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount > Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload)
        {

            Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount -= Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload;

            Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin = Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload;
         
            Seting_Ammo_Text();

            Thread.Sleep(2000);

            No_Ammo = false;

        }

        if (Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount < Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_For_Reload)
        {
            Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin += Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount;

            Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount = 0;
          
            Seting_Ammo_Text();
          
            Thread.Sleep(2000);

            No_Ammo = false;

        }






    }

    public void Seting_Ammo_Text()
    {
        Ammo_Text.text = Wepens[Selected_Wepen].GetComponent<Gun_State>().Full_Bullets_Amount.ToString() + " / " + Wepens[Selected_Wepen].GetComponent<Gun_State>().Bullets_In_Magzin.ToString();
    }

  

    public void Set_Laif()
    {
        Player_Laif -= 1;

        laif_Line.fillAmount -= Player_Full_Laif;


        if (Player_Laif == 0)
        {
            Time.timeScale = 0;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            Set_Laif();
        }
    }


}
