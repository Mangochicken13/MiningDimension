using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Utilities;

namespace MiningDimension
{
    public class SubworldRandomGen : UnifiedRandom
    {
        // Note to self: Work on making a randomizer that will actually randomise the subworld generation
        // Update: using System.Random.Shared.Next() instead appears to work, will cross check to see if this has major performance impacts
    }
}
