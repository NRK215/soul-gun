using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EX3.Framework;
using System.IO;
using System.Text;

namespace EX3.Framework.FSM
{
    public class FSMAssetCreator : EditorWindow
    {
        [MenuItem("Assets/Create/[EX3] Framework/Create FSM State C# class")]
        public static void CreateFSMState()
        {
            // First, localize the EX3.Framework folder:
            string[] folders = Directory.GetDirectories(Application.dataPath, "*EX3.Framework*", SearchOption.AllDirectories);
            if (folders.Length > 0)
            {
                string rootFrameworkPath = folders[0];

                // Get the template file path:
                string templateFile = string.Format("{0}/FSM/Templates/{1}", rootFrameworkPath, "FSMStateTemplate.cs");

                if (File.Exists(templateFile))
                {
                    // Get the current path of selected object in Unity3D editor:
                    string destPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                    if (destPath == "")
                    {
                        destPath = "Assets";
                    }
                    else if (Path.GetExtension(destPath) != "")
                    {
                        destPath = destPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
                    }

                    string newFile = EditorUtility.SaveFilePanel("New FSM State", destPath, "New FSM State.cs", "cs");

                    if (!string.IsNullOrEmpty(newFile))
                    {
                        // Create the asset:
                        string contentTemplate = File.ReadAllText(templateFile);

                        // Replace wildcard class name with the new name:
                        contentTemplate = contentTemplate.Replace("FSMStateTemplate", Path.GetFileNameWithoutExtension(newFile));

                        // Save changes in new file on the selected location:
                        File.WriteAllText(newFile, contentTemplate);
                        
                        // Update Unity Editor:
                        AssetDatabase.Refresh();
                        EditorUtility.FocusProjectWindow();
                    }                    
                }
                else
                {
                    throw new DirectoryNotFoundException("Template FSM State not found. Reimport the package to fix it.");
                }
            }
            else
            {
                throw new DirectoryNotFoundException("The root [EX3] Framework not found. Reimport the package to fix it.");
            }
        }
    }
}