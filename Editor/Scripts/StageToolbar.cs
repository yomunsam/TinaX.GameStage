using TinaX.GameStage;
using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;
using TinaX;
using TinaXEditor.Utils;
using TinaX.GameStage.Const;

namespace TinaXEditor.GameStage.Internal
{
    [InitializeOnLoad]
    class StageToolbar
    {
        static Texture m_icon_stage;
        static IGameStage stage;
        const string ShowStageInfo_Key = "TinaX.GameStage_ShowStageToolbar";
        static bool ShowStageInfo;
        static bool GameStageNotRegistered = false;

        static StageToolbar()
        {
            ShowStageInfo = EditorPrefs.GetBool(ShowStageInfo_Key, true);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        static void OnToolbarGUI()
        {
            if (!EditorApplication.isPlaying)
                return;
            if (TinaX.XCore.MainInstance == null)
                return;
            if (!XCore.MainInstance.IsRunning) return;
            if (!TinaX.XCore.MainInstance.Services.TryGet<IGameStage>(out stage))
            {
                GameStageNotRegistered = true;
                return;
            }
            if (GameStageNotRegistered) return;

            if (m_icon_stage == null)
                m_icon_stage = AssetDatabase.LoadAssetAtPath<Texture>("Packages/io.nekonya.tinax.gamestage/Editor/Res/stage2.png");

            GUILayout.FlexibleSpace();
            if(GUILayout.Button(new GUIContent(m_icon_stage, "TinaX Game Stage"), EditorStyles.toolbarButton, GUILayout.MaxWidth(25), GUILayout.MaxHeight(20)))
            {
                GenericMenu menu = new GenericMenu();
                if (ShowStageInfo)
                {
                    menu.AddItem(new GUIContent("Hide Stage Info"), false, () =>
                    {
                        EditorPrefs.SetBool(ShowStageInfo_Key, false);
                        ShowStageInfo = false;
                    });
                }
                else
                {
                    menu.AddItem(new GUIContent("Show Stage Info"), false, () =>
                    {
                        EditorPrefs.SetBool(ShowStageInfo_Key, true);
                        ShowStageInfo = true;
                    });
                }
                
                menu.ShowAsContext();
            }


            if (ShowStageInfo)
            {
                if (stage.CurrentStageName.IsNullOrEmpty())
                {
                    GUILayout.Label(new GUIContent(EditorGUIUtil.IsCmnHans?"无 Stage":"No Stage"), EditorStyles.toolbarButton, GUILayout.MaxWidth(75));
                }
                else
                {
                    string cur_stage_name = stage.CurrentStageName;
                    if(GUILayout.Button(new GUIContent(cur_stage_name), EditorStyles.toolbarButton, GUILayout.MaxWidth(75)))
                    {
                        var stage_names = stage.GetStageNames();
                        GenericMenu _menu = new GenericMenu();
                        foreach(var item in stage_names)
                        {
                            string name = item;
                            _menu.AddItem(new GUIContent(name), (name == cur_stage_name),
                                () =>
                                {
                                    if(name != cur_stage_name)
                                    {
                                        Debug.Log($"[{GameStageConst.ServiceName}]{(EditorGUIUtil.IsCmnHans ? "编辑器接入切换Stage: " : "Editor switching stage: ")} " +
                                            $"{cur_stage_name} -> {name}");
                                        stage.SwitchStage(name);
                                    }
                                });
                        }
                    }
                }
            }
            
        }


    }
}

