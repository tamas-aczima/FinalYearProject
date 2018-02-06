using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class SpellController : MonoBehaviour {

    [SerializeField] private SphereCollider[] spellColliders;
    [SerializeField] private Transform spellLocation;
    [SerializeField] private GameObject fireSpell;
    [SerializeField] private GameObject lightningSpell;

    private YostSkeletonRig myPrioRig;
    private bool isLeft;
    private bool isCasting;
    private List<int> touchedColliders = new List<int>();
    private List<int> fireSpellColliders = new List<int>() { 2, 1, 0, 3 };
    private List<int> lightningSpellColliders = new List<int>() { 1, 0, 2, 3 };
    private int maxColliders = 4;
    private GameObject spell = null;
    private float fireBallVelocity = 10;
    private float lightningBoltLength = 3;

	void Start () {
        myPrioRig = gameObject.transform.root.gameObject.GetComponent<YostSkeletonRig>();
        isLeft = gameObject.name == "LeftHand";
    }
	
	void Update () {
        CheckCasting();

        CastSpell();

        ShootFireBall();

        UpdateLightningBolt();
    }

    private void CheckCasting()
    {
        //if button pressed, then start casting
        if ((myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            isCasting = true;
            //Debug.Log("Start casting");
        }

        //if button released, then stop casting
        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft && isCasting) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft && isCasting))
        {
            isCasting = false;
            //Debug.Log("Stop casting");
        }
    }

    private void CastSpell()
    {
        if (spell != null) return;

        if (touchedColliders.Count == 4)
        Debug.Log(touchedColliders[0] + " " + touchedColliders[1] + " " + touchedColliders[2] + " " + touchedColliders[3]);

        if (touchedColliders.SequenceEqual(fireSpellColliders))
        {
            spell = Instantiate(fireSpell, spellLocation.position, Quaternion.identity, spellLocation);
        }

        if (touchedColliders.SequenceEqual(lightningSpellColliders))
        {
            spell = Instantiate(lightningSpell, spellLocation.position, Quaternion.identity, spellLocation);
        }

        if (touchedColliders.Count > 0 && (touchedColliders.Count >= maxColliders || !isCasting))
        {
            Debug.Log("cleared list");
            touchedColliders.Clear();
        }
    }

    private void ShootFireBall()
    {
        if (spell == null || (spell != null && !spell.name.StartsWith(fireSpell.name))) return;

        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            Vector3 velocity = isLeft ? -transform.right : transform.right;
            velocity *= fireBallVelocity;
            spell.GetComponent<Rigidbody>().velocity = velocity;
            spell.GetComponent<Rigidbody>().isKinematic = false;
            spell.transform.parent = null;
            spell = null;
        }
    }

    private void UpdateLightningBolt()
    {
        if (spell == null || (spell != null && !spell.name.StartsWith(lightningSpell.name))) return;

        spell.GetComponent<LightningBoltScript>().StartPosition = Vector3.zero;
        spell.GetComponent<LightningBoltScript>().EndPosition = isLeft ? -transform.right * lightningBoltLength : transform.right * lightningBoltLength;

        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            Destroy(spell);
            spell = null; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCasting)
        {
            for (int i = 0; i < spellColliders.Length; i++)
            {
                if (other == spellColliders[i] && !touchedColliders.Contains(i))
                {
                    Debug.Log("Added: " + i);
                    touchedColliders.Add(i);
                }
            }
        }
    }
}
