using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro_animation : MonoBehaviour {
    // canvas objects
    public Canvas slide1a;
    public Canvas slide1b;
    public Canvas slide2;
    public Canvas slide3a;
    public Canvas slide3b;
    public Canvas slide4;
    public Canvas slide5;
    public Canvas slide6;
    public Canvas slide7;
    public Canvas slide8;
    public Canvas slide9;
    public Canvas FoF_Logo;

    // sequential flags
    private bool flag0 = true;
    private bool flag1 = true;
    private bool flag2 = true;
    private bool flag3 = true;
    private bool flag4 = true;
    private bool flag5 = true;
    private bool flag6 = true;
    private bool flag7 = true;

    private void Start()
    {
        step0();
    }
    public void step0()
    {
        slide2.gameObject.SetActive(false);
        slide9.gameObject.SetActive(false);
        FoF_Logo.gameObject.SetActive(true);
        slide1a.gameObject.SetActive(true);
        slide1b.gameObject.SetActive(true);
        flag0 = false;
    }
    // if user presses "begin tutorial button" this sequence is executed"
    public void step1() {
        if (!flag0)
        {
            slide1a.gameObject.SetActive(false);
            slide1b.gameObject.SetActive(false);
            slide2.gameObject.SetActive(true);
            flag1 = false;
            flag0 = true;
        }
    }
    public void step2()
    {
        if (!flag1)
        {
            slide2.gameObject.SetActive(false);
            slide3a.gameObject.SetActive(true);
            slide3b.gameObject.SetActive(true);
            flag2 = false;
            flag1 = true;
        }
    }
    public void step3()
    {
        if (!flag2)
        {
            slide3a.gameObject.SetActive(false);
            slide3b.gameObject.SetActive(false);
            slide4.gameObject.SetActive(true);
            slide5.gameObject.SetActive(true);
            flag3 = false;
            flag2 = true;
        }
    }
    public void step4()
    {
        if (!flag3)
        {
            slide4.gameObject.SetActive(false);
            slide5.gameObject.SetActive(false);
            slide6.gameObject.SetActive(true);
            flag4 = false;
            flag3 = true;
        }
    }
    public void step5()
    {
        if (!flag4)
        {
            slide6.gameObject.SetActive(false);
            slide7.gameObject.SetActive(true);
            flag5 = false;
            flag4 = true;
        }
    }
    public void step6()
    {
        if (!flag5)
        {
            slide7.gameObject.SetActive(false);
            slide8.gameObject.SetActive(true);
            flag6 = false;
            flag5 = true;
        }
    }
    public void step7()
    {
        if (!flag6)
        {
            slide8.gameObject.SetActive(false);
            slide9.gameObject.SetActive(true);
            flag7 = false;
            flag6 = true;
        }
    }

    public void end()
    {
        slide1a.gameObject.SetActive(false);
        slide1b.gameObject.SetActive(false);
        slide2.gameObject.SetActive(false);
        slide3a.gameObject.SetActive(false);
        slide3b.gameObject.SetActive(false);
        slide4.gameObject.SetActive(false);
        slide5.gameObject.SetActive(false);
        slide6.gameObject.SetActive(false);
        slide7.gameObject.SetActive(false);
        slide8.gameObject.SetActive(false);
        slide9.gameObject.SetActive(false);
        FoF_Logo.gameObject.SetActive(false);
        flag7 = true;
    }

}
