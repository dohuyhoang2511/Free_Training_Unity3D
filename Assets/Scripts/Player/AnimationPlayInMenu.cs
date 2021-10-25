using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayInMenu : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayAnim();
    }

    private void OnEnable()
    {
        PlayAnim();
    }

    private void PlayAnim()
    {
        if (gameObject.tag == "Adam")
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
}
