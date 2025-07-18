using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace HoshinoLabs.Sardinject.Tests {
    [TestFixture]
    public class ContainerTest : IPrebuildSetup, IPostBuildCleanup {
#if !VRC_CLIENT && UNITY_EDITOR && VRC_SDK_VRCSDK3
        IDictionary InitialTargetAccessFilters {
            get {
                var initialTargetAccessFiltersField = typeof(VRC.Core.UnityEventFilter)
                    .GetField("_initialTargetAccessFilters", BindingFlags.Static | BindingFlags.NonPublic);
                return (IDictionary)initialTargetAccessFiltersField.GetValue(null);
            }
        }
#endif

#if !VRC_CLIENT && UNITY_EDITOR && VRC_SDK_VRCSDK3
        readonly string[] AllowedTarget = new[] {
            "UnityEditor.TestTools.TestRunner.Api.CallbacksDelegatorListener",
            "UnityEditor.TestTools.TestRunner.TestRunnerCallback",
            "UnityEngine.TestRunner.Utils.TestRunCallbackListener",
            "UnityEngine.TestTools.TestRunner.Callbacks.PlayModeRunnerCallback",
        };
#endif

        public void Setup() {
#if !VRC_CLIENT && UNITY_EDITOR && VRC_SDK_VRCSDK3
            var allowedMethodFilterType = typeof(VRC.Core.UnityEventFilter).Assembly.GetTypes()
                .Where(x => x.FullName == "VRC.Core.UnityEventFilter+AllowedMethodFilter")
                .First();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (!AllowedTarget.Contains(type.FullName)) {
                        continue;
                    }
                    InitialTargetAccessFilters.Remove(type);
                    var allowedTargetMethodNames = type
                        .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                        .Select(x => x.Name)
                        .ToList();
                    var allowedTargetPropertyNames = new List<string>();
                    var allowedMethodFilter = Activator.CreateInstance(allowedMethodFilterType, new[] {
                        allowedTargetMethodNames,
                        allowedTargetPropertyNames,
                    });
                    InitialTargetAccessFilters.Add(type, allowedMethodFilter);
                }
            }

#if VRC_SDK_WORLDS3_7_4_OR_NEWER && !VRC_SDK_WORLDS3_8_2_OR_NEWER
            if (VRC.SDKBase.VRC_SceneDescriptor.Instance == null) {
                var gameObjectName = typeof(VRC.SDKBase.VRC_SceneDescriptor).Name;
                var gameObject = new GameObject(typeof(VRC.SDKBase.VRC_SceneDescriptor).Name);
                var component = gameObject.AddComponent<VRC.SDK3.Components.VRCSceneDescriptor>();
                component.spawns = new[] { gameObject.transform };
            }
#endif
#endif
        }

        public void Cleanup() {
#if !VRC_CLIENT && UNITY_EDITOR && VRC_SDK_VRCSDK3
            var allowedMethodFilterType = typeof(VRC.Core.UnityEventFilter).Assembly.GetTypes()
                .Where(x => x.FullName == "VRC.Core.UnityEventFilter+AllowedMethodFilter")
                .First();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    if (!AllowedTarget.Contains(type.FullName)) {
                        continue;
                    }
                    InitialTargetAccessFilters.Remove(type);
                }
            }
