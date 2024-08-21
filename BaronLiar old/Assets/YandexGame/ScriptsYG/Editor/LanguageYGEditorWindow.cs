using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

namespace YG
{
    public class LanguageYGEditorWindow : EditorWindow
    {
        [MenuItem("YG/Localization/Autolocalization Masse")]
        public static void ShowWindow()
        {
            GetWindow<LanguageYGEditorWindow>("AutoLanguage");
        }

        List<GameObject> objectsTranlate = new List<GameObject>();

        private void OnGUI()
        {
            GUILayout.Space(10);

            if (GUILayout.Button("Поиск всех объектов на сцене по типу Text/TextMesh", GUILayout.Height(30)))
            {
                objectsTranlate.Clear();

                foreach (Text obj in SceneAsset.FindObjectsOfType<Text>())
                {
                    objectsTranlate.Add(obj.gameObject);
                }

                foreach (TextMesh obj in SceneAsset.FindObjectsOfType<TextMesh>())
                {
                    objectsTranlate.Add(obj.gameObject);
                }
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Добавить выделенные"))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    if (obj.GetComponent<Text>() || obj.GetComponent<TextMesh>())
                    {
                        bool check = false;
                        for (int i = 0; i < objectsTranlate.Count; i++)
                            if (obj == objectsTranlate[i])
                                check = true;

                        if (!check)
                            objectsTranlate.Add(obj);
                    }
                }
            }

            if (GUILayout.Button("Убрать выделенные"))
            {
                foreach (GameObject obj in Selection.gameObjects)
                {
                    objectsTranlate.Remove(obj);
                }
            }

            GUILayout.EndHorizontal();

            if (objectsTranlate.Count > 0)
            {
                if (GUILayout.Button("Очистить"))
                {
                    objectsTranlate.Clear();
                }
            }

            if (objectsTranlate.Count > 0)
            {
                GUILayout.Space(10);

                if (GUILayout.Button("Перевести все на все языки", GUILayout.Height(30)))
                {
                    foreach (GameObject obj in objectsTranlate)
                    {
                        LanguageYG scrAL = obj.GetComponent<LanguageYG>();

                        if (scrAL == null)
                            scrAL = obj.AddComponent<LanguageYG>();

                        scrAL.Serialize();
                        scrAL.componentTextField = true;
                        scrAL.Translate(19);
                    }
                }

                if (GUILayout.Button("Удалить автоматический перевод"))
                {
                    foreach (GameObject obj in objectsTranlate)
                    {
                        LanguageYG scrAL = obj.GetComponent<LanguageYG>();

                        if (scrAL)
                            DestroyImmediate(scrAL);
                    }
                }
            }

            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            GUILayout.Label($"({objectsTranlate.Count} объектов в списке)", style, GUILayout.ExpandWidth(true));

            for (int i = 0; i < objectsTranlate.Count; i++)
            {
                objectsTranlate[i] = (GameObject)EditorGUILayout.ObjectField($"{i + 1}. { objectsTranlate[i].name}", objectsTranlate[i], typeof(GameObject), false);
            }
        }

        //IEnumerator GlobalTranslate()
        //{
        //    int completedCount = 0;

        //    while (completedCount < objectsTranlate.Count)
        //    {
        //        LanguageYG scrAL = objectsTranlate[completedCount].GetComponent<LanguageYG>();

        //        if (scrAL == null)
        //            scrAL = objectsTranlate[completedCount].AddComponent<LanguageYG>();

        //        scrAL.componentTextField = true;
        //        scrAL.Translate(19);

        //        completedCount++;
        //        yield return null;
        //    }

        //    Debug.Log("Translate Complit!");
        //}
    }
}

