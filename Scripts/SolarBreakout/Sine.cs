using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sine : MonoBehaviour
{
    public float col;
    public GameObject photon;
    private float spawnTime;
    public float startSpawnTime = 1f;
    public float speed = 600f;
    
    private ParticleSystem system;
    void Start()
    {
        system = this.GetComponent<ParticleSystem>();
        LeanTween.moveX(gameObject, 0.5f, .25f).setEaseInOutSine().setLoopPingPong();
        LeanTween.moveY(gameObject, 1000, speed);
    }

    void Update()
    {
        system.startColor = Color.HSVToRGB(col, 1,1);
    }

    public void PRETTY(float newCol)
    {
        col = newCol;
    }
    
    public void IAMSPEED(float sped)
    {
        speed = sped;
        Time.timeScale = speed;
    }
    
}
