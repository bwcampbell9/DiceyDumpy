using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropPoint : MonoBehaviour
{
    private GameObject hover;
    private GameObject obj;

    public void Hover(DiceOption option)
    {
        if (hover == null && obj == null)
        {
            hover = Instantiate(option.prefab, transform.position, transform.rotation, transform);
        }
    }

    public void UnHover()
    {
        Destroy(hover);
        hover = null;
    }

    public void Drop()
    {
        Debug.Log(hover);
        obj = hover;
        hover = null;
        Debug.Log(obj);
    }
}
