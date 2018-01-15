using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(YostSkeletonRig))]
public class YeiRigEditor : Editor
{
	SerializedObject m_target;
	YostSkeletonRig myTarget;

	void OnEnable()
	{
		m_target = new SerializedObject(target);
		myTarget = (YostSkeletonRig)target;
    }

	public override void OnInspectorGUI()
	{

        if (GUILayout.Button("Settings"))
		{
			YostRigSettingsWindow.Init();
		}

        if(GUI.changed)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }

        m_target.ApplyModifiedProperties();
		m_target.Update();
	}
}