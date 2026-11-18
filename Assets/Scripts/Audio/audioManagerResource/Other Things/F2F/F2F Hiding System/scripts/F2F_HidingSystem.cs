using System.Collections;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class F2F_HidingSystem : MonoBehaviour
{

    public GameObject Vcam_Player;
    public GameObject Vcam_Closet;

    public GameObject Door1_norm;
    public GameObject Door1_Transp;
    public GameObject Door2_norm;
    public GameObject Door2_Transp;

   // public FirstPersonController FPSScript;

    public Animator Closet_Anim;

    public bool InsideCloset = false;


    public IEnumerator GoInsideCloset_CO()
    {

        // go inside closet 

        Vcam_Closet.SetActive(true);
        Vcam_Player.SetActive(false);


       // FPSScript.enabled = false;

        Closet_Anim.SetInteger("C", 1);

        yield return new WaitForSeconds(1.5f);

        Door1_Transp.SetActive(true);
        Door2_Transp.SetActive(true);
        Door1_norm.SetActive(false);
        Door2_norm.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        Closet_Anim.SetInteger("C", 0);

        InsideCloset = true;

    }


    public IEnumerator GoOutCloset_CO()
    {


        Vcam_Player.SetActive(true);
        Vcam_Closet.SetActive(false);

        Door1_norm.SetActive(true);
        Door2_norm.SetActive(true);
        Door1_Transp.SetActive(false);
        Door2_Transp.SetActive(false);

        Closet_Anim.SetInteger("C", 1);

        yield return new WaitForSeconds(2f);

       // FPSScript.enabled = true;

        Closet_Anim.SetInteger("C", 0);

    }




}
