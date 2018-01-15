using UnityEngine;
using UnityEditor;

public class YostRigSettingsWindow : EditorWindow 
{
	private static YostRigSettingsWindow window = null;
	//private Camera _editorCamera = null;
	private bool showSkeletonProps = true;
	private int buttonSize = 18;
	private int labelSize = 110;

	string[] deviceList = {"PrioVR", "3Space"};

	string[] genderList = {"Male", "Female"};

    string[] activeList = { "Yes", "No" };


    private bool showPreview = true;
	GameObject target;
	Editor targetEditorPreview = null;
	YostSkeletonRig rig;
	GUIStyle titleStyle;

	// Bone Menu Items
	private bool showBones = true;
	private Vector2 showBonesPos = new Vector2(0,0);

    private bool showSmooth = true;
    private bool showPed = true;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Yost/Rig Settings")]
	public static void Init () 
	{
		window = GetWindow<YostRigSettingsWindow>("YostRigSettings Editor");
		window.Show();
		window.OnStart();
	}

	void OnStart()
	{	
		titleStyle = GUIStyle.none;

		titleStyle.fontSize = 20;
		titleStyle.alignment = TextAnchor.UpperCenter;

		GetSelected();
		window.Repaint();
	}

	void OnSelectionChange()
	{
		GetSelected();
		Repaint();
	}

