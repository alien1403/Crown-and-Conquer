using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeSprite : MonoBehaviour
{
    public Image img;
    public List<Sprite> spriteChoices;

    private int counter;
    private int currentSprite = -1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        counter++;
        if(counter >= 1)
        {
            currentSprite = 1;
        //    img.sprite = spriteChoices[currentSprite];
        }
    }

}
