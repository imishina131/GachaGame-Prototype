using UnityEngine;
[DisallowMultipleComponent, RequireComponent(typeof(ServiceLocator))]
public abstract class ServiceLocatorBootstrapper : MonoBehaviour
{
    ServiceLocator m_container;
    internal ServiceLocator Container => m_container.OrNull() ?? (m_container = GetComponent<ServiceLocator>());
    bool m_hasBeenBootstrapped;
    void Awake() => BootstrapOnDemand();
    public void BootstrapOnDemand()
    {
        if(m_hasBeenBootstrapped) return;
        m_hasBeenBootstrapped = true;
        Bootstrap();
    }
    protected abstract void Bootstrap();
}