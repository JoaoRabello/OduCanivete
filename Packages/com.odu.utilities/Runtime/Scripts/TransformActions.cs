using UnityEngine;

namespace Odu.Utilities.GameObjects
{
    public class TransformActions
    {
        /// <summary>
        /// Rotaciona um Transform, fazendo com que ele olhe para uma direção específica
        /// </summary>
        /// <param name="newDirection">Alvo para qual o Transform irá olhar</param>
        /// <param name="transformToRotate">Transform que será rotacionado</param>
        /// <param name="RotationSpeed">Velocidade da rotação</param>
        public static void OduLookAt(Vector3 newDirection, Transform transformToRotate, float RotationSpeed)
        {
            if (Mathf.Abs(newDirection.y - transformToRotate.position.y) < 1)
            {
                newDirection.y = transformToRotate.position.y;
            }
            transformToRotate.rotation = Quaternion.Lerp(transformToRotate.rotation, Quaternion.LookRotation((newDirection - transformToRotate.position).normalized), Time.deltaTime * RotationSpeed);
        }
    }
}
