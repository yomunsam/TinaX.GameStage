using System.Collections;
using System.Collections.Generic;
using TinaX.Utils;
using UnityEngine;

namespace TinaX.GameStage
{
    public class StageControllerBase
    {
        public string StageName { get; protected internal set; } = string.Empty;
        public IGameStage GameStages { get; protected internal set; }

        protected DisposableGroup DisposableGroup_OnExit = new DisposableGroup();

        /// <summary>
        /// If framework not start, it will be invoked after framework start .
        /// or it will invoked when register.
        /// 如果在框架启动之前注册，它将在框架启动时被调用（服务提供者次序为150）,
        /// 如果框架启动后再注册，那么注册时就被调用
        /// </summary>
        public virtual void OnInit(string RegisterStageName)
        {
            this.StageName = RegisterStageName;
        }

        public virtual void OnEnter(string LastStageName)
        {

        }

        public virtual void OnExit(string NextStageName)
        {
            DisposableGroup_OnExit?.Dispose();
        }

        /// <summary>
        /// On Game Destroy
        /// 游戏整个被退出了
        /// </summary>
        public virtual void OnDestroy()
        {

        }

        public void SwitchStage(string NewStageName)
        {
            this.GameStages?.SwitchStage(NewStageName);
        }

    }
}

