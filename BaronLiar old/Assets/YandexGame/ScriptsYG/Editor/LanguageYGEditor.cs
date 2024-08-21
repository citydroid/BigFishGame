using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

namespace YG
{
    [CustomEditor(typeof(LanguageYG))]
    public class LanguageYGEditor : Editor
    {
        LanguageYG scr;

        GUIStyle red;
        GUIStyle green;

        int processTranslateLabel;

        private void OnEnable()
        {
            scr = (LanguageYG)target;
            scr.Serialize();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            scr = (LanguageYG)target;
            Undo.RecordObject(scr, "Undo LanguageYG");

            red = new GUIStyle(EditorStyles.label);
            red.normal.textColor = Color.red;
            green = new GUIStyle(EditorStyles.label);
            green.normal.textColor = Color.green;

            //if (scr.textUIComponent) EditorGUILayout.PropertyField(serializedObject.FindProperty("textUIComponent"));
            //else if (scr.textMeshComponent) EditorGUILayout.PropertyField(serializedObject.FindProperty("textMeshComponent"));
            //else
            if (scr.textUIComponent == null && scr.textMeshComponent == null)
            {
                if (GUILayout.Button("Определить Text/TextMesh"))
                {
                    scr.textUIComponent = scr.GetComponent<Text>();
                    scr.textMeshComponent = scr.GetComponent<TextMesh>();
                }
                if (GUILayout.Button("Создать Text компонент"))
                    scr.textUIComponent = scr.gameObject.AddComponent<Text>();
                if (GUILayout.Button("Создать TextMesh компонент"))
                    scr.textMeshComponent = scr.gameObject.AddComponent<TextMesh>();

                GUILayout.Space(10);
            }

            //if (scr.infoYG) EditorGUILayout.PropertyField(serializedObject.FindProperty("infoYG"));
            //else
            if (scr.infoYG == null)
            {
                if (GUILayout.Button("Определить infoYG", GUILayout.Height(35)))
                {
                    scr.infoYG = GameObject.Find("YandexGame").GetComponent<YandexGame>().infoYG;
                    if (scr.infoYG == null)
                        Debug.LogError("InfoYG not found!  (ru) InfoYG не найден!");
                }
            }

            if (scr.infoYG)
            {
                GUILayout.Space(10);

                if (!scr.infoYG.autolocationInspector)
                    scr.textHeight = EditorGUILayout.Slider("Text Height", scr.textHeight, 20f, 400f);

                if (scr.infoYG.autolocationInspector)
                {
                    GUILayout.BeginVertical("HelpBox");

                    scr.componentTextField = EditorGUILayout.ToggleLeft("Component Text/TextMesh Translate", scr.componentTextField);
                    scr.textHeight = EditorGUILayout.Slider("Text Height", scr.textHeight, 20f, 400f);

                    if (!scr.componentTextField)
                        scr.text = EditorGUILayout.TextArea(scr.text, GUILayout.Height(scr.textHeight));
                    else
                    {
                        if (scr.textUIComponent) GUILayout.Label("*" + scr.textUIComponent.text);
                        else if (scr.textMeshComponent) GUILayout.Label("*" + scr.textMeshComponent.text);
                    }

                    GUILayout.BeginHorizontal();

                    if (scr.componentTextField)
                    {
                        if (scr.textUIComponent)
                        {
                            if (scr.textUIComponent.text.Length > 0)
                            {
                                GUILayout.Label("Text Component", green);

                                if (GUILayout.Button("ПЕРЕВЕСТИ"))
                                    TranslateButton();
                            }
                            else
                                GUILayout.Label("Text Component", red);
                        }
                        else if (scr.textMeshComponent)
                        {
                            if (scr.textMeshComponent.text.Length > 0)
                            {
                                GUILayout.Label("TextMesh Component", green);

                                if (GUILayout.Button("ПЕРЕВЕСТИ"))
                                    TranslateButton();
                            }
                            else
                                GUILayout.Label("TextMesh Component", red);
                        }
                    }
                    else
                    {
                        if (scr.componentTextField || (scr.text == null || scr.text.Length == 0))
                        {
                            GUILayout.Label("ЗАПОЛНИТЕ ПОЛЕ", red);
                        }
                        else if (GUILayout.Button("ПЕРЕВЕСТИ"))
                            TranslateButton();
                    }

                    if (GUILayout.Button("ОЧИСТИТЬ"))
                    {
                        scr.ru = "";
                        scr.en = "";
                        scr.tr = "";
                        scr.az = "";
                        scr.be = "";
                        scr.he = "";
                        scr.hy = "";
                        scr.ka = "";
                        scr.et = "";
                        scr.fr = "";
                        scr.kk = "";
                        scr.ky = "";
                        scr.lt = "";
                        scr.lv = "";
                        scr.ro = "";
                        scr.tg = "";
                        scr.tk = "";
                        scr.uk = "";
                        scr.uz = "";

                        scr.processTranslateLabel = "";
                        scr.countLang = processTranslateLabel;
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("box");
                    GUILayout.BeginHorizontal();

                    bool labelProcess = false;
                    if (scr.processTranslateLabel != "")
                    {
                        if (scr.countLang == processTranslateLabel)
                        {
                            GUILayout.Label(scr.processTranslateLabel, green, GUILayout.Height(20));
                            labelProcess = true;
                        }
                        else if (scr.processTranslateLabel == "")
                        {
                            labelProcess = true;
                        }
                        else
                        {
                            GUILayout.Label(scr.processTranslateLabel, GUILayout.Height(20));
                            labelProcess = true;
                        }

                        try
                        {
                            if (scr.processTranslateLabel.Contains("error"))
                            {
                                GUILayout.Label(scr.processTranslateLabel, red, GUILayout.Height(20));
                                labelProcess = true;
                            }
                        }
                        catch
                        {
                        }
                    }

                    if (labelProcess == false)
                        GUILayout.Label(processTranslateLabel + " Languages", GUILayout.Height(20));

                    try
                    {
                        if (!scr.processTranslateLabel.Contains("completed"))
                            GUILayout.Label("Перезайдите в инспектор!", GUILayout.Height(20));
                    }
                    catch
                    {
                    }

                    GUILayout.EndHorizontal();

                    UpdateLanguages();
                    GUILayout.EndVertical();
                }
            }
            
            if ((scr.textUIComponent || scr.textMeshComponent) && scr.infoYG.fonts.defaultFont != null)
            {
                GUILayout.Space(10);
                GUILayout.BeginVertical("box");

                if (GUILayout.Button("Заменить шрифт на стандартный"))
                {
                    if (scr.infoYG.fonts.defaultFont == null)
                        Debug.LogError("The standard font is not specified! Specify it in the InfoYG plugin settings.  (ru) Не указан стандартный шрифт! Укажите его в настройках плагина InfoYG");
                    else
                    {
                        if (scr.textUIComponent)
                            scr.textUIComponent.font = scr.infoYG.fonts.defaultFont;
                        else if (scr.textMeshComponent)
                            scr.textMeshComponent.font = scr.infoYG.fonts.defaultFont;
                    }
                }

                scr.uniqueFont = (Font)EditorGUILayout.ObjectField("Default Font ThisObj", scr.uniqueFont, typeof(Font), false);

                GUILayout.EndVertical();
            }

            if (GUI.changed) SetObjectDirty(scr.gameObject);
        }

        void TranslateButton()
        {
            scr.processTranslateLabel = "";
            scr.Translate(processTranslateLabel);

            if (!scr.componentTextField)
            {
                if (scr.textUIComponent)
                    scr.textUIComponent.text = scr.text;

                if (scr.textMeshComponent)
                    scr.textMeshComponent.text = scr.text;
            }
        }

        void UpdateLanguages()
        {
            processTranslateLabel = 0;

            if (scr.infoYG.languages.ru)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("ru", GUILayout.Width(20), GUILayout.Height(20));
                scr.ru = EditorGUILayout.TextArea(scr.ru, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.en)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("en", GUILayout.Width(20), GUILayout.Height(20));
                scr.en = EditorGUILayout.TextArea(scr.en, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.tr)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("tr", GUILayout.Width(20), GUILayout.Height(20));
                scr.tr = EditorGUILayout.TextArea(scr.tr, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.az)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("az", GUILayout.Width(20), GUILayout.Height(20));
                scr.az = EditorGUILayout.TextArea(scr.az, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.be)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("be", GUILayout.Width(20), GUILayout.Height(20));
                scr.be = EditorGUILayout.TextArea(scr.be, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.he)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("he", GUILayout.Width(20), GUILayout.Height(20));
                scr.he = EditorGUILayout.TextArea(scr.he, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.hy)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("hy", GUILayout.Width(20), GUILayout.Height(20));
                scr.hy = EditorGUILayout.TextArea(scr.hy, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.ka)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("ka", GUILayout.Width(20), GUILayout.Height(20));
                scr.ka = EditorGUILayout.TextArea(scr.ka, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.et)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("et", GUILayout.Width(20), GUILayout.Height(20));
                scr.et = EditorGUILayout.TextArea(scr.et, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.fr)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("fr", GUILayout.Width(20), GUILayout.Height(20));
                scr.fr = EditorGUILayout.TextArea(scr.fr, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.kk)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("kk", GUILayout.Width(20), GUILayout.Height(20));
                scr.kk = EditorGUILayout.TextArea(scr.kk, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.ky)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("ky", GUILayout.Width(20), GUILayout.Height(20));
                scr.ky = EditorGUILayout.TextArea(scr.ky, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.lt)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("lt", GUILayout.Width(20), GUILayout.Height(20));
                scr.lt = EditorGUILayout.TextArea(scr.lt, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.lv)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("lv", GUILayout.Width(20), GUILayout.Height(20));
                scr.lv = EditorGUILayout.TextArea(scr.lv, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.ro)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("ro", GUILayout.Width(20), GUILayout.Height(20));
                scr.ro = EditorGUILayout.TextArea(scr.ro, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.tg)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("tg", GUILayout.Width(20), GUILayout.Height(20));
                scr.tg = EditorGUILayout.TextArea(scr.tg, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.tk)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("tk", GUILayout.Width(20), GUILayout.Height(20));
                scr.tk = EditorGUILayout.TextArea(scr.tk, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.uk)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("uk", GUILayout.Width(20), GUILayout.Height(20));
                scr.uk = EditorGUILayout.TextArea(scr.uk, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }

            if (scr.infoYG.languages.uz)
            {
                processTranslateLabel++;
                GUILayout.BeginHorizontal();
                GUILayout.Label("uz", GUILayout.Width(20), GUILayout.Height(20));
                scr.uz = EditorGUILayout.TextArea(scr.uz, GUILayout.Height(scr.textHeight));
                GUILayout.EndHorizontal();
            }
        }

        public static void SetObjectDirty(GameObject obj)
        {
            EditorUtility.SetDirty(obj);
            EditorSceneManager.MarkSceneDirty(obj.scene);
        }
    }
}
