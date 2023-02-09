
[![Build Source Code](https://github.com/sang-hyeon/PlasticCommand/actions/workflows/continous-integration.yaml/badge.svg)](https://github.com/sang-hyeon/PlasticCommand/actions/workflows/continous-integration.yaml)
[![Nuget](https://img.shields.io/nuget/v/PlasticCommand)](https://www.nuget.org/packages/PlasticCommand/)

<br>

# Abstract
This project provides encapsulation of things like Domain, Application Rules, Business Rules or Business Logic in Application. For this, Command pattern is used.

All applications such as Web, CLI, GUI application can use this project.
This can be part of the Usecase Layer, Domain Service Layer or CQRS.

The source generator introduced in .Net 5 is used to implement this Idea. If metaprogramming such as Source generator is properly used, it's possible to provide flexible source code that has not been provided by traditional programming.
Generated source code has the same effect as the source code you wrote yourself because it will be injected at compile time.

The name of this project is Plastic Command.

[Blog post](https://medium.com/@Thwj/heres-a-new-proposal-to-encapsulate-domain-layer-5940dc6c738) <br>
[Blog post(한국어)](https://medium.com/@Thwj/%EC%83%88%EB%A1%9C%EC%9A%B4-domain-layer%EC%9D%98-%EC%BA%A1%EC%8A%90%ED%99%94-5661a3240184)

<br>

# Flow of the plastic command
![Platstic의 명령 흐름](docs/resources/flow.jpg)

<br>
# Plastic Command
## Quick Start

Step 1. Specify The Command
```cs
// [CommandName("AddCommand")]
class AddCommandSpec : ICommandSpecification<int, int>
{
        public AddCommandSpec(IMyCalculator calculator)
        { 
            ...
        }

        public Task<int> ExecuteAsync(int param, CancellationToken token = default)
        {
            ...
        }
}
```

Step 2. Add Plastic to IServiceCollection
```cs
void Configure(IServiceCollection services)
{
        var pipelineBuilder = new BuildPipeline(...);

        services.UsePlastic(pipelineBuilder);
}
```

Step 3. Use a Generated Command
```cs
class AddController : ControllerBase
{
        public AddController(AddCommand addCommand)
        {
                ...
                ...
                int result = addCommand.ExecuteAsync( 1 );
        }
}
```
