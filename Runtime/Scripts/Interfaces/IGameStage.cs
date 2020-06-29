using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinaX.GameStage
{
    public interface IGameStage
    {
        string CurrentStageName { get; }

        StageControllerBase GetStage(string StageName);
        string[] GetStageNames();
        void Register(string stageName, StageControllerBase stageController, bool DI = true, bool initialStage = false);
        void Register(string stageName, StageControllerBase stageControllerBase);
        void SwitchStage(string NewStageName);
        bool TryGetStage(string StageName, out StageControllerBase stage);
    }
}
