
using System.Collections.Generic;
using System;

namespace Mygame.NetInput
{
    public static class NetInput
    {
        private static Dictionary<string, InputMap> inputPerPlayer = new Dictionary<string, InputMap> { {"1", new InputMap() } };
        public static void HandleInput(string input, string id)
        {
            inputPerPlayer.TryGetValue(id, out InputMap map);
            if (map.InputActions.TryGetValue(input, out var action))
            {
                action.Invoke();
            }
            else
            {
                Console.WriteLine($"Unknown input: {input}");
            }
        }
        public static void AddInput(string id)
        {
            inputPerPlayer.Add(id, new InputMap());
        }
        public static InputMap GetInputMap(string id)
        {
            return inputPerPlayer[id];
        }
    }
}