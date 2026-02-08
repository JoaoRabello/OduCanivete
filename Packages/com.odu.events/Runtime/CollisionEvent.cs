using System;
using UnityEngine;

namespace OduLib.Canivete.Events
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Componente utilitário responsável por expor callbacks
    /// de eventos de <see cref="Collider"/> do Unity
    /// através de <see cref="Action"/>.
    /// </para>
    ///
    /// <para><b>Objetivo principal</b></para>
    /// <para>
    /// Facilitar o desacoplamento entre a lógica de colisão
    /// e os sistemas que precisam reagir a ela,
    /// evitando herança direta ou overrides de métodos
    /// como <see cref="MonoBehaviour.OnTriggerEnter"/>.
    /// </para>
    ///
    /// <para><b>Casos de uso comuns</b></para>
    /// <list type="bullet">
    /// <item>
    /// <description>Sistemas de interação genéricos.</description>
    /// </item>
    /// <item>
    /// <description>Triggers reutilizáveis em prefabs.</description>
    /// </item>
    /// <item>
    /// <description>Arquiteturas orientadas a eventos.</description>
    /// </item>
    /// </list>
    /// </summary>
    public class CollisionEvent : MonoBehaviour
    {
        /// <summary>
        /// <para><b>Evento de entrada no trigger</b></para>
        /// <para>
        /// Invocado quando um <see cref="Collider"/> entra
        /// no trigger associado a este <see cref="GameObject"/>.
        /// </para>
        ///
        /// <para><b>Observação</b></para>
        /// <para>
        /// O <see cref="Collider"/> recebido é o objeto que
        /// iniciou a interação.
        /// </para>
        /// </summary>
        public Action<Collider> OnTriggerEntered;

        /// <summary>
        /// <para><b>Evento de permanência no trigger</b></para>
        /// <para>
        /// Invocado continuamente enquanto um
        /// <see cref="Collider"/> permanece dentro
        /// do trigger.
        /// </para>
        /// </summary>
        public Action<Collider> OnTriggerStayed;

        /// <summary>
        /// <para><b>Callback de Trigger Enter</b></para>
        /// <para>
        /// Método interno do Unity responsável por
        /// repassar o evento para os listeners registrados.
        /// </para>
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEntered?.Invoke(other);
        }

        /// <summary>
        /// <para><b>Callback de Trigger Stay</b></para>
        /// <para>
        /// Método interno do Unity responsável por
        /// repassar o evento continuamente para os
        /// listeners registrados.
        /// </para>
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            OnTriggerStayed?.Invoke(other);
        }
    }
}