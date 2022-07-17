using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FartMeter : MonoBehaviour
{
    public RawImage image;
    public RectTransform rectTransform;
    //RenderTexture texture;
    Texture2D texture;
    Fart fart;

    // Start is called before the first frame update
    void Start()
    {
        texture = new Texture2D(image.texture.width, image.texture.height);

        fart = FindObjectOfType<Fart>();
    }

    // Update is called once per frame
    void Update()
    {
        int chance = (int)(fart.chance * texture.height);

        if (chance >= texture.height)
        {
            chance = texture.height - 1;
        }
        else if (chance < 0)
        {
            chance = 0;
        }

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (x == texture.height - 1)
                {
                    if (y == chance)
                    {
                        texture.SetPixel(x, y, Color.red);
                    }
                    else
                    {
                        texture.SetPixel(x, y, Color.white);
                    }
                }
                else
                {
                    texture.SetPixel(x, y, texture.GetPixel(x + 1, y));
                }
            }
        }
        texture.Apply();
        image.texture = (Texture)texture;
    }
}
