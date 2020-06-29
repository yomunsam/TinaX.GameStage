using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.GameStage
{
    public static class GameStageEventConst
    {
        public const string GameStageEventGroupName = "TinaX.GameStage";

        /// <summary>
        /// Params is string[]
        /// Param 1: old stage name
        /// Param 2: new stage name
        /// </summary>
        public const string OnStageChanged = "OnStageChange";

        /// <summary>
        /// Params is string[]
        /// Param 1: current stage name
        /// Param 2: previous stage name
        /// </summary>
        public const string OnStageEnter = "OnStageEnter";

        /// <summary>
        /// Params is string[]
        /// Param 1: current stage name
        /// Param 2: next stage name
        /// </summary>
        public const string OnStageExit = "OnStageExit";

        /// <summary>
        /// Params is string
        /// Concatenate the stage name after the event name
        /// Parameter 1 is the name of the previous stage
        /// 
        /// 事件名后拼接Stage名
        /// 参数1 为上个stage的名
        /// </summary>
        public const string OnStageEnterWithName = "OnStageEnter_";

        /// <summary>
        /// Params is string
        /// Concatenate the stage name after the event name
        /// Parameter 1 is the name of the next stage
        /// 
        /// 事件名后拼接Stage名
        /// 参数1 为下一个stage的名
        /// </summary>
        public const string OnStageExitWithName = "OnStageExit_";
    }
}
