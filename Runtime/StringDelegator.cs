using UnityEngine;
using System;
using UnityEditor;

namespace com.amabie.StringDelegator
{
    public static class StringDelegator
    {
        public static Func<T> ToFunc<T>(this GameObject gameObj, Type classTypeWithNamespace, string methodName)
        {
            var assembly = classTypeWithNamespace.Assembly;
            Type classType = assembly.GetType(classTypeWithNamespace.ToString());
            var component = gameObj.GetComponent(classType);
            var method = classType.GetMethod(methodName);
            return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, method);
        }

        public static Action ToAction(this GameObject gameObj, Type classTypeWithNamespace, string methodName)
        {
            var assembly = classTypeWithNamespace.Assembly;
            Type classType = assembly.GetType(classTypeWithNamespace.ToString());
            var component = gameObj.GetComponent(classType);
            var method = classType.GetMethod(methodName);
            return (Action)Delegate.CreateDelegate(typeof(Action), component, method);
        }
    }
}
