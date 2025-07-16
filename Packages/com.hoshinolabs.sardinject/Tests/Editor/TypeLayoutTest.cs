using NUnit.Framework;

namespace HoshinoLabs.Sardinject.Tests {
    [TestFixture]
    public class TypeLayoutTest {
        class Simple {

        }

        // TypeLayoutの取得
        [Test]
        public void GetOrBuild_ShouldReturnTypeLayout() {
            var type = typeof(Simple);
            var layout = TypeLayoutCache.GetOrBuild(type);
            Assert.IsNotNull(layout);
            Assert.AreEqual(type, layout.Type);
            Assert.IsNotNull(layout.Constructor);
            Assert.IsEmpty(layout.Constructor.Parameters);
            Assert.IsNotNull(layout.Fields);
            Assert.IsEmpty(layout.Fields);
            Assert.IsNotNull(layout.Properties);
            Assert.IsEmpty(layout.Properties);
            Assert.IsNotNull(layout.Methods);
            Assert.IsEmpty(layout.Methods);
        }

        class SimpleWithConstructor : Simple {
            public bool Ok { get; }
            public int Value { get; }

            [Inject]
            SimpleWithConstructor(int value) {
                Value = value;
                Ok = true;
            }
        }

        class SimpleWithField : Simple {
            [Inject]
            public readonly int Value;
        }

        class SimpleWithProperty : Simple {
            public bool Ok { get; private set; }
            public int Value { get; private set; }

            [Inject]
            int InjectedProperty {
                set {
                    Value = value;
                    Ok = true;
                }
            }
        }

        class SimpleWithMethod : Simple {
            public bool Ok { get; private set; }
            public int Value { get; private set; }

            [Inject]
            void InjectedMethod(int value) {
                Value = value;
                Ok = true;
            }
        }

        // TypeLayoutを取得して、コンストラクタが正しく設定されていることを確認
        [Test]
        public void GetOrBuild_ShouldReturnTypeLayoutWithConstructor() {
            var type = typeof(SimpleWithConstructor);
            var layout = TypeLayoutCache.GetOrBuild(type);
            Assert.IsNotNull(layout);
            Assert.AreEqual(type, layout.Type);
            Assert.IsNotNull(layout.Constructor);
            Assert.IsNotEmpty(layout.Constructor.Parameters);
            Assert.AreEqual(1, layout.Constructor.Parameters.Length);
            Assert.AreEqual("value", layout.Constructor.Parameters[0].Name);
            Assert.AreEqual(typeof(int), layout.Constructor.Parameters[0].ParameterType);
            Assert.IsNull(layout.Constructor.Parameters[0].Id);
            var obj = layout.Constructor.Invoke(new object[] { 123 });
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<SimpleWithConstructor>(obj);
            var simple = (SimpleWithConstructor)obj;
            Assert.IsTrue(simple.Ok);
            Assert.AreEqual(123, simple.Value);
        }

        // TypeLayoutを取得して、フィールドが正しく設定されていることを確認
        [Test]
        public void GetOrBuild_ShouldReturnTypeLayoutWithField() {
            var type = typeof(SimpleWithField);
            var layout = TypeLayoutCache.GetOrBuild(type);
            Assert.IsNotNull(layout);
            Assert.AreEqual(type, layout.Type);
            Assert.IsNotNull(layout.Constructor);
            Assert.IsEmpty(layout.Constructor.Parameters);
            Assert.IsNotNull(layout.Fields);
            Assert.IsNotEmpty(layout.Fields);
            Assert.AreEqual(1, layout.Fields.Length);
            Assert.AreEqual("Value", layout.Fields[0].Name);
            Assert.AreEqual(typeof(int), layout.Fields[0].FieldType);
            Assert.IsNull(layout.Fields[0].Id);
            var obj = layout.Constructor.Invoke(new object[] { });
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<SimpleWithField>(obj);
            var simple = (SimpleWithField)obj;
            layout.Fields[0].SetValue(simple, 123);
            Assert.AreEqual(123, simple.Value);
        }

        // TypeLayoutを取得して、プロパティが正しく設定されていることを確認
        [Test]
        public void GetOrBuild_ShouldReturnTypeLayoutWithProperty() {
            var type = typeof(SimpleWithProperty);
            var layout = TypeLayoutCache.GetOrBuild(type);
            Assert.IsNotNull(layout);
            Assert.AreEqual(type, layout.Type);
            Assert.IsNotNull(layout.Constructor);
            Assert.IsEmpty(layout.Constructor.Parameters);
            Assert.IsNotNull(layout.Properties);
            Assert.IsNotEmpty(layout.Properties);
            Assert.AreEqual(1, layout.Properties.Length);
            Assert.AreEqual("InjectedProperty", layout.Properties[0].Name);
            Assert.AreEqual(typeof(int), layout.Properties[0].PropertyType);
            Assert.IsNull(layout.Properties[0].Id);
            var obj = layout.Constructor.Invoke(new object[] { });
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<SimpleWithProperty>(obj);
            var simple = (SimpleWithProperty)obj;
            layout.Properties[0].SetValue(simple, 123);
            Assert.IsTrue(simple.Ok);
            Assert.AreEqual(123, simple.Value);
        }

        // TypeLayoutを取得して、メソッドが正しく設定されていることを確認
        [Test]
        public void GetOrBuild_ShouldReturnTypeLayoutWithMethod() {
            var type = typeof(SimpleWithMethod);
            var layout = TypeLayoutCache.GetOrBuild(type);
            Assert.IsNotNull(layout);
            Assert.AreEqual(type, layout.Type);
            Assert.IsNotNull(layout.Constructor);
            Assert.IsEmpty(layout.Constructor.Parameters);
            Assert.IsNotNull(layout.Methods);
            Assert.IsNotEmpty(layout.Methods);
            Assert.AreEqual(1, layout.Methods.Length);
            Assert.AreEqual("InjectedMethod", layout.Methods[0].Name);
            Assert.IsNotEmpty(layout.Methods[0].Parameters);
            Assert.AreEqual(1, layout.Methods[0].Parameters.Length);
            Assert.AreEqual("value", layout.Methods[0].Parameters[0].Name);
            Assert.AreEqual(typeof(int), layout.Methods[0].Parameters[0].ParameterType);
            var obj = layout.Constructor.Invoke(new object[] { });
            Assert.IsNotNull(obj);
            Assert.IsInstanceOf<SimpleWithMethod>(obj);
            var simple = (SimpleWithMethod)obj;
            layout.Methods[0].Invoke(simple, new object[] { 123 });
            Assert.IsTrue(simple.Ok);
            Assert.AreEqual(123, simple.Value);
        }
    }
}
