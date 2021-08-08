namespace Plastic.UnitTests.Generator.TestSet
{
    using System.Threading;
    using System.Threading.Tasks;

    public static class CommandSpec_InClass
    {
        public class TestCommandSpec : CommandSpecificationBase
        {
            public override Task<Response> CanExecuteAsync(NoParameters param, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }

            public override Task<ExecutionResult> ExecuteAsync(NoParameters param, CancellationToken token = default)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
