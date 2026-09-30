using TMPro;
using UnityEngine;

public class EventComponent : MonoBehaviour {
    public Event EventData { get; set; }
    public TextMeshProUGUI TitleTMP { get; set; }
    public TextMeshProUGUI TextTMP { get; set; }

    public void Start() {
        if (EventData == null) {
            return;
        }

        TitleTMP = GameObject.Find("TitleTMP").GetComponent<TextMeshProUGUI>();
        TextTMP = GameObject.Find("TextTMP").GetComponent<TextMeshProUGUI>();
        TitleTMP.SetText(EventData.Title);
        TextTMP.SetText(EventData.Text);
        return;
    }
}