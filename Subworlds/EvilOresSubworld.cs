using MiningDimension.WorldGenPasses;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace MiningDimension.Subworlds
{
    public class EvilOresSubworld : Subworld
    {
        public override int Height => 1200;
        public override int Width => 4200;
        public override bool ShouldSave => false;

        public override List<GenPass> Tasks => new()
        {
            new BaseDirtPass(),
            new StonePatchesPass(),
            new CavingPass(),
            new OreGenerationPass(MiningSubworldID.EvilOres)
        };
    }
}
