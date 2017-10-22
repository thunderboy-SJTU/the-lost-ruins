using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{

    public float age;
    public float gender;
    public float smile;
    public float anger;
    public float contempt;
    public float disgust;
    public float fear;
    public float happiness;
    public float neutral;
    public float sadness;
    public float surprise;

    public string matchSinger = "";
    public string createData(){
        string data = "";
        data += age.ToString();
        data += " ";
        data += gender.ToString();
        data += " ";
        data += smile.ToString();
        data += " ";
        data += anger.ToString();
        data += " ";
        data += contempt.ToString();
        data += " ";
        data += disgust.ToString();
        data += " ";
        data += fear.ToString();
        data += " ";
        data += happiness.ToString();
        data += " ";
        data += neutral.ToString();
        data += " ";
        data += sadness.ToString();
        data += " ";
        data += surprise.ToString();
        return data;
    }
    public void setAge(float uage)
    {
        age = uage;
    }
    public float getAge()
    {
        return age;
    }


    public void setgender(float ugender)
    {
        gender = ugender;
    }
    public float getGender()
    {
        return gender;
    }

}
