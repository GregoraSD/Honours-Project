using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAnimationScript : MonoBehaviour {
    public Animator French;
    public Animator English;
    public float Timer;
    public float Timer2; 

    // Use this for initialization
    void Start () {
        StartCoroutine(TextAnimationWait());
	}

    IEnumerator TextAnimationWait()
    {
        French.SetTrigger("FrenchTrigger");
        yield return new WaitForSeconds(Timer);
        French.SetTrigger("FrenchFade");
        English.SetTrigger("EnglishTrigger");
        yield return new WaitForSeconds(Timer2);
        English.SetTrigger("EnglishFade");
    }
}
