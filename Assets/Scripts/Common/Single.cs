using System.Collections;
using UnityEngine;

namespace Common
{
    public class MonoSingle
    {

    }

    public class PhySingle
    {

    }

    public class Single<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (_instance == null) {
                    _instance = new T();
                }
                return (T)_instance;
            }
        }

        private static T _instance = default(T);
    }

    public class Single<T, Q> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null) {
                    string na = typeof(Q).Name;
                    GameObject go = GameObject.Find(na);
                    if (go == null) {
                        go = new GameObject(na);
                    }
                    _instance = go.GetComponent<T>();
                    if (_instance == null) {
                        _instance = go.AddComponent<T>();
                    }
                }
                return (T)_instance;
            }
        }

        private static T _instance = null;

        public static void Destroy()
        {
            GameObject.Destroy(_instance);
        }

        void Awake()
        {
            _instance = this.GetComponent<T>();
            DontDestroyOnLoad(_instance);
        }
    }
}
