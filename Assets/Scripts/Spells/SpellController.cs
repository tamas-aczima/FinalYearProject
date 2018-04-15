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
    private bool isLeftCasting;
    private bool isRightCasting;
    private List<int> touchedColliders = new List<int>();
    private List<int> fireSpellColliders = new List<int>() { 2, 1, 0, 3 };
    private List<int> lightningSpellColliders = new List<int>() { 1, 0, 2, 3 };
    private int maxColliders = 4;
    private GameObject spell = null;
    private float fireBallVelocity = 10;
    private float lightningBoltLength = 3;

    public bool IsLeft
    {
        get { return isLeft; }
    }

    public bool IsLeftCasting
    {
        get { return isLeftCasting; }
    }

    public bool IsRightCasting
    {
        get { return isRightCasting; }
    }

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
        if (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft)
        {
            isLeftCasting = true;
        } 
        if (myPrioRig.GetJoyStickButtonDown(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft)
        {
            isRightCasting = true;
            //Debug.Log("Start casting");
        }

        //if button released, then stop casting
        if (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft)
        {
            isLeftCasting = false;
        }
        if (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft)
        {
            isRightCasting = false;
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

        if (touchedColliders.Count >= maxColliders || (touchedColliders.Count > 0 && isLeft && !isLeftCasting) || (touchedColliders.Count > 0 && !isLeft && !isRightCasting))
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

        //spell.GetComponent<LightningBoltScript>().StartPosition = Vector3.zero;
        spell.GetComponent<LightningBoltScript>().EndPosition = isLeft ? -transform.right * lightningBoltLength : transform.right * lightningBoltLength;

        //rotate spellLocation to align collider with lightning
        Vector3 relativePos = spell.GetComponent<LightningBoltScript>().EndPosition - spellLocation.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        spellLocation.rotation = rotation;

        if ((myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON) && isLeft) ||
            (myPrioRig.GetJoyStickButtonUp(YostSkeletalAPI.YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON) && !isLeft))
        {
            Destroy(spell);
            spell = null; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isLeftCasting || isRightCasting)
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
