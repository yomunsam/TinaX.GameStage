using System.Threading.Tasks;
using TinaX.GameStage.Const;
using TinaX.GameStage.Internal;
using TinaX.Services;

namespace TinaX.GameStage
{
    [XServiceProviderOrder(150)]
    public class GameStageProvider : IXServiceProvider
    {
        public string ServiceName => GameStageConst.ServiceName;

        public Task<XException> OnInit(IXCore core)
        {
            return Task.FromResult<XException>(null);
        }

        public void OnServiceRegister(IXCore core)
        {
            core.Services.Singleton<IGameStage, GameStageCore>()
                .SetAlias<IGameStageInternal>();
        }

        public Task<XException> OnStart(IXCore core)
            => core.Services.Get<IGameStageInternal>().Start();

        public void OnQuit() 
        {
            XCore.GetMainInstance().Services.Get<IGameStageInternal>().Close();
        }

        public Task OnRestart() => Task.CompletedTask;
    }
}

