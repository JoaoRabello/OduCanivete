using System;
using System.Collections.Generic;
using UnityEngine;

namespace OduLib.Systems.MapDesign
{
    [Serializable]
    public class MapPin
    {
        public string Name;
        public Sprite Image;
        public string Description;
        public Vector3 Position;
        public Map RelatedMap;

        public MapPin(string name, Sprite image, string description, Vector3 position, Map relatedMap)
        {
            Name = name;
            Image = image;
            Description = description;
            Position = position;
            RelatedMap = relatedMap;
        }
    }

    [Serializable]
    public class Map
    {
        public string Name;
    }

    [Serializable]
    public class MapPinGroup
    {
        public Map RelatedMap;
        public List<MapPin> Pins = new List<MapPin>();
    }

    [CreateAssetMenu(fileName = "MapPinDatabase", menuName = "OduLib/Systems/MapDesign/MapPinDatabase")]
    public class MapPinDatabase : ScriptableObject
    {
        [SerializeField] private List<MapPinGroup> _mapPinGroups = new List<MapPinGroup>();

        public void CreatePin(Map map, string name, string description, Sprite image, Vector3 position)
        {
            foreach (var group in _mapPinGroups)
            {
                if (group.RelatedMap.Name.Equals(map.Name))
                {
                    var newPin = new MapPin(name, image, description, position, map);
                    group.Pins.Add(newPin);
                    return;
                }
            }
        }

        public List<MapPin> GetPinsFromMap(Map relatedMap)
        {
            var result = new List<MapPin>();
            foreach (var group in _mapPinGroups)
            {
                if(!group.RelatedMap.Name.Equals(relatedMap.Name))
                {
                    continue;
                }
                result.AddRange(group.Pins);
            }
            return result;
        }
    }
}