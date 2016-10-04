using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace SaveDataManagerForEditor
{
    public class SaveDataManagerForEditor : EditorWindow
    {
        private string[] selectTabs = { "Cache","SaveData","PlayerPrefs"};
        private int selectTab = -1;
        private string[] pathList;

        [MenuItem("Tools/SaveDataManager")]
        public static void CreateWindow()
        {
            EditorWindow.GetWindow<SaveDataManagerForEditor>();
        }

        void OnGUI()
        {
            int oldSelect = this.selectTab;
            if (this.selectTab < 0) { this.selectTab = 0; }
            this.selectTab = GUILayout.Toolbar(selectTab, this.selectTabs);
            if (this.selectTab != oldSelect)
            {
                switch (this.selectTab)
                {
                    case 0:
                        OnInitCache();
                        break;
                    case 1:
                        OnInitSaveData();
                        break;
                    case 2:
                        OnInitPlayerPref();
                        break;
                }
            }
            switch (this.selectTab)
            {
                case 0:
                    OnGUICache();
                    break;
                case 1:
                    OnGUISaveData();
                    break;
                case 2:
                    OnGUIPlayerPrefs();
                    break;
            }
        }
        private void OnInitCache()
        {
            this.CreateFileList(Application.temporaryCachePath);
        }
        private void OnInitSaveData()
        {
            this.CreateFileList(Application.persistentDataPath);
        }
        private void OnInitPlayerPref()
        {

        }

        private void OnGUIPathes()
        {
            EditorGUILayout.LabelField("Application.temporaryCachePath " + Application.temporaryCachePath);
            if (GUILayout.Button("Open"))
            {
                OpenInExplorer(Application.temporaryCachePath);
            }
            EditorGUILayout.LabelField("Application.persistentDataPath " + Application.persistentDataPath);
            if (GUILayout.Button("Open"))
            {
                OpenInExplorer(Application.persistentDataPath);
            }
        }

        private void OnGUICache()
        {

            EditorGUILayout.LabelField("Application.temporaryCachePath ");
            EditorGUILayout.LabelField( Application.temporaryCachePath);

            if (GUILayout.Button("Open"))
            {
                OpenInExplorer(Application.temporaryCachePath);
            }
            if (this.pathList == null)
            {
                return;
            }
            foreach (var file in this.pathList)
            {
                EditorGUILayout.LabelField(file);
            }
        }


        private void OnGUISaveData()
        {

            EditorGUILayout.LabelField("Application.persistentDataPath ");
            EditorGUILayout.LabelField(Application.persistentDataPath);

            if (GUILayout.Button("Open"))
            {
                OpenInExplorer(Application.persistentDataPath);
            }
            if (this.pathList == null)
            {
                return;
            }
            foreach (var file in this.pathList)
            {
                EditorGUILayout.LabelField(file);
            }
        }

        private void OnGUIPlayerPrefs()
        {
        }

        private void CreateFileList(string path)
        {
            this.pathList = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
            for (int i = 0; i < this.pathList.Length; ++i)
            {
                string str = pathList[i].Substring(path.Length);
                pathList[i] = str;
            }
        }

        private void OpenInExplorer(string path)
        {
#if UNITY_EDITOR_WIN
            path = path.Replace("/", "\\");
            Process.Start("explorer.exe", "/select," + path);
#endif
        }
    }
}