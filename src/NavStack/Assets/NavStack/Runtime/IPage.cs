using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace NavStack
{
    public interface IPage
    {
        /// <summary>
        /// Pageを有効にする際に操作するTransform
        /// </summary>
        [CanBeNull]
        UnityEngine.Transform transform { get; }

        /// <summary>
        /// <see cref="transform"/>に対する操作を行わないように要求する
        /// </summary>
        bool controlTransform { get; }
        
        /// <summary>
        /// このページがStackにPushされた時か、シートとしてアクティブになった呼び出される
        /// </summary>
        UniTask OnNavigatePush(NavigationContext context, CancellationToken cancellationToken = default);
        /// <summary>
        /// このページがStackからPopされた時か、シートとして非アクティブになったに呼び出される
        /// </summary>
        UniTask OnNavigatePop(NavigationContext context, CancellationToken cancellationToken = default);
        /// <summary>
        /// このページがStackの最後に存在し、そこに新たなページがPushされる時に呼び出される
        /// </summary>
        UniTask OnNavigatePushToThis(NavigationContext context, CancellationToken cancellationToken = default);
        /// <summary>
        /// このページがStackの最後から1つ手前に存在し、最後のページがPopされた時に呼び出される
        /// </summary>
        UniTask OnNavigatePopToThis(NavigationContext context, CancellationToken cancellationToken = default);
    }
}
