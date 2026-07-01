
using System.Collections.Generic;
using System;

namespace Mygame.NetInput
{
    public static class NetInput
    {
        private static Dictionary<string, InputMap> inputPerPlayer = new Dictionary<string, InputMap> { {"1", new InputMap() } };
        public static void HandleInput(string input, string id)
        {
            if(inputPerPlayer.TryGetValue(id, out InputMap map))
            {
                map.SetInput(input);
            }
            else
            {
                Console.WriteLine($"No input map found for player ID: {id}");
            }
        }
        public static void AddInput(string id)
        {
            inputPerPlayer.Add(id, new InputMap());
        }
        public static InputMap GetInputMap(string id)
        {
            if(inputPerPlayer.TryGetValue(id, out InputMap map))
            {
                return map;
            }
            else
            {
                Console.WriteLine($"No input map found for player ID: {id}");
                return null;
            }
        }
        public static void UpdateInputMaps()
        {
            foreach (var inputMap in inputPerPlayer.Values)
            {
                inputMap.UpdateInputMap();
            }
        }
    }
}