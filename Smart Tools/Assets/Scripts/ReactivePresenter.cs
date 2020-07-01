using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public abstract class ReactivePresenter <TModel>:MonoBehaviour
{
    private readonly ReactiveProperty<TModel> m_model = new ReactiveProperty<TModel>();

    /// <summary>
    /// Could be used for the subscribtion on different events
    /// </summary>
    protected readonly CompositeDisposable Subscriptions = new CompositeDisposable();

    public IReadOnlyReactiveProperty<TModel> Model
    {
        get { return m_model; }
    }

    protected virtual void OnEnable()
    {
    }

    protected virtual void OnDisable()
    {
        Subscriptions.Clear();
    }

    /// <summary>
    /// Call this method if you don't use this method inside the sequence. 
    /// </summary>
    public virtual void SetModel(TModel model)
    {
        m_model.Value = model;
    }
}
