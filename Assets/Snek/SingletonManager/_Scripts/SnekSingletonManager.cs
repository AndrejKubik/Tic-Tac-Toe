using System.Collections.Generic;
using Snek.Utilities;
using UnityEngine;

namespace Snek.SingletonManager
{
    [UseSnekInspector]
    public class SnekSingletonManager : SnekMonoBehaviour
    {
        public List<SnekMonoBehaviour> Singletons;
        public List<SnekMonoBehaviour> SingletonPrefabs;

        private static SnekSingletonManager _instance;

        private static bool _isInitialized = false;

        protected override void Initialize()
        {
            if (_instance != null)
                Destroy(gameObject);

            _instance = this;
            _isInitialized = false;

            CreateSingletonInstances();

            _isInitialized = true;
        }

        protected override void Validate()
        {
            if (Singletons.Count != SingletonPrefabs.Count)
                FailValidation("Duplicate singleton prefabs found.");
        }

        protected override void OnInitializationSuccess()
        {
            DontDestroyOnLoad(this);
        }

        private void CreateSingletonInstances()
        {
            Singletons = new List<SnekMonoBehaviour>();

            foreach (SnekMonoBehaviour prefab in SingletonPrefabs)
                CreateSingletonInstance(prefab);
        }

        private void CreateSingletonInstance(SnekMonoBehaviour prefab)
        {
            SnekMonoBehaviour singletonInstance = Instantiate(prefab, transform);
            singletonInstance.name = prefab.name;

            Singletons.Add(singletonInstance);
        }

        public static T GetSingleton<T>() where T : SnekMonoBehaviour
        {
            if(!_isInitialized)
            {
                Debug.LogError(
                    "Trying to get a singleton reference before Singleton Manager is initialized.\n" +
                    "Make sure to use a bootstrapper, disable dependencies until initialization is complete or initialize dependencies through Start().");

                return null;
            }

            foreach (SnekMonoBehaviour singleton in _instance.Singletons)
                if (singleton is T instance)
                    return instance;

            Debug.LogError($"Cannot find singleton of type {typeof(T)}.");

            return null;
        }
    }
}