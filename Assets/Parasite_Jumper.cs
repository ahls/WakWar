using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasite_Jumper : MonoBehaviour
{
    public Parasite_Behaviour PB;
   
    public void callJump()
    {
        PB.jump();
    }
}
