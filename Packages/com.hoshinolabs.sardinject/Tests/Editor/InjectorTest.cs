using NUnit.Framework;
using System.Collections.Generic;

namespace HoshinoLabs.Sardinject.Tests {
    [TestFixture]
    public class InjectorTest {
        class Simple {

        }

        // Injector���擾���āAConstruct���\�b�h���e�X�g����
        [Test]
        public void Construct_ShouldReturnInstance() {
            var container = new ContainerBuilder().Build();
            var type = typeof(Simple);
            var layout = TypeLayoutCache.GetOrBuild(type);
            var injector = new Injector(layout);
            var instance = injector.Construct(container, new Dictionary<object, IResolver>());
            Assert.IsNotNull(instance);
            Assert.IsInstanceOf<Simple>(instance);
        }
    }
}
