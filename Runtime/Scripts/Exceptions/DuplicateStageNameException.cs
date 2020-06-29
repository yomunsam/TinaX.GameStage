using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinaX.GameStage.Const;

namespace TinaX.GameStage.Exceptions
{
    public class DuplicateStageNameException : GameStageException
    {
        public string StageName { get; private set; }
        public DuplicateStageNameException(string stageName) 
            : base($"Cannot register the same stagename: {stageName}", (int)GameStageErrorCode.DuplicateStageName)
        {
            this.StageName = stageName;
        }
    }
}
