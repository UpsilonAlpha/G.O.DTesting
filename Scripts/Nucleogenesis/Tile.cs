using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int indRow;
    public int indCol;

    public int Number
    {
        get { return number; }
        set
        {
            number = value;
            if (number == 0)
            {
                SetEmpty();
            }
            else
            {
                ApplyElement(number);
                SetVisible();
            }
        }
    }

    private int number;

    private Text ElementName;
    private Image ElementImage;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ElementName = GetComponentInChildren<Text>();
        ElementImage = transform.Find("TextPanel").GetComponent<Image>();
    }

    public void Merge()
    {
        anim.SetTrigger("Merge");
    }

    public void Appear()
    {
        anim.SetTrigger("Appear");
    }

    void Apply(int index)
    {
        ElementName.text = Element.instance.elementDetails[index].Name;
        ElementImage.color = Element.instance.elementDetails[index].ElementColor;
    }

    void ApplyElement(int num)
    {
        switch (num)
        {
            case 1:
                Apply(0);
                break;
            case 2:
                Apply(1);
                break;
            case 3:
                Apply(2);
                break;
            case 4:
                Apply(3);
                break;
            case 5:
                Apply(4);
                break;
            case 6:
                Apply(5);
                break;
            case 7:
                Apply(6);
                break;
            case 8:
                Apply(7);
                break;
            case 9:
                Apply(8);
                break;
            default:
                print("YOU DONE FUCKED UP SONNY");
                break;
        }
    }

    private void SetVisible()
    {
        ElementName.enabled = true;
        ElementImage.enabled = true;
    }

    private void SetEmpty()
    {
        ElementName.enabled = false;
        ElementImage.enabled = false;
    }
}
