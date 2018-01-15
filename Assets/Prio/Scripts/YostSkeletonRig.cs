// Authors: Travis Lynn, Nick Leyder 
// This script is a collection of convenience functions that wrap normal functionality
// of the Skeleton and Prio APIs for use in Unity. This script is designed to get a Prio suit
// working quickly with the standard layouts (Lite, Core, Pro).

using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using YostSkeletalAPI;

[RequireComponent(typeof(Animator))]

//PrioVR Skeleton Rig Class
//This class retargets the Prio Humaniod Skeleton
//on to the Unity Humaniod Animator Skeleton.
public class YostSkeletonRig : MonoBehaviour
{

    //Skeleton Information
    [System.Serializable]
    public class SkeletonEntry
    {
        public YOST_SKELETON_BONE type;
        public Transform bone;

        [HideInInspector]
        public Quaternion next_quat;

        public SkeletonEntry()
        {
            type = YOST_SKELETON_BONE.YOST_SKELETON_BONE_NULL;
            bone = null;
        }
        public SkeletonEntry(YOST_SKELETON_BONE yostType, Transform mBone)
        {
            type = yostType;
            bone = mBone;
        }
    }

    // A reference to our animator component
    private Animator anim;

    // Skeleton Attributes
    public int isPrio; // Whether or not the suit is a PrioVR Suit or a TSS SUIT (0=PrioVR,1=TSS)
    public int isMale; // Whether or not the user is male.
    public bool customSkeleton = false; // Whether or not to use a standard skeleton, or if the user wants a custom skeleton
    public uint playerNumber = 0; // The number of the player, this is used for determining which PrioVR Suit to be used to drive the model
    public int playerAge; // The age of the user
    public float playerHeight; // The height of the user
    public List<SkeletonEntry> bones = new List<SkeletonEntry>(); // A list of the bones the user wants to be tracked

    // Prio Suit Attributes
    public PrioSuitLayout suitLayout = PrioSuitLayout.LAYOUT_PRO; // The current layout wanted by the user

    // Tss Suit Attributes
    public string skeletonXML; // The current xml layout wanted by the user

    // API Ids
    private uint skeletonDeviceId; // The device id for the Yost Skeleton  
    public uint animateProcId; // The processor id for the Yost Connection processor
    private uint pedTrackingProcId; // The processor id for the Pedestrian Tracking processor
    private uint smoothingProcId; // The processor id for the Smoothing processor

    // Streaming Variables
    private bool isStreaming = false; // Whether or not the Yost Connection processor is streaming

    private YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR; // A Yost Skeleton error 

    // JoyStick Data
    private YOST_JOYSTICK leftJoyStickLastFrame; // The Left PrioVR Hand Controller's data for the last frame
    private YOST_JOYSTICK leftJoyStick; // The Left PrioVR Hand Controller's data for the current frame
    private YOST_JOYSTICK rightJoyStickLastFrame; // The PrioVRRight Hand Controller's data for the last frame
    private YOST_JOYSTICK rightJoyStick; // The Right PrioVR Hand Controller's data for the current frame

    private byte hubButtonState; // The current state of the PrioVR Hub Buttons

    // Pedestrian Variables
    private Vector3 moveDirection = Vector3.zero; // The current direction to move the model by

    public float maximumCertainty = 1;
    public float varianceMultiplyFactorPed = 4;
    public int varianceDataLengthPed = 10;

    // Smoothing Variables
    public int attachSmoothing = 1;
    public float minSmoothingFactor = 0;
    public float maxSmoothingFactor = 0.6f;
    public float lowerVarianceBound = .02f;
    public float upperVarianceBound = .25f;
    public float varianceMultiplyFactorSmooth = 4;
    public int varianceDataLengthSmooth = 10;

