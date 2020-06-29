using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.GameStage.Const;

namespace TinaX.GameStage.Exceptions
{
    public class StageNotFoundException : GameStageException
    {
        public string StageName { get; private set; }
        public StageNotFoundException(string stageName) 
            : base($"Cannot found stagename: {stageName}", (int)GameStageErrorCode.StageNotFound)
        {
            this.StageName = stageName;
        }
    }
}
