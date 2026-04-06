using UnityEngine;
using UnityEngine.Serialization;
[AddComponentMenu("Service Locator/Global Service Locator")]
public class ServiceLocatorGlobalBootstrapper : ServiceLocatorBootstrapper
{
    [SerializeField] bool m_dontDestroyOnLoad = true;
    protected override void Bootstrap()
    {
        Container.ConfigureAsGlobal(m_dontDestroyOnLoad);
    }
}