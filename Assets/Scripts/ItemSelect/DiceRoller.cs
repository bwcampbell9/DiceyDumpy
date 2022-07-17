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
    public float cone = 15;
    public float minForce = 1000, maxForce = 1500;
    public float minTorque = -60, maxTorque = 60;
    public float diceDelay = .2f;

    // Start is called before the first frame update
    void Start()
    {
        RollDice(20, 0);
    }

    void RollDice(int weaponDice, int foodDice)
    {
        for (int i = 0; i < weaponDice; i++)
        {
            Vector2 pointInCircle = Random.insideUnitCircle * radius;
            Vector3 finalPos = transform.TransformPoint(new Vector3(pointInCircle.x, 0, pointInCircle.y));
            Quaternion rotation = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(Random.Range(-cone, cone), Random.Range(-cone, cone), 0));
            StartCoroutine(SpawnDie(finalPos, rotation, i * diceDelay));
        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, transform.up, radius);
    }

    IEnumerator SpawnDie(Vector3 pos, Quaternion rot, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject newDie = Instantiate(DicePrefab, pos, rot, transform);
        newDie.GetComponent<Rigidbody>().AddForce(newDie.transform.up * Random.Range(minForce, maxForce));
        newDie.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque)));
        newDie.GetComponent<Die>().SetSides(new List<DiceOption>(new[] {
            GetRandomFromArray(weapons),
            GetRandomFromArray(weapons),
            GetRandomFromArray(weapons),
            GetRandomFromArray(weapons),
            GetRandomFromArray(weapons),
            GetRandomFromArray(weapons),
        }));
    }

    T GetRandomFromArray<T>(List<T> itemlist)
    {
        return itemlist[Random.Range(0, itemlist.Count)];
    }
}
