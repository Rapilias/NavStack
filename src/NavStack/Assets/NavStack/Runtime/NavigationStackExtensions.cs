using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack.Content;

namespace NavStack
{
    public static class NavigationStackExtensions
    {
        public static UniTask PushAsync(this INavigationStack navigationStack, IPage page, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(page, navigationStack.DefaultOptions, cancellationToken);
        }

        public static UniTask PushAsync(this INavigationStack navigationStack, Func<UniTask<IPage>> factory, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(factory, navigationStack.DefaultOptions, cancellationToken);
        }

        public static UniTask PushNewObjectAsync<T>(this INavigationStack navigationStack, T prefab, NavigationOptions options, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object, IPage
        {
            return navigationStack.PushAsync(() =>
            {
                var instance = UnityEngine.Object.Instantiate(prefab);
                if (instance is Component component)
                {
                    instance.LifecycleEvents.Add(new DestroyObjectEvent(component.gameObject));
                }
                return new(instance);
            }, options, cancellationToken);
        }

        public static UniTask PushNewObjectAsync<T>(this INavigationStack navigationStack, T prefab, CancellationToken cancellationToken = default)
            where T : UnityEngine.Object, IPage
        {
            return PushNewObjectAsync(navigationStack, prefab, navigationStack.DefaultOptions, cancellationToken);
        }

        public static UniTask PushNewObjectAsync(this INavigationStack navigationStack, string key, CancellationToken cancellationToken = default)
        {
            return PushNewObjectAsync(navigationStack, key, ResourceProvider.DefaultResourceProvider, cancellationToken);
        }

        public static UniTask PushNewObjectAsync(this INavigationStack navigationStack, string key, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return PushNewObjectAsync(navigationStack, key, ResourceProvider.DefaultResourceProvider, options, cancellationToken);
        }

        public static UniTask PushNewObjectAsync(this INavigationStack navigationStack, string key, IResourceProvider resourceProvider, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(async () =>
            {
                var resource = await resourceProvider.LoadAsync<UnityEngine.Object>(key, cancellationToken);

                var instance = UnityEngine.Object.Instantiate(resource);
                if (!TryGetComponent<IPage>(instance, out var page)) throw new Exception(); // TODO:

                page.LifecycleEvents.Add(new ResourceUnloadEvent(page, resource, instance, resourceProvider));

                return page;
            }, navigationStack.DefaultOptions, cancellationToken);
        }

        public static UniTask PushNewObjectAsync(this INavigationStack navigationStack, string key, IResourceProvider resourceProvider, NavigationOptions options, CancellationToken cancellationToken = default)
        {
            return navigationStack.PushAsync(async () =>
            {
                var resource = await resourceProvider.LoadAsync<UnityEngine.Object>(key, cancellationToken);

                var instance = UnityEngine.Object.Instantiate(resource);
                if (!TryGetComponent<IPage>(instance, out var page)) throw new Exception(); // TODO:

                page.LifecycleEvents.Add(new ResourceUnloadEvent(page, resource, instance, resourceProvider));

                return page;
            }, options, cancellationToken);
        }

        public static UniTask PopAsync(this INavigationStack navigation, CancellationToken cancellationToken = default)
        {
            return navigation.PopAsync(navigation.DefaultOptions, cancellationToken);
        }

        static bool TryGetComponent<T>(UnityEngine.Object obj, out T result)
        {
            if (obj is GameObject gameObject)
            {
                result = gameObject.GetComponent<T>();
                return true;
            }

            if (obj is Component component)
            {
                result = component.GetComponent<T>();
                return true;
            }

            result = default;
            return false;
        }
    }
}