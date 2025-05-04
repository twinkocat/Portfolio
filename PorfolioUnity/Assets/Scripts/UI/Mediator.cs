using VContainer;
using VContainer.Unity;

public abstract class Mediator
{
    
}

public abstract class Mediator<T> : Mediator where T : View
{
    [Inject] private UIRoot root;
    [Inject] private T prefab;

    private T viewInstance;

    protected T View
    {
        get
        {
            if (viewInstance)
                return viewInstance;

            viewInstance = Game.Resolver.Instantiate(prefab, root.transform);
            viewInstance.gameObject.SetActive(false);
            root.Add(viewInstance);
            root.UpdateRootOrder();

            return viewInstance;
        }
    }

    public virtual void Show() => View.gameObject.SetActive(true);
    public virtual void Hide() => View.gameObject.SetActive(false);
}
