using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int combo;
    static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore=0;
        combo = 0;
    }

    public static void Hit(){
        //Instance.hitSFX.Play();
        combo += 1;
        comboScore += combo*100;
    }

    public static void Miss(){
        //Instance.missSFX.Play();
        combo=0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text= "Puntos: "+ comboScore.ToString() + "\n Combo: "+ combo.ToString();
    }
}
