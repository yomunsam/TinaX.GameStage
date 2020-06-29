using System.Collections.Generic;
using System.Threading.Tasks;
using TinaX.GameStage.Exceptions;
using TinaX.GameStage.Internal;
using System.Linq;
using UnityEngine;
using TinaX.GameStage.Const;
using TinaX.Core.Localization;

namespace TinaX.GameStage
{
    public class GameStageCore : IGameStage , IGameStageInternal
    {
        //key:stageName
        private Dictionary<string, StageControllerBase> m_DictStages = new Dictionary<string, StageControllerBase>();
        private List<StageControllerBase> m_Stages = new List<StageControllerBase>();

        /// <summary>
        /// 初始Stage
        /// </summary>
        private StageControllerBase m_InitialStage;

        private bool m_Inited = false;

        private StageControllerBase m_CurrentStage;

        public string CurrentStageName => (m_CurrentStage != null)? m_CurrentStage.StageName : string.Empty;

        public async Task<XException> Start()
        {
            lock (this)
            {
                if (m_Inited)
                    return null;
            }

            lock (this)
            {
                foreach (var item in m_DictStages)
                {
                    item.Value.GameStages = this;
                    item.Value.OnInit(item.Key);
                }

                if (m_InitialStage != null)
                    this.switchStage(m_InitialStage);

                m_Inited = true;
            }
            await Task.Yield();
            return null;
        }


        public void Close()
        {
            foreach (var item in m_Stages)
                item.OnDestroy();
        }

        public void Register(string stageName, StageControllerBase stageControllerBase)
        {
            this.Register(stageName, stageControllerBase, true, false);
        }

        public void Register(string stageName, StageControllerBase stageController, bool DI = true, bool initialStage = false)
        {
            lock (this)
            {
                //是否已经添加
                if(m_DictStages.TryGetValue(stageName,out var _stage))
                {
                    if (_stage != stageController)
                        throw new DuplicateStageNameException(stageName);
                }
                else
                {
                    m_DictStages.Add(stageName, stageController);
                    m_Stages.Add(stageController);
                }

                //如果当前没有任何Stage，则指定第一个注册的Stage是初始Stage
                if (m_Stages.Count <= 1 && m_InitialStage == null)
                    m_InitialStage = stageController;
                else
                {
                    if (initialStage) //手动指定了初始场景
                        m_InitialStage = stageController;
                }

                //依赖注入
                if(DI)
                {
                    XCore.GetMainInstance().Services.Inject(stageController);
                }

                if (m_Inited)
                {
                    stageController.OnInit(stageName);
                    if (m_InitialStage == stageController)
                        this.switchStage(m_InitialStage);
                }
            }
        }

        public void SwitchStage(string NewStageName)
        {
            if(this.m_DictStages.TryGetValue(NewStageName,out var stage))
            {
                this.switchStage(stage);
            }
            else
            {
                throw new StageNotFoundException(NewStageName);
            }
        }

        public bool TryGetStage(string StageName, out StageControllerBase stage)
        {
            return this.m_DictStages.TryGetValue(StageName, out stage);
        }

        public StageControllerBase GetStage(string StageName) => m_DictStages[StageName];

        public string[] GetStageNames()
            => this.m_Stages.Select(s => s.StageName).ToArray();

        /// <summary>
        /// 实际切换Stage的内部方法
        /// </summary>
        /// <param name="newStage"></param>
        private void switchStage(StageControllerBase newStage)
        {
            if(m_CurrentStage != null)
            {
                if (m_CurrentStage == newStage)
                    return;
                else
                {
                    var last_stage = m_CurrentStage;

                    #region Exit Event 广播退出事件
                    XEvent.Call(GameStageEventConst.OnStageExit, new string[] { last_stage.StageName, newStage.StageName }, GameStageEventConst.GameStageEventGroupName);
                    XEvent.Call($"{GameStageEventConst.OnStageExit}{last_stage.StageName}", newStage.StageName, GameStageEventConst.GameStageEventGroupName);
                    #endregion

                    m_CurrentStage.OnExit(newStage.StageName);
                    m_CurrentStage = newStage;
                    Debug.Log($"[{GameStageConst.ServiceName}]{(XCore.GetMainInstance().IsCmnHans() ? "切换Stage：" : "Switch Stage:")}" +
                        $"<color=#{TinaX.Internal.XEditorColorDefine.Color_Normal_Pure_16}>{last_stage.StageName}</color> --> <color=#{TinaX.Internal.XEditorColorDefine.Color_Emphasize_16}>{newStage.StageName}</color>");
                    
                    newStage.OnEnter(last_stage.StageName);

                    #region Enter Event 广播进入事件
                    XEvent.Call(GameStageEventConst.OnStageEnter, new string[] { newStage.StageName , last_stage.StageName }, GameStageEventConst.GameStageEventGroupName);
                    XEvent.Call($"{GameStageEventConst.OnStageExit}{newStage.StageName}", last_stage.StageName, GameStageEventConst.GameStageEventGroupName);
                    #endregion

                    //广播切换事件
                    XEvent.Call(GameStageEventConst.OnStageChanged, new string[] { last_stage.StageName, newStage.StageName }, GameStageEventConst.GameStageEventGroupName);
                }
            }
            else
            {
                m_CurrentStage = newStage;

                bool isHans = XCore.GetMainInstance().IsCmnHans();
                Debug.Log($"[{GameStageConst.ServiceName}]{(isHans ? "切换Stage：" : "Switch Stage:")}" +
                        $"<color=#{TinaX.Internal.XEditorColorDefine.Color_Safe_16}>*{(isHans?"空 Stage":"No Stage")}*</color> --> <color=#{TinaX.Internal.XEditorColorDefine.Color_Emphasize_16}>{newStage.StageName}</color>");


                newStage.OnEnter(string.Empty);

                //广播进入事件
                XEvent.Call(GameStageEventConst.OnStageEnter, new string[] { newStage.StageName, string.Empty }, GameStageEventConst.GameStageEventGroupName);
                XEvent.Call($"{GameStageEventConst.OnStageExit}{newStage.StageName}", string.Empty, GameStageEventConst.GameStageEventGroupName);

                //广播切换事件
                XEvent.Call(GameStageEventConst.OnStageChanged, new string[] { string.Empty, newStage.StageName }, GameStageEventConst.GameStageEventGroupName);
            }
        }

    }
}
