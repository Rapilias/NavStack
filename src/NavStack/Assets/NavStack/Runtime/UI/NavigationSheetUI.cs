#if NAVSTACK_UGUI_SUPPORT
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Internal;

namespace NavStack.UI
{
    [AddComponentMenu("NavStack/Navigation Sheet UI")]
    [RequireComponent(typeof(RectTransform))]
    public class NavigationSheetUI : MonoBehaviour, INavigationSheet
    {
        [SerializeField] RectTransform parentTransform;

        readonly NavigationSheetCore core = new();

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

        public IReadOnlyCollection<IPage> Pages => core.Pages;
        public IPage ActivePage => core.ActivePage;

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

        public async UniTask AddAsync(IPage page, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await  core.AddAsync(page, cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask RemoveAsync(IPage page, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.RemoveAsync(page, cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask RemoveAllAsync(CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.RemoveAllAsync(cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask ShowAsync(int index, NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.ShowAsync(index, context, cancellationToken);
            this.isTransitioning = false;
        }

        public async UniTask HideAsync(NavigationContext context, CancellationToken cancellationToken = default)
        {
            this.isTransitioning = true;
            await core.HideAsync(context, cancellationToken);
            this.isTransitioning = false;
        }
    }
}
#endif
