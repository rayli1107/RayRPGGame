
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NpcUnitUIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _iconQuestGiver;
    [SerializeField]
    private TextMeshProUGUI _iconQuestUpdate;

    private string _npcId;

    private void Awake()
    {
        _npcId = GetComponent<NPCController>().profile.id;
    }

    private void OnEnable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.updateAction += onQuestUpdate;
            onQuestUpdate();
        }
    }

    private void OnDisable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.updateAction -= onQuestUpdate;
        }
    }

    private void onQuestUpdate()
    {
        _iconQuestGiver.enabled = QuestManager.Instance.startQuestNpcs.ContainsKey(_npcId);
        _iconQuestUpdate.enabled = false;
    }

}
