using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceLocator : MonoBehaviour
{
    readonly ServiceManager m_serviceManager = new();
    static ServiceLocator global;
    static Dictionary<Scene, ServiceLocator> sceneContainers = new();
    static List<GameObject> tempSceneGameObjects = new();
    
    const string GlobalServiceLocatorName = "ServiceLocator [Global]";
    const string SceneServiceLocatorName = "ServiceLocator [Scene]";

    internal void ConfigureAsGlobal(bool dontDestroyOnLoad)
    {
        if (global == this)
        {
            Debug.LogWarning("ServiceLocator.ConfigureAsGlobal: Already configured as global");
        }
        else if (global != null)
        {
            Debug.LogError("ServiceLocator.ConfigureAsGlobal: Another service locator already configured as global");
        }
        else
        {
            global = this;
            if(dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }
    }
    internal void ConfigureAsScene()
    {
        Scene scene = gameObject.scene;
        if (sceneContainers.ContainsKey(scene))
        {
            Debug.LogWarning("ServiceLocator.ConfigureAsScene: Another service locator already configured for this scene");
            return;
        }
        sceneContainers.Add(scene, this);
    }
    public static ServiceLocator Global
    {
        get
        {
            if (global != null) return global;
            if (FindFirstObjectByType<ServiceLocatorGlobalBootstrapper>() is {  } found)
            {
                found.BootstrapOnDemand();
                return global;
            }
            GameObject container = new(GlobalServiceLocatorName, typeof(ServiceLocator));
            container.AddComponent<ServiceLocatorGlobalBootstrapper>().BootstrapOnDemand();
            return global;
        }
    }
    
    public static ServiceLocator ForSceneOf(MonoBehaviour monoBehaviour)
    {
        Scene scene =  monoBehaviour.gameObject.scene;
        if (sceneContainers.TryGetValue(scene, out ServiceLocator value))
        {
            return value;
        }
        tempSceneGameObjects.Clear();
        scene.GetRootGameObjects(tempSceneGameObjects);
        foreach (GameObject go in tempSceneGameObjects.Where(go => go.GetComponent<ServiceLocatorSceneBootstrapper>() != null))
        {
            if (!go.TryGetComponent(out ServiceLocatorSceneBootstrapper bootstrapper) || bootstrapper.Container == monoBehaviour) continue;
            bootstrapper.BootstrapOnDemand();
            return bootstrapper.Container;
        }
        return Global;
    }
    public static ServiceLocator For(MonoBehaviour monoBehavior)
    {
        return monoBehavior.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(monoBehavior) ?? Global;
    }
    public ServiceLocator Register<T>(T service)
    {
        m_serviceManager.RegisterService(service);
        return this;
    }
    public ServiceLocator Register(Type type, object service)
    {
        m_serviceManager.RegisterService(type, service);
        return this;
    }
    public ServiceLocator Get<T>(out T service) where T : class
    {
        if (TryGetService(out service)) return this;
        if (!TryGetNextInHierarchy(out ServiceLocator container)) throw new ArgumentException($"Could not find service of type {typeof(T)}");
        container.Get(out service);
        return this;
    }
    bool TryGetService<T>(out T service) where T : class
    {
        return m_serviceManager.TryGet(out service);
    }
    bool TryGetNextInHierarchy(out ServiceLocator container)
    {
        if (this == global)
        {
            container = null;
            return false;
        }
        container = transform.parent.OrNull()?.GetComponentInParent<ServiceLocator>().OrNull() ?? ForSceneOf(this);
        return container !=  null;
    }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        global = null;
        sceneContainers = new();
        tempSceneGameObjects = new();
    }
    void OnDestroy()
    {
        if (this == global)
        {
            global = null;
        }else if (sceneContainers.ContainsValue(this))
        {
            sceneContainers.Remove(gameObject.scene);
        }
    }
    #if UNITY_EDITOR
    [MenuItem("GameObject/Service Locator/Add Global")]
    static void AddGlobal()
    {
        GameObject go = new(GlobalServiceLocatorName, typeof(ServiceLocatorGlobalBootstrapper));
    }
    [MenuItem("GameObject/Service Locator/Add Scene")]
    static void AddScene()
    {
        GameObject go = new(SceneServiceLocatorName, typeof(ServiceLocatorSceneBootstrapper));
    }
    #endif
}