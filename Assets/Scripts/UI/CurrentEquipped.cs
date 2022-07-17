using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEquipped : MonoBehaviour
{
    public Text equippedText;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.equippedSide) {
            case Player.Side.right:
                Weapon rightWeapon = player.weapons[(int)Player.AttachmentPoints.rightPoint];
                if (rightWeapon != null) {
                    equippedText.text = "RIGHT SLOT: " + rightWeapon.weaponName;
                }
                else
                {
                    equippedText.text = "RIGHT SLOT: EMPTY";
                }
                break;
            case Player.Side.top:
                Weapon topWeapon = player.weapons[(int)Player.AttachmentPoints.topPoint];
                if (topWeapon != null)
                {
                    equippedText.text = "TOP SLOT: " + topWeapon.weaponName;
                }
                else
                {
                    equippedText.text = "TOP SLOT: EMPTY";
                }
                break;
            case Player.Side.left:
                Weapon leftWeapon = player.weapons[(int)Player.AttachmentPoints.leftPoint];
                if (leftWeapon != null)
                {
                    equippedText.text = "LEFT SLOT: " + leftWeapon.weaponName;
                }
                else
                {
                    equippedText.text = "LEFT SLOT: EMPTY";
                }
                break;
            case Player.Side.grapple:
                equippedText.text = "GRAPPLE";
                break;
            case Player.Side.legs:
                equippedText.text = "LEGS";
                break;
            case Player.Side.ass:
                equippedText.text = "BUTT";
                break;
        }
    }
}
