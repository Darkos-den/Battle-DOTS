using System.Collections.Generic;
using Unity.Entities;

namespace Darkos {

    public enum UnitType {
        Monster, Paladin
    }

    public struct UnitConfig {
        public UnitType Type;
        public int startPositionX;
        public int startPositionY;
    }


    public class LevelComponent : IComponentData {
        public List<UnitConfig> unitsP1 = new();
        public List<UnitConfig> unitsP2 = new();
    }

}