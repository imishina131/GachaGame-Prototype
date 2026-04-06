using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceManager
{
    readonly Dictionary<Type, object> m_services = new();
    public IEnumerable<object> RegisteredServices =>  m_services.Values;
    public T Get<T>() where T : class
    {
        if (!m_services.TryGetValue(typeof(T), out object service)) throw new ArgumentException($"Service of type {typeof(T).FullName} is not registered");
        return service as T;
    }
    public bool TryGet<T>(out T service) where T : class
    {
        Type type = typeof(T);
        if(m_services.TryGetValue(type, out object foundService))
        {
            service = foundService as T;
            return true;
        }
        service = null;
        return false;
    }
    public ServiceManager RegisterService<T>(T service)
    {
        Type serviceType = typeof(T);
        if (!m_services.TryAdd(serviceType, service))
        {
            Debug.LogError($"Service of type {serviceType.FullName} already registered");
        }
        return this;
    }
    public ServiceManager RegisterService(Type serviceType, object service)
    {
        if (!serviceType.IsInstanceOfType(service))
        {
            throw new ArgumentException($"Service of type {serviceType.FullName} does not implement {serviceType.FullName}");
        }
        if (!m_services.TryAdd(serviceType, service))
        {
            Debug.LogError($"Service of type {serviceType.FullName} already registered");
        }
        return this;
    }
}
