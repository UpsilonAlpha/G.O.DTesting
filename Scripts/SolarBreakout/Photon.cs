using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Photon : MonoBehaviour
{
    public float c;
    Vector2[] dirs = {new Vector2(1,1), new Vector2(1,-1), new Vector2(0,-1), new Vector2(-1,-1), new Vector2(-1,1), new Vector2(0,1),};
    private Rigidbody2D rb;
    private bool photonReleased;
    SpriteRenderer background;
    public GameObject gameOver;
    public Slider progress;
    public Text text;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        background = this.GetComponentsInChildren<SpriteRenderer>()[1];
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && photonReleased == false)
        {
            transform.parent = null;
            photonReleased = true;
            rb.isKinematic = false;
            rb.velocity = Vector2.up * c;
            text.enabled = false;
        }
        rb.velocity = rb.velocity.normalized*c;
        Color tmp = background.color;
        tmp = Color.HSVToRGB(1-transform.position.y/100,1,1);
        tmp.a = 0.75f;
        background.color = tmp;
        progress.value = transform.position.y/100;
        if(transform.position.y > 100)
        {
            gameOver.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // if (col.gameObject.name == "Platform")
        // {
        //     float x = hitFactor(transform.position, col.transform.position, col.collider.bounds.size.x);
        //     Vector2 dir = new Vector2(x, 1).normalized;
        //     rb.velocity = dir * c;
        // }

        if (col.gameObject.name == "Platform")
        {
            Debug.Log("platform");
        }
    }

    float hitFactor(Vector2 photonPos, Vector2 platformPos, float platformWidth)
    {
        return (photonPos.x - platformPos.x) / platformWidth;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void End()
    {
        Application.Quit();
    }


}
