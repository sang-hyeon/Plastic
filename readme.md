
[![Build Main](https://github.com/sang-hyeon/Plastic/actions/workflows/github_actions.yml/badge.svg?branch=main)](https://github.com/sang-hyeon/Plastic/actions/workflows/github_actions.yml)
[![Nuget](https://img.shields.io/nuget/v/Plastic)](https://www.nuget.org/packages/Plastic/)

# Abstract
이 프로젝트는 Application에서 Domain, Application Rules, Business Rules 혹은 Business Logic과 같은것들의 명령 단위 캡슐화를 지원합니다. 명령에 대한 표현은 Command pattern이 사용됩니다.

이것은 Usecase Layer, Domain Service Layer 그리고 CQRS 구축의 일부분이 될 수 도 있습니다.

Web, CLI, GUI Application 등 어느 하나에 중점을 두지 않고 범용적으로 사용할 수 있습니다.

구현부에서는 .Net 5에서 소개된 Source Generator를 활용합니다.
Source Generator와 같은 메타프로그래밍을 적절히 활용한다면 정통 프로그래밍에서 제공할 수 없었던 유연한 소스 코드를 제공할 수 있습니다.
이것은 컴파일 시점에 소스코드를 생성하여 주입하기 때문에 여러분이 직접 작성한 소스 코드와 동일한 효과를 갖습니다.

이 프로젝트의 이름은 Plastic 입니다.

# Quick Start
```cs
// [CommandName("AddCommand")]
class AddCommandSpec : CommandSpecificationBase<AddParam, AddResponse>
{
        public AddCommandSpec(IMyCalculator calculator)
        { ... }

        public override Task<AddResponse> ExecuteAsync(
                AddParam param, CancellationToken token = default)
        { ... }
        
        public override Task<Response> CanExecuteAsync(
                AddParam param, CancellationToken token = default)
        { return CanBeExecuted(); }

}
// ------
void Configure(IServiceCollection services)
{
        var pipelineBuilder = new BuildPipeline(...);

        services.UsePlastic(pipelineBuilder);
}
// ------
class AddController : ControllerBase
{
        public AddController(AddCommand addCommand)
        {
                ...
                addCommand.Execute( ... );
        }
}

```


# Architecture
Plastic은 흔히 알려진 몇가지 주요 개념들을 중점으로 디자인되었습니다.

* 명령 패턴 (Commad Pattern)
* 비상태 저장 (Stateless)
* 파이프라인 (Pipeline)
* 장기 실행 서비스 (Long-running Service)
* 안전성 (Safety)
* 강한 결합 (Coupling)
* 명시적 종속성 (Explicit dependencies)

<br>

아래는 명령의 흐름을 설명합니다.
![Platstic의 명령 흐름](resources/command-flow.jpg)

## 명령 패턴 (Command Pattern)
Command Pattern은 개체 자체가 명령이라는 단순한 디자인 패턴입니다.

일반적으로 Application은 사용자로부터 명령을 받고 그것을 처리합니다.
때문에 이러한 Application의 Usecase는 Command Pattern으로 표현하기에 매우 적절합니다.

Plastic에서 이 Command들은 사용자가 직접적으로 다룰 수 있게 제일 앞단에 배치됩니다.
구현에서는 아래와 같은 소스코드가 이를 담당합니다.

```cs
public interface ICommandSpecification<in TParam, TResult>
        where TParam : CommandParameters
        where TResult : ExecutionResult
{
   Task<TResult> ExecuteAsync(TParam param, CancellationToken token = default);
   
   Task<Response> CanExecuteAsync(TParam param, CancellationToken token = default);
}
```

Plastic은 명세된 `ICommandSpecification<,>` 의 구현체를 바탕으로 사용가능한 Command를 생성합니다.
> 생성된 Command에는 Pipeline을 연결하는 코드들이 포함됩니다.

그러나 이 Interface는 최상위 추상체이며 이를 바탕으로 구현하는것은 불가능 할 것입니다.
작성자는 아래와 같은 추상체를 사용해야 합니다.

또한 명령의 다양한 시나리오를 지원합니다. <br>

```cs
    public abstract class CommandSpecificationBase<TParam, TResult>
    { ... }

    public abstract class CommandSpecificationBase<TResult>
    { ... }

    public abstract class CommandSpecificationBase
    { ... }
```


## 비상태 저장 (Stateless)
모든 Command들은 Stateful 을 지원하지 않습니다. <br>
일회성으로 수행되는 Command가 상태를 저장하는건 Business Transaction을 보장하는것에 유용하지 못합니다.

즉 흔히 Web에서 볼 수 있는 Seesion과 같은 도구를 지원하지 않으며 장려하지 않습니다.

## 파이프라인 (Pipeline)
모든 Command들은 특정한 전역 파이프라인 (Global Pipeline)을 거친 후 실행됩니다. <br>
이 파이프라인을 구성함으로써 보다 나은 기능성을 기대할 수 있습니다.

Logging, Business transaction, Integration event와 같은 시나리오들을 이 파이프라인에서 프로세싱할 수 있습니다.

## 장기 실행 서비스 (Long-running Service)
몇몇 Application들은 특정 Domain 개체의 상태를 변경하는것으로 끝나지 않을 수 있습니다.

특히 Mobile이나 Desktop Application들이 그렇습니다.

또한 Machine learning을 활용하는 Application에서도 볼 수 있습니다. <br>
Machine learning의 학습 과정은 Long-running 입니다.

## 안전성 (Safety)
Platsic은 매우 높은 자유도를 제공하지 않습니다. <br>
계획된 컨셉에 어긋나는 코드는 오히려 시스템에 악영향을 미칠 수 있습니다.

예로 Command에서는 다른 Command들을 사용할 수 없습니다.
물론 몇몇은 이미 작성한 Command를 재사용하고 싶을 것입니다.
하지만 대개 이런 경우는 Layer를 더 복잡하게 만듭니다.

Command들 안에서 중복 코드가 발생할 수 있습니다. 허나 Plastic은 그것을 거짓 중복으로 취급합니다.

## 강한 결합 (Coupling)
Command들은 모두 구현체 그대로 노출 됩니다. <br>
테스트, 다른 구현체 사용 등등 과 같은 명목으로 적용되는 낮은 결합(Low Coupling)은 Plastic에서 배제됩니다.
 
Domain Service를 구축하는 사례를 보면 대개 추상화를 통한 낮은 결합 (Low Coupling)을 이루려고 합니다.
허나 Plastic에서 Command들은 이것의 바깥 Layer인 Consumer측에 강하게 결합되기를 희망합니다.

대개 바깥 Layer는 오직 이 layer를 사용하기 위해 만들어진 영역입니다. 약하게 결합될 이유가 없습니다.
이 말은 이러한 Layer 없이 존재할 이유가 없다라는 것과 같습니다.

Plastic은 바깥 Layer가 명령의 변경사항에 민감하게 반응하기를 희망합니다.

## 명시적 종속성 
다양하게 수많은 Command들을 손쉽게 다루기 위한 방법으로 Mediator 혹은 Service Locator와 같은 방법을 생각해낼 수 있습니다.
그러나 이는 개체의 명시적 종속을 위반합니다.

등가 교환으로 이 명시적 종속성 위반을 품에 안을 수 있습니다.
하지만 Plastic은 이 명시적 종속성을 지키고 싶습니다.

이를 위해 .Net 5에서 소개된 Source Generator 를 활용합니다.
이를 통해 사용성과 명시적 종속성, 이 두가지를 모두 제공합니다.

# 이름에 대하여...
일상 생활에서 Plastic은 대부분 무언가를 보호하기 위해 사용되는 깨지기 쉬운 재질입니다.
프로젝트 Plastic은 소비측의 안쪽 Layer를 보호하기 위해 만들어졌기에 이렇게 작명되었습니다.


# 마치며
이 프로젝트는 아주 새로운 것이 아닙니다.
지금까지 흔하게 접해온 개념들을 잘 조합하는 것이 목표입니다.
