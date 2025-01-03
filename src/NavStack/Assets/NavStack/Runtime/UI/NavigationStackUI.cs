#if NAVSTACK_UGUI_SUPPORT
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Internal;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Stack UI")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationStackUI : MonoBehaviour, INavigationStack
    {
        [SerializeField] RectTransform parentTransform;

        readonly NavigationStackCore core = new();

        public event Action<IPage> OnPageAttached
        {
            add => core.OnPageAttached += value;
            remove => core.OnPageAttached -= value;
        }

        public event Action<IPage> OnPageDetached
        {
            add => core.OnPageDetached += value;
            remove => core.OnPageDetached -= value;
        }

        public event Action<(IPage Previous, IPage Current)> OnNavigated
        {
            add => core.OnNavigated += value;
            remove => core.OnNavigated -= value;
        }
        
        public event Action<(IPage Previous, IPage Current)> OnNavigating
        {
            add => core.OnNavigating += value;
            remove => core.OnNavigating -= value;
        }

        public IPage ActivePage => core.ActivePage;
        public IReadOnlyCollection<IPage> Pages => core.Pages;

        public bool isTransitioning { get; set; }

        protected virtual void Awake()
        {
            OnPageAttached += page =>
            {
                if (page is Component component)
                {
                    if (parentTransform != null)
                    {
                        component.transform.SetParent(parentTransform, false);
                    }
                }
            };
        }

        public async UniTask PopAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.PopAsync(context, cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask PushAsync(IPage page, NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.PushAsync(() => new(page), context, cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask PushAsync(Func<UniTask<IPage>> factory, NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.PushAsync(factory, context, cancellationToken);
            this.isTransitioning = false;
        }
    }
}
#endif
