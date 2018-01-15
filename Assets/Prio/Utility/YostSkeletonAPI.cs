using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace YostSkeletalAPI
{

    /********************************************//**
     * An enum representing the standard humanoid Skeleton/Prio Layouts.
    ***********************************************/
    public enum PrioSuitLayout
    {
	    LAYOUT_PRO,
	    LAYOUT_CORE,
	    LAYOUT_LITE,
	    LAYOUT_CHEST
    }
	
    public enum YostSkeletonIdMask
    {
	    YOST_SKELETON_INVALID_ID     = 0x0040000,    /**< Invalid ID */
	    YOST_SKELETON_ID             = 0x0080000,    /**< Skeleton ID */
	    YOST_SKELETON_PROCESSOR_ID   = 0x01000000    /**< Processor ID */
    };

    /********************************************//**
     * \ingroup skel_api
     * An enum expressing the different kinds of units a skeleton can have.
    ***********************************************/
    public enum YOST_SKELETON_UNIT
    {
        YOST_SKELETON_UNIT_CENTIMETERS,  /**< This skeleton's units are in centimeters */
        YOST_SKELETON_UNIT_METERS,       /**< This skeleton's units are in meters */
        YOST_SKELETON_UNIT_INCHES,       /**< This skeleton's units are in inches */
        YOST_SKELETON_UNIT_FEET,         /**< This skeleton's units are in feet */
    };

    /********************************************//**
     * An enum expressing the different types of errors a 3-Space API call can return.
     ***********************************************/
    public enum YOST_SKELETON_ERROR
    {
        YOST_SKELETON_NO_ERROR,                                  /**< The API call successfully executed */
        YOST_SKELETON_ERROR_SKELETON_NOT_FOUND,                  /**< Invalid skeleton index given */
        YOST_SKELETON_ERROR_PROCESSOR_NOT_FOUND,                 /**< Invalid processor index given */
        YOST_SKELETON_ERROR_BONE_NOT_FOUND,                      /**< Invalid bone name given */
        YOST_SKELETON_ERROR_PARENT_BONE_NOT_FOUND,               /**< Invalid parent bone name given */
        YOST_SKELETON_ERROR_NO_ROOT_BONE,                        /**< Skeleton has no root bone */
        YOST_SKELETON_ERROR_UPDATE_NOT_NEEDED,                   /**< No new data to update skeleton with */
        YOST_SKELETON_ERROR_ANCHOR_SENSOR_MISSING,               /**< The anchor sensor is missing during calibration */
        YOST_SKELETON_ERROR_EMPTY_NAME,                          /**< Given name was empty */
        YOST_SKELETON_ERROR_DUPLICATE_BONE,                      /**< Bone with given name already exists in skeleton */
        YOST_SKELETON_ERROR_DUPLICATE_ROOT_BONE,                 /**< Skeleton already has a root bone */
        YOST_SKELETON_ERROR_HIERARCHY_NO_ROOT_NODE,              /**< There is no root node in hierarchy */
        YOST_SKELETON_ERROR_HIERARCHY_NO_SKELETON_NODE,          /**< There is no skeleton node in hierarchy */
        YOST_SKELETON_ERROR_HIERARCHY_NO_ROOT_BONE_NODE,         /**< There is no root bone node in hierarchy */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_NAME,              /**< Node in hierarchy has no name */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_UNIT,              /**< Node in hierarchy has no unit */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_OFFSET,            /**< Node in hierarchy has no offset */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_LENGTH,            /**< Node in hierarchy has no length */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_MASS,              /**< Node in hierarchy has no mass */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_POSE_ORIENTATION,  /**< Node in hierarchy has no pose orientation */
        YOST_SKELETON_ERROR_HIERARCHY_NODE_NO_UPDATE_TYPE,       /**< Node in hierarchy has no update type */
        YOST_SKELETON_ERROR_NO_MORE_DATA,                        /**< Indicates that all data in a list has been read */
        YOST_SKELETON_ERROR_BUFFER_TOO_SMALL,                    /**< Buffer could not hold the data */
        YOST_SKELETON_ERROR_FILE_READ,                           /**< File could not be read */
        YOST_SKELETON_ERROR_FILE_WRITE,                          /**< File could not be written to*/
        YOST_SKELETON_ERROR_INDEX_OUT_OF_RANGE,                  /**< Index is out of range */
        YOST_SKELETON_ERROR_PROCESSOR_COMMAND,                   /**< Processor command failed */
        YOST_SKELETON_ERROR_PROCESSOR_IN_USE,                    /**< Processor is being used by a skeleton */
        YOST_SKELETON_ERROR_JOYSTICK_INACTIVE,                   /**< The joystick is not plugged in */
        YOST_SKELETON_ERROR_RESET_API                            /**< The API failed to reset itself */
    }

    /********************************************//**
    * An enum expressing the standard names of bones for a skeleton.
    ***********************************************/
    public enum YOST_SKELETON_BONE
    {
        YOST_SKELETON_BONE_HIPS,                    /**< This will get the standard bone name for the hips */
        YOST_SKELETON_BONE_LOWER_SPINE_2,           /**< This will get the standard bone name for the second lower spine */
        YOST_SKELETON_BONE_LOWER_SPINE_1,           /**< This will get the standard bone name for the first lower spine */
        YOST_SKELETON_BONE_SPINE,                   /**< This will get the standard bone name for the spine */
        YOST_SKELETON_BONE_NECK,                    /**< This will get the standard bone name for the neck */
        YOST_SKELETON_BONE_HEAD,                    /**< This will get the standard bone name for the head */
        YOST_SKELETON_BONE_HEAD_END,                /**< This will get the standard bone name for the head end */
        YOST_SKELETON_BONE_LEFT_SHOULDER,           /**< This will get the standard bone name for the left shoulder */
        YOST_SKELETON_BONE_LEFT_UPPER_ARM,          /**< This will get the standard bone name for the left upper arm */
        YOST_SKELETON_BONE_LEFT_LOWER_ARM,          /**< This will get the standard bone name for the left lower arm */
        YOST_SKELETON_BONE_LEFT_HAND,               /**< This will get the standard bone name for the left hand */
        YOST_SKELETON_BONE_LEFT_HAND_THUMB_1,       /**< This will get the standard bone name for the first left hand thumb */
        YOST_SKELETON_BONE_LEFT_HAND_THUMB_2,       /**< This will get the standard bone name for the second left hand thumb */
        YOST_SKELETON_BONE_LEFT_HAND_THUMB_3,       /**< This will get the standard bone name for the third left hand thumb */
        YOST_SKELETON_BONE_LEFT_HAND_THUMB_END,     /**< This will get the standard bone name for the end left hand thumb */
        YOST_SKELETON_BONE_LEFT_HAND_INDEX_1,       /**< This will get the standard bone name for the first left hand index */
        YOST_SKELETON_BONE_LEFT_HAND_INDEX_2,       /**< This will get the standard bone name for the second left hand index */
        YOST_SKELETON_BONE_LEFT_HAND_INDEX_3,       /**< This will get the standard bone name for the third left hand index */
        YOST_SKELETON_BONE_LEFT_HAND_INDEX_END,     /**< This will get the standard bone name for the end left hand index */
        YOST_SKELETON_BONE_LEFT_HAND_MIDDLE_1,      /**< This will get the standard bone name for the first left hand middle */
        YOST_SKELETON_BONE_LEFT_HAND_MIDDLE_2,      /**< This will get the standard bone name for the second left hand middle */
        YOST_SKELETON_BONE_LEFT_HAND_MIDDLE_3,      /**< This will get the standard bone name for the third left hand middle */
        YOST_SKELETON_BONE_LEFT_HAND_MIDDLE_END,    /**< This will get the standard bone name for the end left hand middle */
        YOST_SKELETON_BONE_LEFT_HAND_RING_1,        /**< This will get the standard bone name for the first left hand ring */
        YOST_SKELETON_BONE_LEFT_HAND_RING_2,        /**< This will get the standard bone name for the second left hand ring */
        YOST_SKELETON_BONE_LEFT_HAND_RING_3,        /**< This will get the standard bone name for the third left hand ring */
        YOST_SKELETON_BONE_LEFT_HAND_RING_END,      /**< This will get the standard bone name for the end left hand ring */
        YOST_SKELETON_BONE_LEFT_HAND_PINKY_1,       /**< This will get the standard bone name for the first left hand pinky */
        YOST_SKELETON_BONE_LEFT_HAND_PINKY_2,       /**< This will get the standard bone name for the second left hand pinky */
        YOST_SKELETON_BONE_LEFT_HAND_PINKY_3,       /**< This will get the standard bone name for the third left hand pinky */
        YOST_SKELETON_BONE_LEFT_HAND_PINKY_END,     /**< This will get the standard bone name for the end left hand pinky */
        YOST_SKELETON_BONE_LEFT_JOYSTICK,           /**< This will get the standard bone name for the left joystick */
        YOST_SKELETON_BONE_RIGHT_SHOULDER,          /**< This will get the standard bone name for the right shoulder */
        YOST_SKELETON_BONE_RIGHT_UPPER_ARM,         /**< This will get the standard bone name for the right upper arm */
        YOST_SKELETON_BONE_RIGHT_LOWER_ARM,         /**< This will get the standard bone name for the right lower arm */
        YOST_SKELETON_BONE_RIGHT_HAND,              /**< This will get the standard bone name for the right hand */
        YOST_SKELETON_BONE_RIGHT_HAND_THUMB_1,      /**< This will get the standard bone name for the first right hand thumb */
        YOST_SKELETON_BONE_RIGHT_HAND_THUMB_2,      /**< This will get the standard bone name for the second right hand thumb */
        YOST_SKELETON_BONE_RIGHT_HAND_THUMB_3,      /**< This will get the standard bone name for the third right hand thumb */
        YOST_SKELETON_BONE_RIGHT_HAND_THUMB_END,    /**< This will get the standard bone name for the end right hand thumb */
        YOST_SKELETON_BONE_RIGHT_HAND_INDEX_1,      /**< This will get the standard bone name for the first right hand index */
        YOST_SKELETON_BONE_RIGHT_HAND_INDEX_2,      /**< This will get the standard bone name for the second right hand index */
        YOST_SKELETON_BONE_RIGHT_HAND_INDEX_3,      /**< This will get the standard bone name for the third right hand index */
        YOST_SKELETON_BONE_RIGHT_HAND_INDEX_END,    /**< This will get the standard bone name for the end right hand index */
        YOST_SKELETON_BONE_RIGHT_HAND_MIDDLE_1,     /**< This will get the standard bone name for the first right hand middle */
        YOST_SKELETON_BONE_RIGHT_HAND_MIDDLE_2,     /**< This will get the standard bone name for the second right hand middle */
        YOST_SKELETON_BONE_RIGHT_HAND_MIDDLE_3,     /**< This will get the standard bone name for the third right hand middle */
        YOST_SKELETON_BONE_RIGHT_HAND_MIDDLE_END,   /**< This will get the standard bone name for the end right hand middle */
        YOST_SKELETON_BONE_RIGHT_HAND_RING_1,       /**< This will get the standard bone name for the first right hand ring */
        YOST_SKELETON_BONE_RIGHT_HAND_RING_2,       /**< This will get the standard bone name for the second right hand ring */
        YOST_SKELETON_BONE_RIGHT_HAND_RING_3,       /**< This will get the standard bone name for the third right hand ring */
        YOST_SKELETON_BONE_RIGHT_HAND_RING_END,     /**< This will get the standard bone name for the end right hand ring */
        YOST_SKELETON_BONE_RIGHT_HAND_PINKY_1,      /**< This will get the standard bone name for the first right hand pinky */
        YOST_SKELETON_BONE_RIGHT_HAND_PINKY_2,      /**< This will get the standard bone name for the second right hand pinky */
        YOST_SKELETON_BONE_RIGHT_HAND_PINKY_3,      /**< This will get the standard bone name for the third right hand pinky */
        YOST_SKELETON_BONE_RIGHT_HAND_PINKY_END,    /**< This will get the standard bone name for the end right hand pinky */
        YOST_SKELETON_BONE_RIGHT_JOYSTICK,           /**< This will get the standard bone name for the right joystick */
        YOST_SKELETON_BONE_LEFT_UPPER_LEG,          /**< This will get the standard bone name for the left upper leg */
        YOST_SKELETON_BONE_LEFT_LOWER_LEG,          /**< This will get the standard bone name for the left lower leg */
        YOST_SKELETON_BONE_LEFT_FOOT,               /**< This will get the standard bone name for the left foot */
        YOST_SKELETON_BONE_LEFT_FOOT_HEEL,          /**< This will get the standard bone name for the left foot heel */
        YOST_SKELETON_BONE_LEFT_FOOT_BALL,          /**< This will get the standard bone name for the left foot ball */
        YOST_SKELETON_BONE_RIGHT_UPPER_LEG,         /**< This will get the standard bone name for the right upper leg */
        YOST_SKELETON_BONE_RIGHT_LOWER_LEG,         /**< This will get the standard bone name for the right lower leg */
        YOST_SKELETON_BONE_RIGHT_FOOT,              /**< This will get the standard bone name for the right foot */
        YOST_SKELETON_BONE_RIGHT_FOOT_HEEL,         /**< This will get the standard bone name for the right foot heel */
        YOST_SKELETON_BONE_RIGHT_FOOT_BALL,         /**< This will get the standard bone name for the right foot ball */
        YOST_SKELETON_BONE_NULL
    };


    public class YOST_JOYSTICK
    {
        public float x_axis = 0;
        public float y_axis = 0;

        public byte trigger = 0;

        public byte x_button = 0;
        public byte a_button = 0;
        public byte b_button = 0;
        public byte y_button = 0;
        public byte top_button = 0;
        public byte joystick_button = 0;
    }

    public enum YOST_SKELETON_JOYSTICK_AXIS
    {
        YOST_SKELETON_LEFT_X_AXIS,
        YOST_SKELETON_LEFT_Y_AXIS,
        YOST_SKELETON_RIGHT_X_AXIS,
        YOST_SKELETON_RIGHT_Y_AXIS
    };

    public enum YOST_SKELETON_JOYSTICK_BUTTON
    {
        YOST_SKELETON_LEFT_X_BUTTON,
        YOST_SKELETON_LEFT_A_BUTTON,
        YOST_SKELETON_LEFT_B_BUTTON,
        YOST_SKELETON_LEFT_Y_BUTTON,
        YOST_SKELETON_LEFT_TOP_BUTTON,
        YOST_SKELETON_LEFT_JOYSTICK_BUTTON,
        YOST_SKELETON_LEFT_TRIGGER,
        YOST_SKELETON_RIGHT_X_BUTTON,
        YOST_SKELETON_RIGHT_A_BUTTON,
        YOST_SKELETON_RIGHT_B_BUTTON,
        YOST_SKELETON_RIGHT_Y_BUTTON,
        YOST_SKELETON_RIGHT_TOP_BUTTON,
        YOST_SKELETON_RIGHT_JOYSTICK_BUTTON,
        YOST_SKELETON_RIGHT_TRIGGER
};
	
	public static class yost
	{
		#if UNITY_STANDALONE_WIN_32 || UNITY_EDITOR_32
		    const string  YOSTSkeletonPlugin = "YOSTSkeletonPlugin_x32";
		#elif UNITY_STANDALONE_WIN_64 || UNITY_EDITOR_64
		    const string YOSTSkeletonPlugin = "YOSTSkeletonPlugin_x64";
#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            const string YOSTSkeletonPlugin = "YOSTSkeletonPlugin_OSX";
#endif

        // Skeleton Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setSkeletonUnit")]
        public static extern uint setSkeletonUnit(uint skel_id, YOST_SKELETON_UNIT unit );

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_createStandardSkeletonWithAge")]
		public static extern uint createStandardSkeletonWithAge(byte male, uint age);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_createStandardSkeletonWithHeight")]
		public static extern uint createStandardSkeletonWithHeight(byte male, float height);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setSkeletonToStandardTPose")]
		public static extern YOST_SKELETON_ERROR setSkeletonToStandardTPose(uint skel_id);
		
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setSkeletonToStandardClaspedPose")]
		public static extern YOST_SKELETON_ERROR setSkeletonToStandardClaspedPose(uint skel_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_loadDeviceXMLMap")]
		public static extern YOST_SKELETON_ERROR loadDeviceXmlMapCustomLayout(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 512)] byte[] file_name);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setStandardDeviceXmlMapPrioLiteLayout")]
		public static extern YOST_SKELETON_ERROR setStandardDeviceXmlMapPrioLiteLayout(uint skel_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setStandardDeviceXmlMapPrioCoreLayout")]
		public static extern YOST_SKELETON_ERROR setStandardDeviceXmlMapPrioCoreLayout(uint skel_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setStandardDeviceXmlMapPrioProLayout")]
		public static extern YOST_SKELETON_ERROR setStandardDeviceXmlMapPrioProLayout(uint skel_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_addProcessorToSkeleton")]
		public static extern YOST_SKELETON_ERROR addProcessorToSkeleton(uint skel_id, uint index, uint proc_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_addBoneAlias")]
		public static extern YOST_SKELETON_ERROR addBoneAlias(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] alias_name, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_getStandardBoneName")]
		public static extern YOST_SKELETON_ERROR getStandardBoneName(YOST_SKELETON_BONE bone, [Out][MarshalAs(UnmanagedType.LPArray, SizeConst = 32)] byte[] error_list, uint name_len);
        
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_setBoneOrientationOffset")]
		public static extern YOST_SKELETON_ERROR setBoneOrientationOffset(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, [In,Out] float[] data);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_getBoneOrientationOffset")]
		public static extern YOST_SKELETON_ERROR getBoneOrientationOffset(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, [In,Out] float[] data);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_getBoneOrientation")]
		public static extern YOST_SKELETON_ERROR getBoneOrientation(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, [In, Out] float[] data);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_getBoneVelocity")]
		public static extern YOST_SKELETON_ERROR getBoneVelocity(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, [In,Out] float[] data);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint= "yostskel_getBonePosition")]
		public static extern YOST_SKELETON_ERROR getBonePosition(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_setBoneFusedMode")]
        public static extern YOST_SKELETON_ERROR setBoneFusedMode(uint skel_id, bool fused);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getBoneFusedMode")]
        public static extern YOST_SKELETON_ERROR getBoneFusedMode(uint skel_id, out bool fused);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_utilityRegisterBone")]
        public static extern YOST_SKELETON_ERROR utilityRegisterBone(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_utilityDeregisterBone")]
        public static extern YOST_SKELETON_ERROR utilityDeregisterBone(uint skel_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getAllBoneOrientationOffset")]
        public static extern YOST_SKELETON_ERROR getAllBoneOrientationOffset(uint skel_id, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getAllBoneOrientations")]
        public static extern YOST_SKELETON_ERROR getAllBoneOrientations(uint skel_id, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getAllBonePositions")]
        public static extern YOST_SKELETON_ERROR getAllBonePositions(uint skel_id, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getAllBoneVelocities")]
        public static extern YOST_SKELETON_ERROR getAllBoneVelocities(uint skel_id, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getAllBoneAccelerations")]
        public static extern YOST_SKELETON_ERROR getAllBoneAccelerations(uint skel_id, [In, Out] float[] data);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_hasUpdated")]
        public static extern YOST_SKELETON_ERROR hasUpdated(uint skel_id, out bool has_changed);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_update")]
        public static extern YOST_SKELETON_ERROR update(uint skel_id);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_resetSkeletalApi")]
        public static extern YOST_SKELETON_ERROR resetSkeletalAPI();


        // PrioVR Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_createPrioProcessorWithComOffset")]
		public static extern uint createPrioProcessor(uint device_type, uint com_offset);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_calibratePrioProcessor")]
		public static extern YOST_SKELETON_ERROR calibratePrioProcessor(uint prio_proc_id, float wait_time);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_isCalibrated")]
        public static extern YOST_SKELETON_ERROR isCalibrated(uint skel_id, out bool calibrated);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_hasPrioProcessorUpdatedState")]
        public static extern YOST_SKELETON_ERROR hasPrioProcessorUpdatedState(uint prio_proc_id, out bool changed);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_setupStandardStreamPrioProcessor")]
		public static extern YOST_SKELETON_ERROR setupStandardStreamPrioProcessor(uint prio_proc_id);
		
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_startPrioProcessor")]
		public static extern YOST_SKELETON_ERROR startPrioProcessor(uint prio_proc_id);
		
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_stopPrioProcessor")]
		public static extern YOST_SKELETON_ERROR stopPrioProcessor(uint prio_proc_id);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getHubBatteryLevel")]
        public static extern YOST_SKELETON_ERROR getHubBatteryLevel(uint prio_proc_id, out byte battery_level);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getHubBatteryStatus")]
        public static extern YOST_SKELETON_ERROR getHubBatteryStatus(uint prio_proc_id, out byte battery_status);

        // Suit input Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getHubButtonPrioProcessor")]
        public static extern YOST_SKELETON_ERROR getPrioHubButtonState(uint prio_proc_id, out byte buttonState);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yostskel_getHubButtonByIndexPrioProcessor")]
        public static extern YOST_SKELETON_ERROR getHubButtonByIndexPrioProcessor(uint prio_proc_id, byte button_index, out byte button_state);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getJoyStickStatePrioProcessor")]
        public static extern YOST_SKELETON_ERROR getJoyStickStatePrioProcessor(uint prio_proc_id, [In, Out] byte[] left_joystick_state, [In,Out] byte[] right_joystick_state);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getJoyStickStateByIndexPrioProcessor")]
        public static extern YOST_SKELETON_ERROR getJoyStickStateByIndexPrioProcessor(uint prio_proc_id, byte joystick_index, [In, Out] byte[] joystick_state);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getJoyStickButtonStatePrioProcessor")]
        public static extern YOST_SKELETON_ERROR getJoyStickButtonStatePrioProcessor(uint prio_proc_id, byte button_index, out byte button_state);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getJoyStickAxisStatePrioProcessor")]
        public static extern YOST_SKELETON_ERROR getJoyStickAxisStatePrioProcessor(uint prio_proc_id, byte axis_index, out byte axis_state);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getActiveJoySticksPrioProcessor")]
        public static extern YOST_SKELETON_ERROR getActiveJoySticksPrioProcessor(uint prio_proc_id, out byte active_joysticks);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_isJoystickActiveByIndexPrioProcessor")]
        public static extern YOST_SKELETON_ERROR isJoystickActiveByIndexPrioProcessor(uint prio_proc_id, byte joystick_index, out bool active);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setControllerUpdateRate")]
        public static extern YOST_SKELETON_ERROR setControllerUpdateRate(uint prio_proc_id, uint update_rate);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getControllerUpdateRate")]
        public static extern YOST_SKELETON_ERROR getControllerUpdateRate(uint prio_proc_id, out uint update_rate);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_startControllerUpdating")]
        public static extern YOST_SKELETON_ERROR startControllerUpdating(uint prio_proc_id);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_stopControllerUpdating")]
        public static extern YOST_SKELETON_ERROR stopControllerUpdating(uint prio_proc_id);

        // Pedestrian Tracking Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_createPedestrianTrackingProcessor")]
        public static extern uint createPedestrianTrackingProcessor();

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_resetPedestrianTrackingProcessor")]
		public static extern YOST_SKELETON_ERROR resetPedestrianTrackingProcessor(uint proc_id);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getSkeletonTranslation")]
        public static extern YOST_SKELETON_ERROR getSkeletonTranslation(uint proc_id, [In, Out] float[] translation_vector);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getSkeletonTranslationAsPrecentageOfHeight")]
        public static extern YOST_SKELETON_ERROR getSkeletonTranslationAsPrecentageOfHeight(uint proc_id, [In, Out] float[] translation_vector);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_startTrackingBonePedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR startTrackingBonePedestrianTrackingProcessor(uint ped_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_stopTrackingBonePedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR stopTrackingBonePedestrianTrackingProcessor(uint ped_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setMaxCertaintyPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR setMaxCertaintyPedestrianTrackingProcessor(uint ped_proc_id, float certainty);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getMaxCertaintyPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR getMaxCertaintyPedestrianTrackingProcessor(uint ped_proc_id, out float certainty);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setVarianceMultiplyFactorPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR setVarianceMultiplyFactorPedestrianTrackingProcessor(uint ped_proc_id, float multiply_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getVarianceMultiplyFactorPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR getVarianceMultiplyFactorPedestrianTrackingProcessor(uint ped_proc_id, out float multiply_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setVarianceDataLengthPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR setVarianceDataLengthPedestrianTrackingProcessor(uint ped_proc_id, int variance_length);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getVarianceDataLengthPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR getVarianceDataLengthPedestrianTrackingProcessor(uint ped_proc_id, out int variance_length);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getPinnedBoneDataPedestrianTrackingProcessor")]
        public static extern YOST_SKELETON_ERROR getPinnedBoneDataPedestrianTrackingProcessor(uint proc_id, [In, Out] float[] anchor_point, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, int bone_name_length);


        // Smoothing Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_createSmoothingProcessor")]
        public static extern uint createSmoothingProcessor();

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllMinSmoothingFactorSmoothingProcessor")]
		public static extern YOST_SKELETON_ERROR setAllMinSmoothingFactorSmoothingProcessor(uint smooth_proc_id, float min_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllMaxSmoothingFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setAllMaxSmoothingFactorSmoothingProcessor(uint smooth_proc_id, float max_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllLowerVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setAllLowerVarianceBoundSmoothingProcessor(uint smooth_proc_id, float lower_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllUpperVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setAllUpperVarianceBoundSmoothingProcessor(uint smooth_proc_id, float upper_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllVarianceMultiplyFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setAllVarianceMultiplyFactorSmoothingProcessor(uint smooth_proc_id, float multiply_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setAllVarianceDataLengthSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setAllVarianceDataLengthSmoothingProcessor(uint smooth_proc_id, int variance_length);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneMinSmoothingFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneMinSmoothingFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[]  bone_name, float min_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneMinSmoothingFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneMinSmoothingFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out float min_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneMaxSmoothingFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneMaxSmoothingFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, float max_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneMaxSmoothingFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneMaxSmoothingFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out float max_smoothing_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneLowerVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneLowerVarianceBoundSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, float lower_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneLowerVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneLowerVarianceBoundSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out float lower_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneUpperVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneUpperVarianceBoundSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, float upper_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneUpperVarianceBoundSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneUpperVarianceBoundSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out float upper_variance_bound);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneVarianceMultiplyFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneVarianceMultiplyFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, float multiply_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneVarianveMultiplyFactorSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneVarianveMultiplyFactorSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out float multiply_factor);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_setBoneVarianceDataLengthSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR setBoneVarianceDataLengthSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, int variance_length);

        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint = "yostskel_getBoneVarianceDataLengthSmoothingProcessor")]
        public static extern YOST_SKELETON_ERROR getBoneVarianceDataLengthSmoothingProcessor(uint smooth_proc_id, [MarshalAs(UnmanagedType.LPArray, SizeConst = 32)]byte[] bone_name, out int variance_length);

        // TSS Functions
        [DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_createTssProcessor")]
		public static extern uint createTssProcessor();
		
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_calibrateTssProcessor")]
		public static extern YOST_SKELETON_ERROR calibrateTssProcessor(uint proc_id);

		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_startTssProcessor")]
		public static extern YOST_SKELETON_ERROR startTssProcessor(uint proc_id);
		
		[DllImport(YOSTSkeletonPlugin, CallingConvention = CallingConvention.Cdecl, EntryPoint= "yostskel_stopTssProcessor")]
		public static extern YOST_SKELETON_ERROR stopTssProcessor(uint proc_id);
	}
}