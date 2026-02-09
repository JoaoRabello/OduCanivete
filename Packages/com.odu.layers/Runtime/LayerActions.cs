using UnityEngine;

namespace OduLib.Canivete.Layers{
    /// <summary>
    /// Classe utilitária para operações com máscaras de camadas (LayerMask) do Unity.
    /// Fornece métodos para verificar, adicionar e remover camadas de uma máscara de camadas.
    /// </summary>
    public static class LayerActions
    {
        /// <summary>
        /// Verifica se uma camada específica está presente na máscara de camadas fornecida.
        /// </summary>
        /// <param name="originalMask">A máscara de camadas a ser verificada.</param>
        /// <param name="layerToCheck">O nome da camada a ser procurada.</param>
        /// <returns>Retorna <c>true</c> se a camada está presente, caso contrário <c>false</c>.</returns>
        public static bool ContainsLayer(LayerMask originalMask, string layerToCheck)
        {
            return ContainsLayer(originalMask, LayerMask.NameToLayer(layerToCheck));
        }

        /// <summary>
        /// Verifica se uma camada específica está presente na máscara de camadas fornecida.
        /// </summary>
        /// <param name="mask">A máscara de camadas a ser verificada.</param>
        /// <param name="layerToCheck">O índice da camada a ser procurada.</param>
        /// <returns>Retorna <c>true</c> se a camada está presente, caso contrário <c>false</c>.</returns>
        public static bool ContainsLayer(LayerMask mask, LayerMask layerToCheck)
        {
            return (mask & (1 << layerToCheck)) != 0;
        }

        /// <summary>
        /// Adiciona uma camada à máscara de camadas original.
        /// </summary>
        /// <param name="originalMask">A máscara de camadas original.</param>
        /// <param name="layerToAdd">O nome da camada a ser adicionada.</param>
        /// <returns>Retorna a nova máscara de camadas com a camada adicionada.</returns>
        public static LayerMask AddLayer(LayerMask originalMask, string layerToAdd)
        {
            return AddLayer(originalMask, LayerMask.NameToLayer(layerToAdd));
        }

        /// <summary>
        /// Adiciona uma camada à máscara de camadas original.
        /// </summary>
        /// <param name="originalMask">A máscara de camadas original.</param>
        /// <param name="layerToAdd">O índice da camada a ser adicionada.</param>
        /// <returns>Retorna a nova máscara de camadas com a camada adicionada.</returns>
        public static LayerMask AddLayer(LayerMask originalMask, LayerMask layerToAdd)
        {
            return originalMask |= 1 << layerToAdd;
        }

        /// <summary>
        /// Remove uma camada da máscara de camadas original.
        /// </summary>
        /// <param name="originalMask">A máscara de camadas original.</param>
        /// <param name="layerToRemove">O nome da camada a ser removida.</param>
        /// <returns>Retorna a nova máscara de camadas com a camada removida.</returns>
        public static LayerMask RemoveLayer(LayerMask originalMask, string layerToRemove)
        {
            return RemoveLayer(originalMask, LayerMask.NameToLayer(layerToRemove));
        }
        
        /// <summary>
        /// Remove uma camada da máscara de camadas original.
        /// </summary>
        /// <param name="originalMask">A máscara de camadas original.</param>
        /// <param name="layerToRemove">O índice da camada a ser removida.</param>
        /// <returns>Retorna a nova máscara de camadas com a camada removida.</returns>
        public static LayerMask RemoveLayer(LayerMask originalMask, LayerMask layerToRemove)
        {
            return originalMask &= ~(1 << layerToRemove);
        }
    }
}
