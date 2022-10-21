using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Company.Framework.Core.Identity;
using Company.Framework.Core.Identity.Factory;
using static Company.Framework.Core.Identity.IdGenerationType;

namespace Company.Framework.Benchmark
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CoreIdBenchmarks
    {
        private static readonly Guid FakeIdValue = Guid.NewGuid();

        [Benchmark]
        public void EmptyConstructor()
        {
            new FakeId();
        }

        [Benchmark]
        public void ValueConstructor()
        {
            new FakeId(FakeIdValue);
        }

        [Benchmark]
        public void AutoGenerationConstructor()
        {
            new FakeId(Auto);
        }

        [Benchmark]
        public void NoneGenerationConstructor()
        {
            new FakeId(None);
        }

        [Benchmark]
        public void NewMethod()
        {
            FakeId.New();
        }

        [Benchmark]
        public void EmptyStatic()
        {
            var id = FakeId.Empty;
        }


        [Benchmark]
        public void FromMethod()
        {
            FakeId.From(FakeIdValue);
        }

        [Benchmark]
        public void ValueTypedCoreIdFactory()
        {
            CoreIdFactory<FakeId, Guid>.Instance(FakeIdValue);
        }

        [Benchmark]
        public void AutoGenerationTypedCoreIdFactory()
        {
            CoreIdFactory<FakeId>.Instance(Auto);
        }

        [Benchmark]
        public void NoneGenerationTypedCoreIdFactory()
        {
            CoreIdFactory<FakeId>.Instance(None);
        }

        [Benchmark]
        public void EmptyActivator()
        {
            Activator.CreateInstance(typeof(FakeId));
        }

        [Benchmark]
        public void ValueActivator()
        {
            Activator.CreateInstance(typeof(FakeId), FakeIdValue);
        }

        [Benchmark]
        public void AutoGenerationActivator()
        {
            Activator.CreateInstance(typeof(FakeId), Auto);
        }

        [Benchmark]
        public void NoneGenerationActivator()
        {
            Activator.CreateInstance(typeof(FakeId), None);
        }
    }

    public class FakeId : CoreId<FakeId, Guid>
    {
        public FakeId()
        {

        }
        public FakeId(Guid value) : base(value)
        {
            Value = value;
        }

        public FakeId(IdGenerationType generationType) : base(generationType)
        {
        }
    }
}
