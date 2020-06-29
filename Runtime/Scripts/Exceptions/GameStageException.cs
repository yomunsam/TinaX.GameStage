using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.GameStage.Const;

namespace TinaX.GameStage.Exceptions
{
    public class GameStageException : XException
    {
        public GameStageException(string msg) : base($"[{GameStageConst.ServiceName}]{msg}") { }

        public GameStageException(int errorCode) : base(errorCode)
        {
        }

        public GameStageException(string msg, int errorCode) : base($"[{GameStageConst.ServiceName}]{msg}", errorCode)
        {
        }
    }
}
