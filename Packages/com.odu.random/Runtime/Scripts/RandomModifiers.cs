using UnityEngine;

namespace Odu.Randomizers
{
    public class RandomModifiers
    {
        /// <summary>
        /// Retorna -1 ou 1, com 50% de chance de cada
        /// </summary>
        /// <returns>-1 ou 1</returns>
        public static int GetRandomOperatorModifier()
        {
            if (Random.value < 0.5f)
                return 1;
            else
                return -1;
        }
    }
}