#endif
        }

        interface ISimpleComponent {

        }

        class SimpleComponent : MonoBehaviour, ISimpleComponent {

        }

        class SimpleComponentWithValue : MonoBehaviour, ISimpleComponent {
            [Inject]
            public readonly int Value;
        }

        // 型で解決,インスタンスで登録,常に同じ値が返るはず
        [Test]
        public void Resolve_RegisterByInstance_ShouldReturnAlwaysSameInstance() {
            var builder = new ContainerBuilder();
            var simple = new GameObject().AddComponent<SimpleComponent>();
            builder.RegisterInstance(simple);
            var container = builder.Build();
            var simple1 = container.Resolve<SimpleComponent>();
            var simple2 = container.Resolve<SimpleComponent>();
            Assert.AreEqual(simple1, simple);
            Assert.AreEqual(simple2, simple);
            Assert.AreEqual(simple1, simple2);
        }

        // 型で解決,ファクトリで登録,常に新しい値が返るはず
        [Test]
        public void Resolve_RegisterByFactory_ShouldReturnAlwaysANewInstance() {
            var builder = new ContainerBuilder();
            builder.RegisterFactory((_) => new GameObject().AddComponent<SimpleComponent>(), Lifetime.Transient);
            var container = builder.Build();
            var simple1 = container.Resolve<SimpleComponent>();
            var simple2 = container.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple1, simple2);
        }

        // 型で解決,名前指定で値を指定,指定された値が設定されるはず
        [Test]
        public void Resolve_NamedParameterFromValue_ShouldInjectSpecifiedValue() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponentWithValue>(Lifetime.Transient).WithParameter("Value", 123);
            var container = builder.Build();
            Assert.AreEqual(123, container.Resolve<SimpleComponentWithValue>().Value);
        }

        // 型で解決,型で登録,スローされないべき
        [Test]
        public void Resolve_RegisterComponent_ShouldNotThrow() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponent>(Lifetime.Transient);
            var container = builder.Build();
            Assert.DoesNotThrow(() => container.Resolve<SimpleComponent>());
        }

        // 型で解決,インスタンスで登録,スローされないべき
        [Test]
        public void Resolve_RegisterComponentInstance_ShouldNotThrow() {
            var builder = new ContainerBuilder();
            var simple = new GameObject().AddComponent<SimpleComponent>();
            builder.RegisterComponentInstance(simple);
            var container = builder.Build();
            Assert.DoesNotThrow(() => container.Resolve<SimpleComponent>());
        }

        // 型で解決,階層から型で登録,スローされないべき
        [Test]
        public void Resolve_RegisterComponentInHierarchy_ShouldNotThrow() {
            var builder = new ContainerBuilder();
            new GameObject().AddComponent<SimpleComponent>();
            builder.RegisterComponentInHierarchy<SimpleComponent>();
            var container = builder.Build();
            Assert.DoesNotThrow(() => container.Resolve<SimpleComponent>());
        }

        // 型で解決,プレハブで登録,スローされないべき
        [Test]
        public void Resolve_RegisterComponentInNewPrefab_ShouldNotThrow() {
            var builder = new ContainerBuilder();
            var prefab = new GameObject();
            prefab.AddComponent<SimpleComponent>();
            builder.RegisterComponentInNewPrefab<SimpleComponent>(Lifetime.Transient, prefab);
            var container = builder.Build();
            Assert.DoesNotThrow(() => container.Resolve<SimpleComponent>());
        }

        // 型で解決,新規オブジェクト作成で登録,スローされないべき
        [Test]
        public void Resolve_RegisterComponentOnNewGameObject_ShouldNotThrow() {
            var builder = new ContainerBuilder();
            builder.RegisterComponentOnNewGameObject<SimpleComponent>(Lifetime.Transient);
            var container = builder.Build();
            Assert.DoesNotThrow(() => container.Resolve<SimpleComponent>());
        }

        // 型で解決,型で登録し階層を値で指定,指定した階層に作られるはず
        [Test]
        public void Resolve_RegisterComponentWithUnderTransformFromValue_ShouldCreatedUnderHierarchy() {
            var builder = new ContainerBuilder();
            var parent = new GameObject().transform;
            builder.RegisterComponent<SimpleComponent>(Lifetime.Transient).UnderTransform(parent);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponent>();
            Assert.AreEqual(parent, simple.transform);
        }

        // 型で解決,型で登録し階層を生成関数で指定,指定した階層に作られるはず
        [Test]
        public void Resolve_RegisterComponentWithUnderTransformFromFactory_ShouldCreatedUnderHierarchy() {
            var builder = new ContainerBuilder();
            var parent = new GameObject().transform;
            builder.RegisterComponent<SimpleComponent>(Lifetime.Transient).UnderTransform((_) => parent);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponent>();
            Assert.AreEqual(parent, simple.transform);
        }

        // 型で解決,新規オブジェクト作成で登録し階層を値で指定,指定した階層に作られるはず
        [Test]
        public void Resolve_RegisterComponentOnNewGameObjectWithUnderTransformFromValue_ShouldCreatedUnderHierarchy() {
            var builder = new ContainerBuilder();
            var parent = new GameObject().transform;
            builder.RegisterComponentOnNewGameObject<SimpleComponent>(Lifetime.Transient).UnderTransform(parent);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponent>();
            Assert.NotNull(simple.transform.parent);
            Assert.AreEqual(parent, simple.transform.parent);
        }

        // 型で解決,新規オブジェクト作成で登録し階層を生成関数で指定,指定した階層に作られるはず
        [Test]
        public void Resolve_RegisterComponentOnNewGameObjectWithUnderTransformFromFactory_ShouldCreatedUnderHierarchy() {
            var builder = new ContainerBuilder();
            var parent = new GameObject().transform;
            builder.RegisterComponentOnNewGameObject<SimpleComponent>(Lifetime.Transient).UnderTransform((_) => parent);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponent>();
            Assert.NotNull(simple.transform.parent);
            Assert.AreEqual(parent, simple.transform.parent);
        }

        // 型で解決,破棄しないように指定,破棄しないようマークされているはず
        [Test]
        public void Resolve_MarkDontDestroy_ShouldMarkedDontDestroy() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponent>(Lifetime.Transient).DontDestroyOnLoad();
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponent>();
            Assert.AreEqual("DontDestroyOnLoad", container.Resolve<SimpleComponent>().gameObject.scene.name);
        }

        // 型で解決,一時タイプで,常に新しい値を返す
        [Test]
        public void Resolve_AsTransientFromLifetime_ShouldReturnAlwaysANewInstance() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponent>(Lifetime.Transient);
            var container = builder.Build();
            var simple1 = container.Resolve<SimpleComponent>();
            var simple2 = container.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple1, simple2);
        }

        // 型で解決,一時タイプで,常に新しい値を返すが階層は使わない
        [Test]
        public void Resolve_AsTransientFromLifetime_ShouldReturnAlwaysANewInstanceButDoesNotHierarchy() {
            var container1 = new ContainerBuilder().Build();
            var container2 = container1.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Transient));
            var container3 = container2.Scope();
            var container4 = container3.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Transient));
            Assert.Throws<SardinjectException>(() => container1.Resolve<SimpleComponent>());
            var simple1 = container2.Resolve<SimpleComponent>();
            var simple2 = container2.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple1, simple2);
            var simple3 = container3.Resolve<SimpleComponent>();
            var simple4 = container3.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple3, simple1);
            Assert.AreNotEqual(simple4, simple1);
            Assert.AreNotEqual(simple3, simple2);
            Assert.AreNotEqual(simple4, simple2);
            Assert.AreNotEqual(simple3, simple4);
            var simple5 = container4.Resolve<SimpleComponent>();
            var simple6 = container4.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple5, simple1);
            Assert.AreNotEqual(simple6, simple1);
            Assert.AreNotEqual(simple5, simple2);
            Assert.AreNotEqual(simple6, simple2);
            Assert.AreNotEqual(simple5, simple3);
            Assert.AreNotEqual(simple6, simple3);
            Assert.AreNotEqual(simple5, simple4);
            Assert.AreNotEqual(simple6, simple4);
            Assert.AreNotEqual(simple5, simple6);
        }

        // 型で解決,キャッシュタイプで,常に同じ値を返す
        [Test]
        public void Resolve_AsCachedFromLifetime_ShouldReturnAlwaysSameInstance() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponent>(Lifetime.Cached);
            var container = builder.Build();
            var simple1 = container.Resolve<SimpleComponent>();
            var simple2 = container.Resolve<SimpleComponent>();
            Assert.AreEqual(simple1, simple2);
        }

        // 型で解決,キャッシュタイプで,階層間でできるだけ常に同じ値を返す
        [Test]
        public void Resolve_AsCachedFromLifetime_ShouldReturnAlwaysSameInstanceBetweenHierarchyAsMuchAsPossible() {
            var container1 = new ContainerBuilder().Build();
            var container2 = container1.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Cached));
            var container3 = container2.Scope();
            var container4 = container3.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Cached));
            Assert.Throws<SardinjectException>(() => container1.Resolve<SimpleComponent>());
            var simple1 = container2.Resolve<SimpleComponent>();
            var simple2 = container2.Resolve<SimpleComponent>();
            Assert.AreEqual(simple1, simple2);
            var simple3 = container3.Resolve<SimpleComponent>();
            var simple4 = container3.Resolve<SimpleComponent>();
            Assert.AreEqual(simple3, simple1);
            Assert.AreEqual(simple4, simple1);
            Assert.AreEqual(simple3, simple2);
            Assert.AreEqual(simple4, simple2);
            Assert.AreEqual(simple3, simple4);
            var simple5 = container4.Resolve<SimpleComponent>();
            var simple6 = container4.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple5, simple1);
            Assert.AreNotEqual(simple6, simple1);
            Assert.AreNotEqual(simple5, simple2);
            Assert.AreNotEqual(simple6, simple2);
            Assert.AreNotEqual(simple5, simple3);
            Assert.AreNotEqual(simple6, simple3);
            Assert.AreNotEqual(simple5, simple4);
            Assert.AreNotEqual(simple6, simple4);
            Assert.AreEqual(simple5, simple6);
        }

        // 型で解決,スコープタイプで,常に同じ値を返す
        [Test]
        public void Resolve_AsScopedFromLifetime_ShouldReturnAlwaysSameInstance() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponent>(Lifetime.Scoped);
            var container = builder.Build();
            var simple1 = container.Resolve<SimpleComponent>();
            var simple2 = container.Resolve<SimpleComponent>();
            Assert.AreEqual(simple1, simple2);
        }

        // 型で解決,スコープタイプで,常に同じ値を返すが階層は使わない
        [Test]
        public void Resolve_AsScopedFromLifetime_ShouldReturnAlwaysSameInstanceButDoesNotHierarchy() {
            var container1 = new ContainerBuilder().Build();
            var container2 = container1.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Scoped));
            var container3 = container2.Scope();
            var container4 = container3.Scope((builder) => builder.RegisterComponent<SimpleComponent>(Lifetime.Scoped));
            Assert.Throws<SardinjectException>(() => container1.Resolve<SimpleComponent>());
            var simple1 = container2.Resolve<SimpleComponent>();
            var simple2 = container2.Resolve<SimpleComponent>();
            Assert.AreEqual(simple1, simple2);
            var simple3 = container3.Resolve<SimpleComponent>();
            var simple4 = container3.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple3, simple1);
            Assert.AreNotEqual(simple4, simple1);
            Assert.AreNotEqual(simple3, simple2);
            Assert.AreNotEqual(simple4, simple2);
            Assert.AreEqual(simple3, simple4);
            var simple5 = container4.Resolve<SimpleComponent>();
            var simple6 = container4.Resolve<SimpleComponent>();
            Assert.AreNotEqual(simple5, simple1);
            Assert.AreNotEqual(simple6, simple1);
            Assert.AreNotEqual(simple5, simple2);
            Assert.AreNotEqual(simple6, simple2);
            Assert.AreNotEqual(simple5, simple3);
            Assert.AreNotEqual(simple6, simple3);
            Assert.AreNotEqual(simple5, simple4);
            Assert.AreNotEqual(simple6, simple4);
            Assert.AreEqual(simple5, simple6);
        }

        class SimpleComponentWithMethod : MonoBehaviour, ISimpleComponent {
            public bool Ok { get; private set; }
            public int Value { get; private set; }

            [Inject]
            void InjectedMethod(int value) {
                Value = value;
                Ok = true;
            }
        }

        class SimpleComponentWithProperty : MonoBehaviour, ISimpleComponent {
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

        // 型で解決,メソッドが呼び出されている,指定された値が設定されるはず
        [Test]
        public void Resolve_MethodCalled_ShouldInjectSpecifiedValue() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponentWithMethod>(Lifetime.Transient).WithParameter(123);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponentWithMethod>();
            Assert.IsTrue(simple.Ok);
            Assert.AreEqual(123, simple.Value);
        }

        // 型で解決,プロパティが呼び出されている,指定された値が設定されるはず
        [Test]
        public void Resolve_PropertyCalled_ShouldInjectSpecifiedValue() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleComponentWithProperty>(Lifetime.Transient).WithParameter(123);
            var container = builder.Build();
            var simple = container.Resolve<SimpleComponentWithProperty>();
            Assert.IsTrue(simple.Ok);
            Assert.AreEqual(123, simple.Value);
        }

        class SimpleMonoBehaviour : MonoBehaviour, ISimpleComponent {
            public bool Ok { get; private set; }
            public int Value { get; private set; }

            private void Awake() {
                Ok = true;
            }

            private void Update() {
                Value++;
            }
        }

        // 型で解決,Awakeが呼び出されている,Trueになっているはず
        [Test]
        public void Resolve_AwakeCalled_ShouldTrue() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleMonoBehaviour>(Lifetime.Transient);
            var container = builder.Build();
            Assert.IsTrue(container.Resolve<SimpleMonoBehaviour>().Ok);
        }

        // 型で解決,Updateが呼び出されている,1以上になっているはず
        [UnityTest]
        public IEnumerator Resolve_UpdateCalled_ShoulOneOrHigher() {
            var builder = new ContainerBuilder();
            builder.RegisterComponent<SimpleMonoBehaviour>(Lifetime.Transient);
            var container = builder.Build();
            var simple = container.Resolve<SimpleMonoBehaviour>();
            yield return null;
            Assert.IsTrue(simple.Ok);
            Assert.LessOrEqual(1, simple.Value);
        }
    }
}
