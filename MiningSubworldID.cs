using ReLogic.Reflection;

namespace MiningDimension
{
    // Name for these can be finnicky, probably also want to add some sort of search function like the tmod ID classes
    // I choose byte because i don't think there will ever be more than 255 subworlds (but then again vanilla thought that about wings lol)
    public class MiningSubworldID
    {
        public const byte none = 0;
        public const byte PreBoss = 1;
        public const byte EvilOres = 2;
        public const byte Hell = 3;
        public const byte Hardmode = 4;
        public const byte Jungle = 5;
        public const byte Luminite = 6;

        public static readonly byte Count = 7;

        public static readonly IdDictionary Search = IdDictionary.Create<MiningSubworldID, byte>();
    }
}
