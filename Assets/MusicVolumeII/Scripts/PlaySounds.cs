using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySounds : MonoBehaviour
{
    public AudioSource[] audioList = new AudioSource[17];

    public void Playsound01()
    {
        audioList[0].Play();

    }
    public void Playsound02()
    {
        audioList[1].Play();
   
    }
    public void Playsound03()
    {
        audioList[2].Play();

    }
    public void Playsound04()
    {
        audioList[3].Play();

    }
    public void Playsound05()
    {
        audioList[4].Play();

    }
    public void Playsound06()
    {
        audioList[5].Play();

    }

    public void Playsound07()
    {
        audioList[6].Play();

    }

    public void Playsound08()
    {
        audioList[7].Play();

    }

    public void Playsound09()
    {
        audioList[8].Play();

    }
    public void Playsound10()
    {
        audioList[9].Play();

    }
    public void Playsound11()
    {
        audioList[10].Play();

    }
    public void Playsound12()
    {
        audioList[11].Play();

    }
    public void Playsound13()
    {
        audioList[12].Play();

    }
    public void Playsound14()
    {
        audioList[13].Play();

    }

    public void Playsound15()
    {
        audioList[14].Play();

    }

    public void Playsound16()
    {
        audioList[15].Play();

    }

    public void Playsound17()
    {
        audioList[16].Play();

    }
    public void Stopaudio()
    {
        foreach (var i in audioList)
        {
            i.Stop();
        }
    }
}
