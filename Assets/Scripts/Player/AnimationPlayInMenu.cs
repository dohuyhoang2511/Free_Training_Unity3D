using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayInMenu : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        if(gameObject.tag == "Adam")
        {
            anim.Play("Brooklyn Uprock");
        }
        else if (gameObject.tag == "Brian")
        {
            anim.Play("Twist Dance");
        }
        else if (gameObject.tag == "Jackie")
        {
            anim.Play("Happy Idle");
        }
        else if (gameObject.tag == "Jody")
        {
            anim.Play("Idle");
        }
        else if (gameObject.tag == "Sophie")
        {
            anim.Play("Victory Idle");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
