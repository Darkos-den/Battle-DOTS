using Darkos;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class LevelAuthoring : MonoBehaviour
{

    class Baker : Baker<LevelAuthoring> {
        public override void Bake(LevelAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            var config = new LevelComponent();

            config.unitsP1.Add(new UnitConfig { 
                Type = UnitType.Monster,
                startPositionX = 1,
                startPositionY = 2,
            });
            config.unitsP1.Add(new UnitConfig {
                Type = UnitType.Monster,
                startPositionX = 1,
                startPositionY = 4,
            });
            config.unitsP1.Add(new UnitConfig {
                Type = UnitType.Monster,
                startPositionX = 1,
                startPositionY = 5,
            });
            config.unitsP1.Add(new UnitConfig {
                Type = UnitType.Monster,
                startPositionX = 1,
                startPositionY = 7,
            });

            config.unitsP2.Add(new UnitConfig {
                Type = UnitType.Paladin,
                startPositionX = 8,
                startPositionY = 2,
            });
            config.unitsP2.Add(new UnitConfig {
                Type = UnitType.Paladin,
                startPositionX = 8,
                startPositionY = 4,
            });
            config.unitsP2.Add(new UnitConfig {
                Type = UnitType.Paladin,
                startPositionX = 8,
                startPositionY = 5,
            });
            config.unitsP2.Add(new UnitConfig {
                Type = UnitType.Paladin,
                startPositionX = 8,
                startPositionY = 7,
            });

            AddComponentObject(entity, config);
        }
    }
}
