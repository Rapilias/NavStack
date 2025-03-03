using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using NavStack;
using LitMotion;
using LitMotion.Extensions;

public class SamplePage1 : MonoBehaviour, IPage, IPageStackEvent
{
    [SerializeField] Text text;
    
    /// <inheritdoc />
    public bool controlTransform => true;
    /// <inheritdoc />
    public UniTask OnNavigatePush(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    /// <inheritdoc />
    public UniTask OnNavigatePop(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    /// <inheritdoc />
    public UniTask OnNavigatePushToThis(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    /// <inheritdoc />
    public UniTask OnNavigatePopToThis(NavigationContext context, CancellationToken cancellationToken = default) => UniTask.CompletedTask;
    
    public async UniTask OnPush(NavigationContext context, CancellationToken cancellationToken = default)
    {
        text.text = context.Parameters["id"] as string;

        if (context.Parameters.ContainsKey("DisableAnimation"))
        {
            transform.localScale = Vector3.one;
            return;
        }

        await LMotion.Create(Vector3.zero, Vector3.one, 0.25f)
            .WithEase(Ease.InQuad)
            .BindToLocalScale(transform)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    public async UniTask OnPop(NavigationContext context, CancellationToken cancellationToken = default)
    {
        if (context.Parameters.ContainsKey("DisableAnimation"))
        {
            transform.localScale = Vector3.zero;
            return;
        }

        await LMotion.Create(Vector3.one, Vector3.zero, 0.25f)
            .WithEase(Ease.OutQuad)
            .BindToLocalScale(transform)
            .ToUniTask(CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, cancellationToken).Token);
    }

    internal string GetCurrentText()
    {
        return text.text;
    }
}
