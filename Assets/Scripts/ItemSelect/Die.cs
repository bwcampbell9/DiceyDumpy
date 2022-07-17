using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Die : MonoBehaviour
{
    public int sideUp;
    public List<GameObject> sides;

    private List<DiceOption> sideOptions;

    private void Update()
    {
        sideUp = GetSideUp();
    }

    public int GetSideUp()
    {
        int side = GetIndexOfLowestValue(new[] {
            Vector3.Angle(transform.up, Vector3.up),
            Vector3.Angle(-transform.up, Vector3.up),
            Vector3.Angle(transform.right, Vector3.up),
            Vector3.Angle(-transform.right, Vector3.up),
            Vector3.Angle(transform.forward, Vector3.up),
            Vector3.Angle(-transform.forward, Vector3.up),
                });
        return side;
    }

    public DiceOption GetTopOption()
    {
        return sideOptions[GetSideUp()];
    }

    public void SetSides(List<DiceOption> newSides)
    {
        sideOptions = newSides;
        for(int i = 0; i < sides.Count; i++)
        {
            sides[i].GetComponent<Renderer>().material.mainTexture = newSides[i].texture;
        }
    }

    private int GetIndexOfLowestValue(float[] arr)
    {
        float value = float.PositiveInfinity;
        int index = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] < value)
            {
                index = i;
                value = arr[i];
            }
        }
        return index;
    }
}