	void OnGUI () 
	{
		if (target != null && window != null)
		{
			DisplaySkeletonOptions();
            DisplaySmoothingOptions();
            DisplayPedtrackingOptions();
            DisplayBoneOptions();
			DisplayPreview(Screen.width-10,300);
		}
		if (GUI.changed)
		{
            Repaint();
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
	}

	void DisplaySkeletonOptions()
	{
		EditorGUIUtility.labelWidth = 0;

		GUILayout.Label ("Yost Labs Skeleton Settings", titleStyle);

		EditorGUILayout.BeginVertical("Box");

		EditorGUILayout.BeginHorizontal();
		showSkeletonProps = EditorGUILayout.Foldout(showSkeletonProps, "Skeleton Properties");
		if (GUILayout.Button("?", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
		{
			Application.OpenURL("http://yostlabs.com/");
		}
		EditorGUILayout.EndHorizontal();
		if(showSkeletonProps)
		{
			EditorGUIUtility.labelWidth = labelSize;
			EditorGUILayout.BeginVertical("Box");
			
			rig.playerAge = EditorGUILayout.IntField("Age (years)", rig.playerAge);
			rig.playerHeight = EditorGUILayout.FloatField("Height (meters)", rig.playerHeight);

            EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Gender",GUILayout.Width(labelSize));

			rig.isMale = GUILayout.SelectionGrid(rig.isMale, genderList, 2, "radio", GUILayout.MaxWidth(Screen.width*0.8f-10));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Type",GUILayout.Width(labelSize));

			rig.isPrio = GUILayout.SelectionGrid(rig.isPrio, deviceList, 2, "radio", GUILayout.MaxWidth(Screen.width*0.8f-10));
			
			EditorGUILayout.EndHorizontal();

			if(rig.isPrio > 0)
			{
				rig.skeletonXML = (string)EditorGUILayout.TextField("XML:", rig.skeletonXML, GUILayout.MaxWidth(Screen.width));
			}
			else
			{	
				rig.playerNumber = (uint)EditorGUILayout.IntField("Player Number", (int)rig.playerNumber);
			}
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndVertical();
    }

    void DisplaySmoothingOptions()
    {
        EditorGUIUtility.labelWidth = 170;

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        showSmooth = EditorGUILayout.Foldout(showSmooth, "Smoothing Options");
        if (GUILayout.Button("?", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            Application.OpenURL("http://yostlabs.com/");
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Attach Processor?", GUILayout.Width(labelSize));

        rig.attachSmoothing = GUILayout.SelectionGrid(rig.attachSmoothing, activeList, 2, "radio", GUILayout.MaxWidth(Screen.width * 0.8f - 10));
        EditorGUILayout.EndHorizontal();


        if (showSmooth && rig.attachSmoothing != 1)
        {
            EditorGUILayout.BeginVertical("Box");

            rig.minSmoothingFactor = EditorGUILayout.FloatField("Minimum Smoothing Factor", rig.minSmoothingFactor);
            rig.maxSmoothingFactor = EditorGUILayout.FloatField("Maxium Smoothing Factor", rig.maxSmoothingFactor);
            rig.lowerVarianceBound = EditorGUILayout.FloatField("Lower Variance Bound", rig.lowerVarianceBound);
            rig.upperVarianceBound = EditorGUILayout.FloatField("Upper Variance Bound", rig.upperVarianceBound);
            rig.varianceMultiplyFactorSmooth = EditorGUILayout.FloatField("Variance Multiply Factor", rig.varianceMultiplyFactorSmooth);
            rig.varianceDataLengthSmooth = EditorGUILayout.IntField("Variance Data Length", rig.varianceDataLengthSmooth);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void DisplayPedtrackingOptions()
    {
        EditorGUIUtility.labelWidth = 150;

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.BeginHorizontal();
        showPed = EditorGUILayout.Foldout(showPed, "Pedestrian tracking Options");
        if (GUILayout.Button("?", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
        {
            Application.OpenURL("http://yostlabs.com/");
        }
        EditorGUILayout.EndHorizontal();

        if (showPed)
        {
            EditorGUILayout.BeginVertical("Box");

			rig.maximumCertainty = EditorGUILayout.FloatField("Maximun Certainty", rig.maximumCertainty);
            rig.varianceMultiplyFactorPed = EditorGUILayout.FloatField("Variance Multiply Factor", rig.varianceMultiplyFactorPed);
            rig.varianceDataLengthPed = EditorGUILayout.IntField("Variance Data Length", rig.varianceDataLengthPed);

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void DisplayBoneOptions()
	{
		EditorGUIUtility.labelWidth = 0;

		EditorGUILayout.BeginVertical("Box");
		EditorGUILayout.BeginHorizontal();
		showBones = EditorGUILayout.Foldout(showBones, "Bone Options");
		if (GUILayout.Button("?", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
		{
			Application.OpenURL("http://yostlabs.com/");
		}
		EditorGUILayout.EndHorizontal();
		if (showBones)
		{
			EditorGUIUtility.labelWidth = labelSize;

			rig.suitLayout = (YostSkeletalAPI.PrioSuitLayout) EditorGUILayout.EnumPopup("Rig Configuration:", rig.suitLayout);
			EditorGUILayout.BeginHorizontal();


			if(GUILayout.Button("Generate Skeleton", GUILayout.Width(Screen.width/2-10)))
			{
				rig.GenerateSkeleton();
			}
			if (GUILayout.Button("Clear Bones", GUILayout.Width(Screen.width/2-10)))
			{
				rig.bones.Clear();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginVertical();
			
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Add Bone", GUILayout.Width(Screen.width/2-10)))
			{
				rig.bones.Add(new YostSkeletonRig.SkeletonEntry());
			}
			
			if (GUILayout.Button("Remove Bone", GUILayout.Width(Screen.width/2-10)) && rig.bones.Count > 0) 
			{
				rig.bones.RemoveAt(rig.bones.Count-1);
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.EndVertical();

			if(rig.bones.Count > 0)
			{
				EditorGUILayout.BeginVertical("Box");

				EditorGUILayout.IntField("Number of Bones:", rig.bones.Count);

				showBonesPos = EditorGUILayout.BeginScrollView(showBonesPos, "Box");

				for(int i =0 ; i < rig.bones.Count; i++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.BeginVertical();
					rig.bones[i].type = (YostSkeletalAPI.YOST_SKELETON_BONE)EditorGUILayout.EnumPopup("Bone Type:", rig.bones[i].type);
					rig.bones[i].bone = (Transform) EditorGUILayout.ObjectField(rig.bones[i].bone, typeof(Transform), true);
					EditorGUILayout.EndVertical();
					if (GUILayout.Button("X", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize))) 
					{
						rig.bones.RemoveAt(i);
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndScrollView();
				EditorGUILayout.EndVertical();
			}
		}
		EditorGUILayout.EndVertical();
	}
	
	void DisplayPreview(int width, int height)
	{
		if (targetEditorPreview != null)
		{
			EditorGUILayout.BeginVertical("box");
			EditorGUILayout.BeginHorizontal();
			showPreview = EditorGUILayout.Foldout(showPreview, "Model Preview");
			if (GUILayout.Button("?", GUILayout.Width(buttonSize), GUILayout.Height(buttonSize)))
			{
				Application.OpenURL("http://yostlabs.com/");
			}
			EditorGUILayout.EndHorizontal();
			if(showPreview)
			{
				GUILayout.BeginVertical(GUILayout.ExpandHeight(true));
				targetEditorPreview.DrawPreview(GUILayoutUtility.GetRect(width, height));
				GUILayout.EndVertical();
			}
			EditorGUILayout.EndVertical();
		}
	}

	void GetSelected()
	{
		GameObject tempObj = (GameObject)Selection.activeGameObject;
		if(tempObj != null && tempObj.GetComponent<YostSkeletonRig>())
		{
			target = tempObj;
			rig = target.GetComponent<YostSkeletonRig>();
			targetEditorPreview = Editor.CreateEditor(target);

			if(window == null)
			{
				Init();
			}
		}
		else
		{
			target = null;
		}
	}
}