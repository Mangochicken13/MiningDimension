using MiningDimension.WorldGenPasses;
using SubworldLibrary;
using System.Collections.Generic;
using Terraria.WorldBuilding;

namespace MiningDimension.Subworlds
{
    // NOTE TO SELF: Make half decent world gen as a base,
    // copy it into multiple subworlds, one for each stage
    // MAYBE later try to make a modded ore one, or options for other mods to add their own.
    public class PreBossSubworld : Subworld
    {
        public override int Width => 4200;
        public override int Height => 1200;
        public override bool ShouldSave => false;

        public override List<GenPass> Tasks => new()
        {
            new BaseDirtPass(),
            new StonePatchesPass(),
            new CavingPass(),

            new OreGenerationPass(MiningSubworldID.PreBoss)
        };
    }
}
