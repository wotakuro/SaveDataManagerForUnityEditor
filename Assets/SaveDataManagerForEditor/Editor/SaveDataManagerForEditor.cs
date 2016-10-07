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
        private Vector2 scrollPos = Vector2.zero;

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


        private void OnGUICache()
        {
            OnGuiFileList("Application.temporaryCachePath ", Application.temporaryCachePath, "キャッシュ");
        }


        private void OnGUISaveData()
        {
            OnGuiFileList("Application.persistentDataPath ", Application.persistentDataPath,"セーブデータ");
        }

        private void OnGuiFileList(string title, string path,string explain)
        {

            EditorGUILayout.LabelField(title);
            EditorGUILayout.LabelField(path);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open"))
            {
                OpenInExplorer(path);
            }
            if (GUILayout.Button("Reset"))
            {
                if (EditorUtility.DisplayDialog("確認", explain + "を削除してよろしいですか？", "OK", "Cancel"))
                {
                    ResetData(path);
                    this.pathList = null;
                }
            }
            EditorGUILayout.EndHorizontal();
            ListFiles(this.pathList);
        }

        private void ListFiles(string[] list)
        {
            EditorGUILayout.LabelField("ファイル一覧");
            if (list == null)
            {
                return;
            }
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (var file in list)
            {
                EditorGUILayout.LabelField(file);
            }
            EditorGUILayout.EndScrollView();
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

        private void ResetData(string path) {
            Directory.Delete(path, true);
        }

        private void OpenInExplorer(string path)
        {
#if UNITY_EDITOR_WIN
            path = path.Replace("/", "\\");
            Process.Start("explorer.exe", "/select," + path);
#elif UNITY_EDITOR_OSX
            EditorUtility.RevealInFinder(path);
#endif
        }
    }
}