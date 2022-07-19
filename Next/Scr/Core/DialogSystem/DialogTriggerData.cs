using Newtonsoft.Json;

namespace SkySwordKill.Next.DialogSystem
{
    public class DialogTriggerData
    {
        #region 字段
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "triggerEvent")]
        public string TriggerEvent { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "bindData")]
        public string BindData { get; set; } = string.Empty;
        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }  = 0;

        #endregion

        #region 属性



        #endregion

        #region 回调方法



        #endregion

        #region 公共方法



        #endregion

        #region 私有方法



        #endregion


    }
}