using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{

    public GameObject DicePrefab;
    public List<DiceOption> weapons;
    public List<DiceOption> food;
    public float radius = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void RollDice(int weaponDice, int foodDice)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, radius);
    }
}
