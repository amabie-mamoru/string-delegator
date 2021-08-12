using System;
using NUnit.Framework;
using UnityEngine;

namespace com.amabie.StringDelegator
{
    public class StringDelegatorTest
    {
        GameObject a;
        GameObject b;

        public void Initialize()
        {
            a = new GameObject();
            b = new GameObject();
            a.AddComponent<AClass>();
        }

        [Test]
        public void ToFunc()
        {
            Initialize();
            // 正常系
            Assert.That(() => a.ToFunc<int>(typeof(AClass), "DummyFunc"), Throws.Nothing);
            var func = a.ToFunc<int>(typeof(AClass), "DummyFunc");
            Assert.AreEqual(func(), 1);

            // 異常系
            Assert.That(() => a.ToFunc<int>(Type.GetType("NoClass"), "DummyFunc"), Throws.TypeOf<StringDelegatorException>());
            Assert.That(() => a.ToFunc<int>(typeof(AClass), "DontExistFunc"), Throws.TypeOf<StringDelegatorException>());
            Assert.That(() => a.ToFunc<bool>(typeof(AClass), "DummyFunc"), Throws.TypeOf<StringDelegatorException>());
            Assert.That(() => b.ToFunc<int>(typeof(AClass), "DummyFunc"), Throws.TypeOf<StringDelegatorException>());
        }

        [Test]
        public void ToAction()
        {
            Initialize();
            // 正常系
            Assert.That(() => a.ToAction(typeof(AClass), "DummyAction"), Throws.Nothing);
            var func = a.ToAction(typeof(AClass), "DummyAction");

            Assert.That(() => a.ToAction(Type.GetType("NoClass"), "DummyAction"), Throws.TypeOf<StringDelegatorException>());
            Assert.That(() => a.ToAction(typeof(AClass), "DontExistAction"), Throws.TypeOf<StringDelegatorException>());
            Assert.That(() => b.ToAction(typeof(AClass), "DummyAction"), Throws.TypeOf<StringDelegatorException>());

        }

        public class AClass: MonoBehaviour
        {
            public int DummyFunc() => 1;
            public void DummyAction() { }
        }
    }
}