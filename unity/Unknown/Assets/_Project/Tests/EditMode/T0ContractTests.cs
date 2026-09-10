#if TIDE_TEST_FRAMEWORK
using NUnit.Framework;
using Tide.EditorTools;
namespace Tide.Tests
{
    public sealed class T0ContractTests
    {
        [Test]
        public void GeneratedT0ContractChecksPass()
        {
            foreach(var result in T0Verification.RunAll())
                Assert.That(result.Failure,Is.Null,result.Name);
        }
    }
}
#endif
