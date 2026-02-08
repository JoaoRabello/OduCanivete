#if CINEMACHINE_PRESENT
using Unity.Cinemachine;
#endif

#if NAUGHTYATTRIBUTES_PRESENT
using NaughtyAttributes;
#endif

#if SAVESYSTEM_PRESENT
using System;
#endif

using UnityEngine;
using UnityEngine.Events;

namespace OduLib.Canivete.Events
{
    /// <summary>
    /// <para><b>Responsabilidade</b></para>
    /// <para>
    /// Componente genérico responsável por disparar eventos
    /// quando ocorre uma interação física entre este objeto
    /// e outro <see cref="GameObject"/> com uma tag específica.
    /// </para>
    ///
    /// <para><b>Tipos de interação suportados</b></para>
    /// <para>
    /// • Trigger ou Collision  
    /// • Enter, Exit ou Stay  
    /// • Eventos de cena (<see cref="UnityEvent"/>)  
    /// • Eventos baseados em <see cref="ScriptableObject"/>  
    /// </para>
    ///
    /// <para><b>Casos de uso comuns</b></para>
    /// <para>
    /// • Gatilhos de câmera  
    /// • Checkpoints  
    /// • Portas automáticas  
    /// • Cutscenes  
    /// • Eventos persistentes via Save System  
    /// </para>
    /// </summary>
    public class CollisionEventTrigger : MonoBehaviour
#if SAVESYSTEM_PRESENT
    , IDataPersistence
#endif
    {
        /// <summary>
        /// <para><b>Tipo de parâmetro</b></para>
        /// <para>
        /// Define qual tipo de dado extra será passado
        /// ao <see cref="ScriptableEvent"/>, quando aplicável.
        /// </para>
        /// </summary>
        public enum ParameterType
        {
            None,
            CinemachineVirtualCamera
        }

        /// <summary>
        /// <para><b>Tipo de colisão</b></para>
        /// <para>
        /// Determina se a interação será baseada em
        /// <see cref="Collider.isTrigger"/> ou colisão física.
        /// </para>
        /// </summary>
        public enum TriggerType
        {
            Trigger,
            Collision
        }

        /// <summary>
        /// <para><b>Momento da interação</b></para>
        /// <para>
        /// Define quando o evento será disparado
        /// durante a interação física.
        /// </para>
        /// </summary>
        public enum InteractionType
        {
            Enter,
            Exit,
            Stay
        }

        /// <summary>
        /// <para><b>Tipo de evento</b></para>
        /// <para>
        /// Define se o evento será tratado via
        /// <see cref="ScriptableEvent"/> ou <see cref="UnityEvent"/>.
        /// </para>
        /// </summary>
        public enum EventType
        {
            Scriptable,
            Scene
        }

        // ===========================
        // BASICS
        // ===========================

        [Header("Basics")]
        [Tooltip("Determina qual é o tipo de colisor que deve ser checado")]
        [SerializeField] private TriggerType _type;

        [Tooltip("Determina qual é a forma com a qual o jogador interagirá com o colisor")]
        [SerializeField] private InteractionType _interactionType;

        [Tooltip("Determina qual é o tipo de evento a ser invocado")]
        [SerializeField] private EventType _eventType;

        [Tooltip("Quando verdadeiro, o evento tocará somente na primeira vez que interagir com o player.")]
        [SerializeField] private bool _selfDestroy;

        [Tooltip("Determina qual é a tag que será analisada pela colisão")]
        [SerializeField] private string _affectedTag = "Player";

        // ===========================
        // SCRIPTABLE EVENT
        // ===========================

        [Header("Scriptable Event")]
#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_eventType", EventType.Scriptable)]
#endif
        [SerializeField] private ScriptableEvent _event;

#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_eventType", EventType.Scriptable)]
#endif
        [SerializeField] private bool _hasParameter;

#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_hasParameter")]
#endif
        [SerializeField] private ParameterType _parameterType;

#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_hasParameter")]
#endif
        [SerializeField] private GameObject _parameter;

        // ===========================
        // UNITY EVENT
        // ===========================

        [Header("Unity Event")]
#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_eventType", EventType.Scene)]
#endif
        [SerializeField] private UnityEvent _unityEvent;

#if SAVESYSTEM_PRESENT
        // ===========================
        // SAVE SYSTEM
        // ===========================

        [Header("Save System")]
        [SerializeField] private bool _saveWhenDestroyed;
#endif

#if NAUGHTYATTRIBUTES_PRESENT
    [ShowIf("_saveWhenDestroyed")]
#endif
        [SerializeField] private string _eventID;

        /// <summary>
        /// <para><b>Trigger Enter</b></para>
        /// <para>
        /// Dispara o evento quando um <see cref="Collider"/>
        /// entra no trigger, respeitando as regras configuradas.
        /// </para>
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (_type != TriggerType.Trigger ||
                _interactionType != InteractionType.Enter ||
                !other.CompareTag(_affectedTag)) return;

            InvokeEvent();
        }

