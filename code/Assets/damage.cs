using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damage : MonoBehaviour {

    public Image damage_Image;
    public Color flash_Color;
    public Color warn_Color;
    public float flash_Speed = 5;
    static bool damaged = false;
    public gameManager gamemanager;

    // Update is called once per frame  
    void Update()
    {
        //测试的输入代码段  
        /*if (Input.GetMouseButtonDown(0))
        {
            TakeDamage();
        }*/
        PlayDamagedEffect();

    }
    /// <summary>  
    /// 角色受伤后的屏幕效果  
    /// </summary>  
     public void PlayDamagedEffect()
    {
        if (damaged)
        {
            damage_Image.color = flash_Color;
        }
        else
        {
            damage_Image.color = Color.Lerp(damage_Image.color, Color.clear, flash_Speed * Time.deltaTime);

        }
        damaged = false;

    }
    /// <summary>  
    /// 角色受伤  
    /// </summary>  
    static public void TakeDamage()
    {
        damaged = true;

    }

}
