using UnityEngine;
using System;
using UnityEditor;

namespace com.amabie.StringDelegator
{
    /// <summary>
    /// 実行したいクラスの Type インスタンスと実行したいメソッドの string を与えて実行する Extension
    /// NOTE: gameObj に実行したい MonoBehaviour インスタンスが付与されていることを前提としている
    /// 当初は string だけを渡して実行したかったができなかったので名前だけ名残がある
    /// Type インスタンスではなく string を引数に与えて、この Extension の中で Type.GetType しようとしても
    /// パッケージ外部のクラスの Type インスタンスを生成できなかったのでやむなくこの形に。
    /// </summary>
    public static class StringDelegator
    {
        /// <summary>
        /// 引数に指定したメソッドを Func オブジェクトとして返す
        /// </summary>
        /// <typeparam name="T">bool, int, string など</typeparam>
        /// <param name="gameObj">実行したい MonoBehaviour クラスが付与されたゲームオブジェクト</param>
        /// <param name="classTypeWithNamespace">Type インスタンス(ネームスペースを含む)</param>
        /// <param name="methodName">実行したいメソッド名</param>
        /// <returns>Func<T>オブジェクト</returns>
        public static Func<T> ToFunc<T>(this GameObject gameObj, Type classTypeWithNamespace, string methodName)
        {
            if (classTypeWithNamespace == null) {
                throw new StringDelegatorException("Argument 1st -- type instance cannot be null");
            }
            var assembly = classTypeWithNamespace.Assembly;
            Type classType = assembly.GetType(classTypeWithNamespace.ToString());
            var component = gameObj.GetComponent(classType);
            if (component == null)
            {
                throw new StringDelegatorException("GameObject does not have class type component");
            }
            var method = classType.GetMethod(methodName);
            if (method == null)
            {
                throw new StringDelegatorException("Argument 2nd -- string method name cannot be found");
            }
            try
            {
                return (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, method);
            }
            catch (ArgumentException e) {
                throw new StringDelegatorException($"TypeParameter T -- {e.Message}");
            }
        }


        /// <summary>
        /// 引数に指定したメソッドを Action オブジェクトとして返す
        /// </summary>
        /// <param name="gameObj">実行したい MonoBehaviour クラスが付与されたゲームオブジェクト</param>
        /// <param name="classTypeWithNamespace">Type インスタンス(ネームスペースを含む)</param>
        /// <param name="methodName">実行したいメソッド名</param>
        /// <returns>Actionオブジェクト</returns>
        public static Action ToAction(this GameObject gameObj, Type classTypeWithNamespace, string methodName)
        {
            if (classTypeWithNamespace == null)
            {
                throw new StringDelegatorException("Argument 1st -- type instance cannot be null");
            }
            var assembly = classTypeWithNamespace.Assembly;
            Type classType = assembly.GetType(classTypeWithNamespace.ToString());
            var component = gameObj.GetComponent(classType);
            if (component == null)
            {
                throw new StringDelegatorException("GameObject does not have class type component");
            }
            var method = classType.GetMethod(methodName);
            if (method == null)
            {
                throw new StringDelegatorException("Argument 2nd -- string method name cannot be found");
            }
            return (Action)Delegate.CreateDelegate(typeof(Action), component, method);
        }
    }

    public class StringDelegatorException : Exception {
        public StringDelegatorException(string message): base(message) { }
    }
}