        /// <summary>
        /// <para><b>Trigger Stay</b></para>
        /// <para>
        /// Dispara o evento continuamente enquanto o objeto
        /// permanece dentro do trigger.
        /// </para>
        /// </summary>
        private void OnTriggerStay(Collider other)
        {
            if (_type != TriggerType.Trigger ||
                _interactionType != InteractionType.Stay ||
                !other.CompareTag(_affectedTag)) return;

            InvokeEvent();
        }

        /// <summary>
        /// <para><b>Collision Enter</b></para>
        /// <para>
        /// Dispara o evento quando ocorre uma colisão física
        /// entre os objetos.
        /// </para>
        /// </summary>
        private void OnCollisionEnter(Collision collision)
        {
            if (_type != TriggerType.Collision ||
                _interactionType != InteractionType.Enter ||
                !collision.gameObject.CompareTag(_affectedTag)) return;

            InvokeEvent();
        }

        /// <summary>
        /// <para><b>Execução do evento</b></para>
        /// <para>
        /// Resolve qual evento deve ser disparado,
        /// com ou sem parâmetro, e executa a lógica
        /// de autodestruição e persistência.
        /// </para>
        /// </summary>
        private void InvokeEvent()
        {
            switch (_eventType)
            {
                case EventType.Scriptable:
                    if (!_hasParameter)
                    {
                        _event.Invoke();
                    }
                    else
                    {
                        switch (_parameterType)
                        {
                            case ParameterType.CinemachineVirtualCamera:
                                _event.Invoke(_parameter.GetComponent<CinemachineCamera>());
                                break;
                        }
                    }
                    break;

                case EventType.Scene:
                    _unityEvent.Invoke();
                    break;
            }

            if (!_selfDestroy) return;

#if SAVESYSTEM_PRESENT
            if (SaveDataManager.Instance.HasSaveData &&
                _saveWhenDestroyed &&
                !SaveDataManager.Instance.SaveData.destroyedEvents.Contains(_eventID))
            {
                SaveDataManager.Instance.SaveData.destroyedEvents.Add(_eventID);
            }
#endif

            Destroy(gameObject);
        }

#if SAVESYSTEM_PRESENT
        /// <summary>
        /// <para><b>Load de persistência</b></para>
        /// <para>
        /// Remove o objeto caso o evento já tenha sido
        /// executado em um save anterior.
        /// </para>
        /// </summary>
        public void LoadData(SaveData data)
        {
            if (!_saveWhenDestroyed) return;

            if (data.destroyedEvents.Contains(_eventID))
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// <para><b>Save de persistência</b></para>
        /// <para>
        /// Método obrigatório pela interface, porém
        /// sem necessidade de lógica adicional.
        /// </para>
        /// </summary>
        public void SaveData(SaveData data) { }

        /// <summary>
        /// <para><b>Utilitário de editor</b></para>
        /// <para>
        /// Gera um GUID único para identificar
        /// este evento no sistema de save.
        /// </para>
        /// </summary>
        [ContextMenu("Generate guid for eventID")]
        private void GenerateGuid()
        {
            _eventID = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
