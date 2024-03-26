using System;
using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
    public class ParentTool : UnityEditor.EditorWindow
    {
        private GameObject parentObject;
        [MenuItem("MyTool/ParentTool")]   //窗口打开按钮所在位置，例如：点击Window，点击EditColor，就可以打开窗口。你也可以自己设计显示在别的栏
        static void ShowMyWindow()
        {
            ParentTool myWindow = EditorWindow.GetWindow<ParentTool>();//创建自定义窗口
            myWindow.Show();//显示创建的自定义窗口
        }

        [MenuItem("MyTool/SortChildByName")]
        static void SortChildByName()
        {
            var transform= Selection.transforms[0];
            var childs=transform.GetComponentsInChildren<Transform>();
                
            for (var i = 0; i < childs.Length; i++)
            {
                //忽略本体，getComponentInChildren包括自己本身
             if(i==0)
                 continue;
             var child = childs[i];
             Debug.Log("-----");
             Debug.Log("Converting "+child.name);
             var index=Convert.ToInt32(child.name);
             Debug.Log("set index"+(index-1));
             child.SetSiblingIndex(index-1);
             Debug.Log("-----");
            }
        }
        private void OnGUI()
        {
            GUILayout.Label("请选择parent");
            parentObject = (GameObject) EditorGUILayout.ObjectField("Parent GameObject", parentObject, typeof(GameObject), true);
            if (GUILayout.Button("Align"))
            {
                var transforms=  Selection.GetTransforms(SelectionMode.Deep);
                foreach (var transform in transforms)
                {
                    transform.SetParent(parentObject.transform);
                    Debug.Log("set"+transform.name+"parent as "+ parentObject.name);
                    transform.localPosition = Vector3.zero; 
                }                
            }
        }
    }
}