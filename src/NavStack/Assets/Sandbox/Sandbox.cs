using Cysharp.Threading.Tasks;
using UnityEngine;
using NavStack;
using NavStack.UI;
using NavStack.Content;
using R3;

public class Sandbox : MonoBehaviour
{
    [SerializeField] NavigationStackUI navigation;

    void Start()
    {
        navigation.OnNavigatingAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("OnNavigating");
                Debug.Log(x.Previous == null ? "null" : ((SamplePage1)x.Previous).GetCurrentText());
                Debug.Log(x.Current == null ? "null" : ((SamplePage1)x.Current).GetCurrentText());
            })
            .AddTo(this);

        navigation.OnNavigatedAsObservable()
            .Subscribe(x =>
            {
                Debug.Log("OnNavigated");
                Debug.Log(x.Previous == null ? "null" : ((SamplePage1)x.Previous).GetCurrentText());
                Debug.Log(x.Current == null ? "null" : ((SamplePage1)x.Current).GetCurrentText());
            })
            .AddTo(this);
    }
}
