using System;
using SkySwordKill.Next.DialogSystem;
using UnityEngine;
using UnityEngine.UI;

namespace SkySwordKill.Next.DialogEvent
{
    [DialogEvent("ShowCG")]
    public class ShowCG : IDialogEvent
    {
        public void Execute(DialogCommand command, DialogEnvironment env, Action callback)
        {
            var cgName = command.GetStr(0);
            var path = $"Assets/CG/{cgName}.png";
            if (Main.Res.TryGetAsset(path, out var asset)
                && asset is Texture2D texture)
            {
                Sprite sprite = Main.Res.GetSpriteCache(texture);
                CGManager.Instance.CGSprite = sprite;
                CGManager.Instance.Enable = true;
            }
            else
            {
                Main.LogWarning($"背景图片 {path} 不存在。");
            }
            callback?.Invoke();
        }
    }

    public class CGManager : MonoBehaviour
    {
        public static CGManager Instance { get; set; }

        public Canvas Canvas { get; set; }
        public Image Image { get; set; }

        public bool Enable
        {
            get => Instance.gameObject.activeSelf;
            set => Instance.gameObject.SetActive(value);
        }

        public Sprite CGSprite
        {
            get => Image.sprite;
            set => Image.sprite = value;
        }
        
        static CGManager()
        {
            var go = new GameObject("NextCGManager");
            DontDestroyOnLoad(go);
            go.layer = 5;
            Instance = go.AddComponent<CGManager>();
            Instance.Canvas = go.AddComponent<Canvas>();
            Instance.Canvas.sortingOrder = 20;
            Instance.Canvas.planeDistance = 100;
            Instance.Canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var img = new GameObject("CG");
            img.transform.SetParent(go.transform);
            img.layer = 5;
            Instance.Image = img.AddComponent<Image>();
            var imgRect = Instance.Image.rectTransform;
            imgRect.position = Vector3.zero;
            imgRect.localScale = Vector3.one;
            imgRect.anchorMin = Vector2.zero;
            imgRect.anchorMax = Vector2.one;
            imgRect.offsetMax = Vector2.zero;
            imgRect.offsetMin = Vector2.zero;

            Instance.Enable = false;
        }
    }
    
}