    private List<HumanBodyBones> trackedBones = new List<HumanBodyBones>();

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width/2, 50), "Left Joy X-Axis: " + leftJoyStickLastFrame.x_axis);
        GUI.Label(new Rect(0, 50, Screen.width/2, 50), "Left Joy Y-Axis: " + leftJoyStickLastFrame.y_axis);
        GUI.Label(new Rect(0, 100, Screen.width/2, 50), "Left Joy X Button: " + leftJoyStickLastFrame.x_button);
        GUI.Label(new Rect(0, 150, Screen.width/2, 50), "Left Joy A Button: " + leftJoyStickLastFrame.a_button);
        GUI.Label(new Rect(0, 200, Screen.width/2, 50), "Left Joy B Button: " + leftJoyStickLastFrame.b_button);
        GUI.Label(new Rect(0, 250, Screen.width/2, 50), "Left Joy Y Button: " + leftJoyStickLastFrame.y_button);
        GUI.Label(new Rect(0, 300, Screen.width/2, 50), "Left Joy Top Button: " + leftJoyStickLastFrame.top_button);
        GUI.Label(new Rect(0, 350, Screen.width/2, 50), "Left Joy Joystick Button: " + leftJoyStickLastFrame.joystick_button);
        GUI.Label(new Rect(0, 400, Screen.width/2, 50), "Left Joy Trigger: " + leftJoyStickLastFrame.trigger);
        GUI.Label(new Rect(0, 450, Screen.width / 2, 50), "Hub Button State: " + hubButtonState);

        GUI.Label(new Rect(Screen.width / 2, 0, Screen.width / 2, 100), "Right Joy X-Axis: " + rightJoyStick.x_axis);
        GUI.Label(new Rect(Screen.width / 2, 50, Screen.width / 2, 100), "Right Joy Y-Axis: " + rightJoyStick.y_axis);
        GUI.Label(new Rect(Screen.width / 2, 100, Screen.width / 2, 100), "Right Joy X Button: " + rightJoyStick.x_button);
        GUI.Label(new Rect(Screen.width / 2, 150, Screen.width / 2, 100), "Right Joy A Button: " + rightJoyStick.a_button);
        GUI.Label(new Rect(Screen.width / 2, 200, Screen.width / 2, 100), "Right Joy B Button: " + rightJoyStick.b_button);
        GUI.Label(new Rect(Screen.width / 2, 250, Screen.width / 2, 100), "Right Joy Y Button: " + rightJoyStick.y_button);
        GUI.Label(new Rect(Screen.width / 2, 300, Screen.width / 2, 100), "Right Joy Top Button: " + rightJoyStick.top_button);
        GUI.Label(new Rect(Screen.width / 2, 350, Screen.width / 2, 100), "Right Joy Joystick Button: " + rightJoyStick.joystick_button);
        GUI.Label(new Rect(Screen.width / 2, 400, Screen.width / 2, 100), "Right Joy Trigger: " + rightJoyStick.trigger);
    }

    // Use this for initialization
    void Start()
    {
        leftJoyStick = new YOST_JOYSTICK();
        leftJoyStickLastFrame = new YOST_JOYSTICK();
        rightJoyStick = new YOST_JOYSTICK();
        rightJoyStickLastFrame = new YOST_JOYSTICK();
        anim = GetComponentInChildren<Animator>();

        //Create the YOST Skeleton
        if (playerHeight > 0.0f)
        {
            skeletonDeviceId = yost.createStandardSkeletonWithHeight((byte)isMale, playerHeight);
        }
        else
        {
            skeletonDeviceId = yost.createStandardSkeletonWithAge((byte)isMale, (uint)playerAge);
        }

        yost.setSkeletonToStandardTPose(skeletonDeviceId);
        yost.setStandardDeviceXmlMapPrioProLayout(skeletonDeviceId);

        yost.setSkeletonUnit(skeletonDeviceId, YOST_SKELETON_UNIT.YOST_SKELETON_UNIT_METERS);

        SetupProcessors();

        SetupBones();
        SetupBoneOffsets();

        // Start gathering PrioVR Controller data
        error = yost.setControllerUpdateRate(animateProcId, 25);

        //Set the Prio Layout to Match Suit
        UpdateSuitLayout();
        yost.startControllerUpdating(animateProcId);
    }

    // Update is called once per frame before the animation cycle
    void Update()
    { 
        if (isStreaming)
        {
            //Update the Skeleton in the API
            error = yost.update(skeletonDeviceId);
            if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
            {
                Debug.LogError(string.Format("update {1} failed: {0} ", error, animateProcId));
            }
        }

        UpdateLastFrameJoystickData();

        GetJoyStickData();
        yost.getPrioHubButtonState(animateProcId, out hubButtonState);

        if (isStreaming && Input.GetKeyDown(KeyCode.B))
        {
            StopStreaming();
        }

        if (!isStreaming && Input.GetKeyDown(KeyCode.A))
        {
            StartStreaming();
        }
    }

    // LateUpdate is called at a regular interval
    void FixedUpdate()
    {
        if (isStreaming)
        {
            Transform rootBone = anim.GetBoneTransform(HumanBodyBones.Hips);
            float[] tmp = new float[3];
            byte[] bone_name = new byte[32];
            YOST_SKELETON_ERROR error = yost.getPinnedBoneDataPedestrianTrackingProcessor(pedTrackingProcId, tmp, bone_name, 32);
            string pinned_bone_name = System.Text.Encoding.UTF8.GetString(bone_name);

            if (GameObject.Find(pinned_bone_name) == null)
            {
                return;
            }

            CharacterController controller = GetComponent<CharacterController>();
            moveDirection = Vector3.zero;

            tmp[0] = 0;
            tmp[1] = 0;
            tmp[2] = 0;
            error = yost.getSkeletonTranslation(pedTrackingProcId, tmp);
            moveDirection = new Vector3(tmp[0], tmp[1], tmp[2]);

            controller.Move(moveDirection);

            LayerMask groundMask = LayerMask.GetMask("Ground");
            RaycastHit ray;
            Physics.Raycast(rootBone.position + Vector3.up, -Vector3.up, out ray, 10, groundMask);

            rootBone.position = new Vector3(rootBone.position.x, rootBone.position.y - GameObject.Find(pinned_bone_name).transform.position.y + ray.point.y + .08f, rootBone.position.z);
        }
    }

    // LateUpdate is called once per frame after the animation cycle
    void LateUpdate()
    {
        // Update Skeleton To Match YOST Internal Skeleton
        if (isStreaming)
        {
            foreach (SkeletonEntry entry in bones)
            {
                if (entry.bone == null)
                {
                    continue;
                }

                entry.next_quat = GetBoneOrientation(entry.bone.name);

                entry.bone.rotation = entry.next_quat;
            }

            //UpdateLastFrameJoystickData();
        }
    }

    // Get the Global Posistion of a Yost Skeleton Bone
    public Vector3 GetBonePosition(string alias_name)
    {
        YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        byte[] alias = System.Text.Encoding.UTF8.GetBytes(alias_name);
        float[] packet = new float[3];

        error = yost.getBonePosition(skeletonDeviceId, alias, packet);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("getBoneOrientation {1} failed: {0}", error.ToString(), alias_name));
        }

        Vector3 pos = new Vector3(packet[0], packet[1], packet[2]);
        return pos;
    }

    // Calibrate the Yost Connection processor
    public IEnumerator CalibrateSens(float waitTime)
    {
        StopStreaming();
        yost.stopControllerUpdating(animateProcId);
        ResetPose();
        YOST_SKELETON_ERROR sError = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;

        yield return new WaitForSeconds(waitTime);
        yost.setSkeletonToStandardTPose(skeletonDeviceId);


        if (isPrio > 0)
        {
            yost.calibrateTssProcessor(animateProcId);
        }
        else
        {
            sError = yost.calibratePrioProcessor(animateProcId, 0.0f);
        }

        if (sError != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("calibrateProcessor {1} failed: {0}", sError, animateProcId));
        }
        else
        {
            StartStreaming();
        }
    }

    // Update the Yost Connection processor suit layout
    void UpdateSuitLayout()
    {
        StopStreaming();

        if (isPrio > 0)
        {
            string assetPath = Directory.GetCurrentDirectory() + "\\Assets\\TSSXML\\" + skeletonXML;
            LoadXmlSkeletonLayout(assetPath);
        }
        else
        {
            switch (suitLayout)
            {
                case PrioSuitLayout.LAYOUT_PRO:
                    yost.setStandardDeviceXmlMapPrioProLayout(skeletonDeviceId);
                    break;
                case PrioSuitLayout.LAYOUT_CORE:
                    yost.setStandardDeviceXmlMapPrioCoreLayout(skeletonDeviceId);
                    break;
                default:
                    yost.setStandardDeviceXmlMapPrioLiteLayout(skeletonDeviceId);
                    break;
            }
        }
    }

    // Start streaming of the current Yost Connection processor
    void StartStreaming()
    {
        yost.stopControllerUpdating(animateProcId);
        yost.setupStandardStreamPrioProcessor(animateProcId);
        if (isPrio > 0)
        {
            error = yost.startTssProcessor(animateProcId);
        }
        else
        {
            error = yost.startPrioProcessor(animateProcId);
        }

        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("startPrioProcessor {1} failed: {0} ", error, animateProcId));
        }
        Debug.Log("=============Start Streaming=============");
        isStreaming = true;
        anim.enabled = true;
    }

    // Stop streaming of the current Yost Connection processor
    public void StopStreaming()
    {
        error = yost.startControllerUpdating(animateProcId);

        if (isPrio > 0)
        {
            error = yost.stopTssProcessor(animateProcId);
        }
        else
        {
            error = yost.stopPrioProcessor(animateProcId);
        }

        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("stopPrioProcessor {1} failed: {0}", error, animateProcId));
        }
        isStreaming = false;
        Debug.Log("=============Stop Streaming=============");
        anim.enabled = false;
    }

    // Reset the model to it's original pose
    void ResetPose()
    {
        foreach (SkeletonEntry entry in bones)
        {
            if (entry.bone != null)
            {
                entry.bone.transform.rotation = GetBoneOrientationOffset(entry.bone.name);
            }
        }
    }

    // Setup a PrioVR/TSS processor, Smoothing processor, and a Pedestrian Tracking processor
    void SetupProcessors()
    {
        if (isPrio > 0)
        {
            animateProcId = yost.createTssProcessor();
        }
        else
        {
            animateProcId = yost.createPrioProcessor(3, playerNumber);
        }

        YOST_SKELETON_ERROR lError = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        lError = yost.addProcessorToSkeleton(skeletonDeviceId, 0, animateProcId);

        if (lError != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("addProcessorToSkeleton failed: {0}", lError.ToString()));
        }

        pedTrackingProcId = yost.createPedestrianTrackingProcessor();
        yost.addProcessorToSkeleton(skeletonDeviceId, 1, pedTrackingProcId);

        yost.setMaxCertaintyPedestrianTrackingProcessor(pedTrackingProcId, maximumCertainty);
        yost.setVarianceMultiplyFactorPedestrianTrackingProcessor(pedTrackingProcId, varianceMultiplyFactorPed);
        yost.setVarianceDataLengthPedestrianTrackingProcessor(pedTrackingProcId, varianceDataLengthPed);

        if (attachSmoothing == 0)
        {
            smoothingProcId = yost.createSmoothingProcessor();
            yost.addProcessorToSkeleton(skeletonDeviceId, 2, smoothingProcId);

            yost.setAllMinSmoothingFactorSmoothingProcessor(smoothingProcId, minSmoothingFactor);
            yost.setAllMaxSmoothingFactorSmoothingProcessor(smoothingProcId, maxSmoothingFactor);
            yost.setAllLowerVarianceBoundSmoothingProcessor(smoothingProcId, lowerVarianceBound);
            yost.setAllUpperVarianceBoundSmoothingProcessor(smoothingProcId, upperVarianceBound);
            yost.setAllVarianceMultiplyFactorSmoothingProcessor(smoothingProcId, varianceMultiplyFactorSmooth);
            yost.setAllVarianceDataLengthSmoothingProcessor(smoothingProcId, varianceDataLengthSmooth);
        }
    }

    // Pairs the Unity HumanBone to the YOST_SKELETON_BONEs.
    void SetupBones()
    {
        // Remove the default tracked bones, so we can add our aliased bones
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightFoot"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftFoot"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftLowerLeg"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightLowerLeg"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftUpperLeg"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightUpperLeg"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("Hips"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("Spine2"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightShoulder"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftShoulder"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightUpperArm"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftUpperArm"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightLowerArm"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftLowerArm"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("RightHand"));
        yost.stopTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes("LeftHand"));

        //Set up bone alias.
        foreach (SkeletonEntry entry in bones)
        {
            if (entry.bone == null)
                continue;
            if (entry.type != YOST_SKELETON_BONE.YOST_SKELETON_BONE_NULL)
            {
                string bone_name = "";
                bone_name = GetStandardBoneName(entry.type, 32U);
                AddBoneAlias(entry.bone.name, bone_name);

                yost.startTrackingBonePedestrianTrackingProcessor(pedTrackingProcId, System.Text.Encoding.UTF8.GetBytes(entry.bone.name));
            }
        }
    }

    // Generate a skeleton based off of the Unity HumanBodyBones
    public void GenerateSkeleton()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        bones.Clear();
        //Adding Root Bones
        if (suitLayout == PrioSuitLayout.LAYOUT_PRO)
        {
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_HIPS, anim.GetBoneTransform(HumanBodyBones.Hips)));
            if (anim.GetBoneTransform(HumanBodyBones.Chest) != null)
            {
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_SPINE, anim.GetBoneTransform(HumanBodyBones.Chest)));
            }
        }
        else
        {
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_SPINE, anim.GetBoneTransform(HumanBodyBones.Hips)));
        }

        if (suitLayout != PrioSuitLayout.LAYOUT_CHEST)
        {
            //Adding Upperbody Bones
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_HEAD, anim.GetBoneTransform(HumanBodyBones.Head)));
            if (anim.GetBoneTransform(HumanBodyBones.LeftShoulder) != null && suitLayout == PrioSuitLayout.LAYOUT_PRO)
            {
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_SHOULDER, anim.GetBoneTransform(HumanBodyBones.LeftShoulder)));
            }
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_UPPER_ARM, anim.GetBoneTransform(HumanBodyBones.LeftUpperArm)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_LOWER_ARM, anim.GetBoneTransform(HumanBodyBones.LeftLowerArm)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_HAND, anim.GetBoneTransform(HumanBodyBones.LeftHand)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_JOYSTICK, null));
            if (anim.GetBoneTransform(HumanBodyBones.RightShoulder) != null && suitLayout == PrioSuitLayout.LAYOUT_PRO)
            {
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_SHOULDER, anim.GetBoneTransform(HumanBodyBones.RightShoulder)));
            }
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_UPPER_ARM, anim.GetBoneTransform(HumanBodyBones.RightUpperArm)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_LOWER_ARM, anim.GetBoneTransform(HumanBodyBones.RightLowerArm)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_HAND, anim.GetBoneTransform(HumanBodyBones.RightHand)));
            bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_JOYSTICK, null));

            //Adding Lower Body Bones
            if (suitLayout <= PrioSuitLayout.LAYOUT_CORE)
            {
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_UPPER_LEG, anim.GetBoneTransform(HumanBodyBones.LeftUpperLeg)));
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_LOWER_LEG, anim.GetBoneTransform(HumanBodyBones.LeftLowerLeg)));
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_LEFT_FOOT, anim.GetBoneTransform(HumanBodyBones.LeftFoot)));
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_UPPER_LEG, anim.GetBoneTransform(HumanBodyBones.RightUpperLeg)));
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_LOWER_LEG, anim.GetBoneTransform(HumanBodyBones.RightLowerLeg)));
                bones.Add(new SkeletonEntry(YOST_SKELETON_BONE.YOST_SKELETON_BONE_RIGHT_FOOT, anim.GetBoneTransform(HumanBodyBones.RightFoot)));
            }
        }
    }

    // Send our models current bone orientations to the skeleton
    void SetupBoneOffsets()
    {
        foreach (SkeletonEntry entry in bones)
        {
            if (entry.bone == null)
            {
                continue;
            }
            float[] packet = new float[4];
            packet[0] = entry.bone.transform.rotation.x;
            packet[1] = entry.bone.transform.rotation.y;
            packet[2] = entry.bone.transform.rotation.z;
            packet[3] = entry.bone.transform.rotation.w;

            yost.setBoneOrientationOffset(skeletonDeviceId, System.Text.Encoding.UTF8.GetBytes(entry.bone.name), packet);
        }
    }

    //Gets the Orientation from the YOST_SKELETON_BONE.
    void LoadXmlSkeletonLayout(string file_name)
    {
        YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        byte[] file = System.Text.Encoding.UTF8.GetBytes(file_name);

        error = yost.loadDeviceXmlMapCustomLayout(skeletonDeviceId, file);

        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("loadDeviceXmlMapCustomLayout {1} failed: {0}", error.ToString(), file_name));
        }
    }

    //Returns the string form of the YOST_SKELETON_BONE.
    string GetStandardBoneName(YOST_SKELETON_BONE bone, uint name_len)
    {
        YOST_SKELETON_ERROR error;
        byte[] name = new byte[name_len];

        error = yost.getStandardBoneName(bone, name, name_len);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("getStandardBoneName failed: {0}", error.ToString()));
            return "";
        }

        return System.Text.Encoding.UTF8.GetString(name);
    }

    //Adds a Bone Alias to the YOST_SKELETON_BONE.
    void AddBoneAlias(string alias_name, string bone_name)
    {
        YOST_SKELETON_ERROR error;
        byte[] alias = System.Text.Encoding.UTF8.GetBytes(alias_name);
        byte[] name = System.Text.Encoding.UTF8.GetBytes(bone_name);

        error = yost.addBoneAlias(skeletonDeviceId, alias, name);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("addBoneAlias failed: {0}", error.ToString()));
        }
    }

    //Gets the Orientation from the YOST_SKELETON_BONE.
    Quaternion GetBoneOrientationOffset(string alias_name)
    {
        YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        byte[] alias = System.Text.Encoding.UTF8.GetBytes(alias_name);
        float[] packet = new float[4];

        error = yost.getBoneOrientationOffset(skeletonDeviceId, alias, packet);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("getBoneOrientationOffset {1} failed: {0}", error.ToString(), alias_name));
        }

        Quaternion quat = new Quaternion(packet[0], packet[1], packet[2], packet[3]);
        return quat;
    }

    //Gets the Orientation from the YOST_SKELETON_BONE.
    Quaternion GetBoneOrientation(string alias_name)
    {
        YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        byte[] alias = System.Text.Encoding.UTF8.GetBytes(alias_name);
        float[] packet = new float[4];

        error = yost.getBoneOrientation(skeletonDeviceId, alias, packet);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("getBoneOrientation {1} failed: {0}", error.ToString(), alias_name));
        }

        Quaternion quat = new Quaternion(packet[0], packet[1], packet[2], packet[3]);
        return quat;
    }

    //Gets the Orientation from the YOST_SKELETON_BONE.
    public Vector3 GetBoneVelocity(string alias_name)
    {
        YOST_SKELETON_ERROR error = YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR;
        byte[] alias = System.Text.Encoding.UTF8.GetBytes(alias_name);
        float[] packet = new float[3];

        error = yost.getBoneVelocity(skeletonDeviceId, alias, packet);
        if (error != YOST_SKELETON_ERROR.YOST_SKELETON_NO_ERROR)
        {
            Debug.LogError(string.Format("getBoneVelocity {1} failed: {0}", error.ToString(), alias_name));
        }

        Vector3 vel = new Vector3(packet[0], packet[1], packet[2]);
        return vel;
    }

    // Update the last frame of PrioVR joystick for button down events
    void UpdateLastFrameJoystickData()
    {
        leftJoyStickLastFrame.x_axis = leftJoyStick.x_axis;
        leftJoyStickLastFrame.y_axis = leftJoyStick.y_axis;
        leftJoyStickLastFrame.x_button = leftJoyStick.x_button;
        leftJoyStickLastFrame.a_button = leftJoyStick.a_button;
        leftJoyStickLastFrame.b_button = leftJoyStick.b_button;
        leftJoyStickLastFrame.y_button = leftJoyStick.y_button;
        leftJoyStickLastFrame.top_button = leftJoyStick.top_button;
        leftJoyStickLastFrame.joystick_button = leftJoyStick.joystick_button;
        leftJoyStickLastFrame.trigger = leftJoyStick.trigger;

        rightJoyStickLastFrame.x_axis = rightJoyStick.x_axis;
        rightJoyStickLastFrame.y_axis = rightJoyStick.y_axis;
        rightJoyStickLastFrame.x_button = rightJoyStick.x_button;
        rightJoyStickLastFrame.a_button = rightJoyStick.a_button;
        rightJoyStickLastFrame.b_button = rightJoyStick.b_button;
        rightJoyStickLastFrame.y_button = rightJoyStick.y_button;
        rightJoyStickLastFrame.top_button = rightJoyStick.top_button;
        rightJoyStickLastFrame.joystick_button = rightJoyStick.joystick_button;
        rightJoyStickLastFrame.trigger = rightJoyStick.trigger;
    }

    // Get the PrioVR Hand Controller data
    void GetJoyStickData()
    {
        byte[] leftJoy = new byte[4];
        leftJoy[0] = 128;
        leftJoy[1] = 128;
        leftJoy[2] = 0;
        leftJoy[3] = 0;
        byte[] rightJoy = new byte[4];
        rightJoy[0] = 128;
        rightJoy[1] = 128;
        rightJoy[2] = 0;
        rightJoy[3] = 0;
        yost.getJoyStickStatePrioProcessor(animateProcId, leftJoy, rightJoy);


        if (leftJoy[0] != 255)
        {
            leftJoyStick.x_axis = (leftJoy[0] - 128) / 128.0f;
        }
        else
        {
            leftJoyStick.x_axis = 1;
        }

        if (leftJoy[1] != 255)
        {
            leftJoyStick.y_axis = (leftJoy[1] - 128) / 128.0f;
        }
        else
        {
            leftJoyStick.y_axis = 1;
        }

        leftJoyStick.trigger = leftJoy[2];

        if (rightJoy[0] != 255)
        {
            rightJoyStick.x_axis = (rightJoy[0] - 128) / 128.0f;
        }
        else
        {
            rightJoyStick.x_axis = 1;
        }

        if (rightJoy[1] != 255)
        {
            rightJoyStick.y_axis = (rightJoy[1] - 128) / 128.0f;
        }
        else
        {
            rightJoyStick.y_axis = 1;
        }

        rightJoyStick.trigger = rightJoy[2];

        ParseJoyStickButtonState(leftJoy[3], rightJoy[3]);
    }

    // Parse the button state of the PrioVR Hand Controllers
    void ParseJoyStickButtonState(short leftButtonState, short rightButtonState)
    {
        if( (leftButtonState & 1) != 0 )
        {
            leftJoyStick.x_button = 1;
        }
        else
        {
            leftJoyStick.x_button = 0;
        }

        if ((leftButtonState & 2) != 0)
        {
            leftJoyStick.a_button = 1;
        }
        else
        {
            leftJoyStick.a_button = 0;
        }

        if ((leftButtonState & 4) != 0)
        {
            leftJoyStick.b_button = 1;
        }
        else
        {
            leftJoyStick.b_button = 0;
        }

        if ((leftButtonState & 8) != 0)
        {
            leftJoyStick.y_button = 1;
        }
        else
        {
            leftJoyStick.y_button = 0;
        }

        if ((leftButtonState & 16) != 0)
        {
            leftJoyStick.top_button = 1;
        }
        else
        {
            leftJoyStick.top_button = 0;
        }

        if ((leftButtonState & 32) != 0)
        {
            leftJoyStick.joystick_button = 1;
        }
        else
        {
            leftJoyStick.joystick_button = 0;
        }

        if ((rightButtonState & 1) != 0)
        {
            rightJoyStick.x_button = 1;
        }
        else
        {
            rightJoyStick.x_button = 0;
        }

        if ((rightButtonState & 2) != 0)
        {
            rightJoyStick.a_button = 1;
        }
        else
        {
            rightJoyStick.a_button = 0;
        }

        if ((rightButtonState & 4) != 0)
        {
            rightJoyStick.b_button = 1;
        }
        else
        {
            rightJoyStick.b_button = 0;
        }

        if ((rightButtonState & 8) != 0)
        {
            rightJoyStick.y_button = 1;
        }
        else
        {
            rightJoyStick.y_button = 0;
        }

        if ((rightButtonState & 16) != 0)
        {
            rightJoyStick.top_button = 1;
        }
        else
        {
            rightJoyStick.top_button = 0;
        }

        if ((rightButtonState & 32) != 0)
        {
            rightJoyStick.joystick_button = 1;
        }
        else
        {
            rightJoyStick.joystick_button = 0;
        }
    }

    // Get the value of a PrioVR Joystick Axis
    public float GetJoyStickAxis(YOST_SKELETON_JOYSTICK_AXIS axis)
    {
        float axisState = 0;
        if (isStreaming)
        {
            switch (axis)
            {
                case YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_LEFT_X_AXIS:
                    axisState = leftJoyStick.x_axis;
                    break;
                case YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_LEFT_Y_AXIS:
                    axisState = leftJoyStick.y_axis;
                    break;
                case YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_X_AXIS:
                    axisState = rightJoyStick.x_axis;
                    break;
                case YOST_SKELETON_JOYSTICK_AXIS.YOST_SKELETON_RIGHT_Y_AXIS:
                    axisState = rightJoyStick.y_axis;
                    break;
            }
            return axisState;
        }

        byte axisData = 0;
        yost.getJoyStickAxisStatePrioProcessor(animateProcId, (byte)axis, out axisData);

        if (axisData < 128)
        {
            axisState = (axisData + 128) / -128.0f;
        }
        else if (axisData != 255)
        {
            axisState = (axisData - 128) / 128.0f;
        }
        else
        {
            axisState = 1;
        }

        return axisState;
    }

    // Get the current state of a PrioVR Hand Controller Button
    public byte GetJoyStickButton(YOST_SKELETON_JOYSTICK_BUTTON button)
    {
        if (isStreaming)
        {
            switch (button)
            {
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON:
                    return leftJoyStick.x_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_A_BUTTON:
                    return leftJoyStick.a_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_B_BUTTON:
                    return leftJoyStick.b_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_Y_BUTTON:
                    return leftJoyStick.y_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON:
                    return leftJoyStick.top_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON:
                    return leftJoyStick.joystick_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_X_BUTTON:
                    return rightJoyStick.x_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_A_BUTTON:
                    return rightJoyStick.a_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_B_BUTTON:
                    return rightJoyStick.b_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_Y_BUTTON:
                    return rightJoyStick.y_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON:
                    return rightJoyStick.top_button;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON:
                    return rightJoyStick.joystick_button;
                default:
                    return 255;
            }
        }

        byte buttonData = 255;
        yost.getJoyStickButtonStatePrioProcessor(animateProcId, (byte)button, out buttonData);
        return buttonData;
    }

    // Get whether or not the PrioVR Hand Controller was pressed this frame
    public bool GetJoyStickButtonDown(YOST_SKELETON_JOYSTICK_BUTTON button)
    {
        if (isStreaming)
        {
            switch (button)
            {
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON:
                    if ((leftJoyStick.x_button == 1) && (leftJoyStickLastFrame.x_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_A_BUTTON:
                    if ((leftJoyStick.a_button == 1) && (leftJoyStickLastFrame.a_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_B_BUTTON:
                    if ((leftJoyStick.b_button == 1) && (leftJoyStickLastFrame.b_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_Y_BUTTON:
                    if ((leftJoyStick.y_button == 1) && (leftJoyStickLastFrame.y_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON:
                    if ((leftJoyStick.top_button == 1) && (leftJoyStickLastFrame.top_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON:
                    if ((leftJoyStick.joystick_button == 1) && (leftJoyStickLastFrame.joystick_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_X_BUTTON:
                    if ((rightJoyStick.x_button == 1) && (rightJoyStickLastFrame.x_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_A_BUTTON:
                    if ((rightJoyStick.a_button == 1) && (rightJoyStickLastFrame.a_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_B_BUTTON:
                    if ((rightJoyStick.b_button == 1) && (rightJoyStickLastFrame.b_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_Y_BUTTON:
                    if ((rightJoyStick.y_button == 1) && (rightJoyStickLastFrame.y_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON:
                    if ((rightJoyStick.top_button == 1) && (rightJoyStickLastFrame.top_button != 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON:
                    if ((rightJoyStick.joystick_button == 1) && (rightJoyStickLastFrame.joystick_button != 1))
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    // Get whether or not the PrioVR Hand Controller was released this frame
    public bool GetJoyStickButtonUp(YOST_SKELETON_JOYSTICK_BUTTON button)
    {
        if (isStreaming)
        {
            switch (button)
            {
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_X_BUTTON:
                    if ((leftJoyStick.x_button != 1) && (leftJoyStickLastFrame.x_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_A_BUTTON:
                    if ((leftJoyStick.a_button != 1) && (leftJoyStickLastFrame.a_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_B_BUTTON:
                    if ((leftJoyStick.b_button != 1) && (leftJoyStickLastFrame.b_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_Y_BUTTON:
                    if ((leftJoyStick.y_button != 1) && (leftJoyStickLastFrame.y_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_TOP_BUTTON:
                    if ((leftJoyStick.top_button != 1) && (leftJoyStickLastFrame.top_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_LEFT_JOYSTICK_BUTTON:
                    if ((leftJoyStick.joystick_button != 1) && (leftJoyStickLastFrame.joystick_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_X_BUTTON:
                    if ((rightJoyStick.x_button != 1) && (rightJoyStickLastFrame.x_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_A_BUTTON:
                    if ((rightJoyStick.a_button != 1) && (rightJoyStickLastFrame.a_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_B_BUTTON:
                    if ((rightJoyStick.b_button != 1) && (rightJoyStickLastFrame.b_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_Y_BUTTON:
                    if ((rightJoyStick.y_button != 1) && (rightJoyStickLastFrame.y_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_TOP_BUTTON:
                    if ((rightJoyStick.top_button != 1) && (rightJoyStickLastFrame.top_button == 1))
                    {
                        return true;
                    }
                    break;
                case YOST_SKELETON_JOYSTICK_BUTTON.YOST_SKELETON_RIGHT_JOYSTICK_BUTTON:
                    if ((rightJoyStick.joystick_button != 1) && (rightJoyStickLastFrame.joystick_button == 1))
                    {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    // Cleanup the PrioVR Suit, and Reset the API
    void OnDestroy()
    {
        StopStreaming();
        yost.stopControllerUpdating(animateProcId);
        if (playerNumber == 0)
        {
            yost.resetSkeletalAPI();
        }
    }
}