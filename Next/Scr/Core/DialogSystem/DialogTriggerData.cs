using Newtonsoft.Json;

namespace SkySwordKill.Next.DialogSystem
{
    public class DialogTriggerData
    {
        #region 字段
        /// <summary>
        /// 触发器ID
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; } = string.Empty;
        /// <summary>
        /// 触发类型
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } = string.Empty;
        /// <summary>
        /// 触发条件
        /// </summary>
        [JsonProperty(PropertyName = "condition")]
        public string Condition { get; set; } = string.Empty;
        /// <summary>
        /// 触发事件
        /// </summary>
        [JsonProperty(PropertyName = "triggerEvent")]
        public string TriggerEvent { get; set; } = string.Empty;
        /// <summary>
        /// 绑定数据
        /// </summary>
        [JsonProperty(PropertyName = "bindData")]
        public string BindData { get; set; } = string.Empty;
        /// <summary>
        /// 触发优先级
        /// </summary>
        [JsonProperty(PropertyName = "priority")]
        public int Priority { get; set; }  = 0;
        /// <summary>
        /// 初始状态
        /// </summary>
        [JsonProperty(PropertyName = "default")]
        public bool Default { get; set; } = true;
        /// <summary>
        /// 触发一次后关闭
        /// </summary>
        [JsonProperty(PropertyName = "once")]
        public bool Once { get; set; } = false;

        #endregion
    }
}