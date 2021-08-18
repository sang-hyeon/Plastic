
[![Build Main](https://github.com/sang-hyeon/Plastic/actions/workflows/github_actions.yml/badge.svg?branch=main)](https://github.com/sang-hyeon/Plastic/actions/workflows/github_actions.yml)
[![Nuget](https://img.shields.io/nuget/v/Plastic)](https://www.nuget.org/packages/Plastic/)

[한국어](https://github.com/sang-hyeon/Plastic/tree/main/docs/kor)

<br>

# Abstract
This project provides encapsulation of things like Domain, Application Rules, Business Rules or Business Logic in Application. For this, Command pattern is used.

All applications such as Web, CLI, GUI application can use this project.
This can be part of the Usecase Layer, Domain Service Layer or CQRS.

The source generator introduced in .Net 5 is used to implement this Idea. If metaprogramming such as Source generator is properly used, it's possible to provide flexible source code that has not been provided by traditional programming.
Generated source code has the same effect as the source code you wrote yourself because it will be injected at compile time.

The name of this project is Plastic.

<br>

# Flow of the command
![Platstic의 명령 흐름](docs/resources/flow.jpg)

<br>

# Quick Start

Step 1. Specify The Command
```cs
// [CommandName("AddCommand")]
class AddCommandSpec : CommandSpecificationBase<int, int>
{
        public AddCommandSpec(IMyCalculator calculator)
        { 
            ...
        }

        public override Task<ExecutionResult<int>> ExecuteAsync(int param, CancellationToken token = default)
        {
            ...
        }
        
        public override Task<Response> CanExecuteAsync(int param, CancellationToken token = default)
        {
            return CanBeExecuted();
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

Step 3. Use a generated command
```cs
class AddController : ControllerBase
{
        public AddController(AddCommand addCommand)
        {
                ...
                var result = addCommand.Execute( 1 );
        }
}
```
