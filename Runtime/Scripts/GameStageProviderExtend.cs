using TinaX.GameStage;

namespace TinaX.Services
{
    public static class GameStageProviderExtend
    {
        public static IXCore UseGameStage(this IXCore core)
        {
            return core.RegisterServiceProvider(new GameStageProvider());
        }
    }
}
