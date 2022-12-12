
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlasticCommand.UnitTests.TestCommands.Helpers;

namespace PlasticCommand.UnitTests.TestCommands;

public class ServiceCommandSpec : ICommandSpecification<int, IEnumerable<object>>
{
    public ITestService TestService { get; }
    public ITest2Service Test2Service { get; }

    public ServiceCommandSpec(ITestService testService, ITest2Service test2Service)
    {
        this.TestService = testService;
        this.Test2Service = test2Service;
    }

    public Task<IEnumerable<object>> ExecuteAsync(int param, CancellationToken token = default)
    {
        return Task.FromResult(new object[]
        {
            this.TestService,
            this.Test2Service
        }.ToList().AsEnumerable());
    }
}
