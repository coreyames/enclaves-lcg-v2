using UnityEngine;
using Newtonsoft.Json;

public interface ICardInfo {
    public static bool LoadFromJSON(string json, ICardInfo target) {
        var info = new { Title = "", Text = "", Image = "" };
        var itemInfo = JsonConvert.DeserializeAnonymousType(json, info);
        if (itemInfo == null) {
           return false;
        }
        target.Title = itemInfo.Title;
        target.Text = itemInfo.Text;
        target.Image = itemInfo.Image;
        return true;
    }
    public string Title { get; set; }
    public string Text { get; set; }
    public string Image { get; set; }
}