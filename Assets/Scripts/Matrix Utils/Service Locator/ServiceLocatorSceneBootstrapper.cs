using UnityEngine;
[AddComponentMenu("Service Locator/Scene Service Locator")]
public class ServiceLocatorSceneBootstrapper : ServiceLocatorBootstrapper
{
    protected override void Bootstrap()
    {
        Container.ConfigureAsScene();
    }
}
