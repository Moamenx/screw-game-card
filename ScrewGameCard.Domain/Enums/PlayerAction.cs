using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrewGameCard.Domain.Enums
{
    public enum PlayerAction
    {
        UseSwap = 1,
        UseBlindSwap,
        UsePeekOwn,
        UsePeekOther,
        DiscardCard,
        EndTurn,
        Screw
    }
}
