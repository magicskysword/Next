using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SkySwordKill.Next.Utils;

public class MainPanelButtonAnimation : MonoBehaviour
{
    /// <summary>
    /// 按钮动画结束后的位置
    /// </summary>
    public Vector3 PrimaryPosition;
    /// <summary>
    /// 按钮开始动画时的偏移位置
    /// </summary>
    public Vector3 OffsetPosition = new Vector3(-25, 0, 0);
    /// <summary>
    /// 延迟多久播放动画
    /// </summary>
    public float DelayTime = 0.75f;
    /// <summary>
    /// 动画持续时长
    /// </summary>
    public float AnimationTIme = 1f;
    private void Awake()
    {
        PrimaryPosition = transform.localPosition;
    }
    
    private void OnEnable()
    {
        RunAnimation().Forget();
    }
    
    /// <summary>
    /// 播放动画，下一帧开始执行
    /// </summary>
    public async UniTaskVoid RunAnimation()
    {
        var btnImg = gameObject.GetComponent<Image>();
        btnImg.color = new Color(1, 1, 1, 0f);

        await UniTask.WaitForEndOfFrame(this);
        
        btnImg.transform.localPosition = PrimaryPosition;
        btnImg.transform.MoveLocal(OffsetPosition);
        var sequence = DOTween.Sequence();
        sequence.Insert(DelayTime,btnImg.DOColor(new Color(1, 1, 1, 1), 0.5f) );
        sequence.Insert(DelayTime, btnImg.transform.DOLocalMove(PrimaryPosition, 0.5f));
    }
}