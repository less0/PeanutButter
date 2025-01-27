using NExpect;
using NUnit.Framework;
using PeanutButter.TestUtils.AspNetCore.Fakes;
using static NExpect.Expectations;

namespace PeanutButter.TestUtils.AspNetCore.Tests;

[TestFixture]
public class TestMinimalServiceProvider
{
    [Test]
    public void ShouldResolveFromFactory()
    {
        // Arrange
        var sut = Create();   
        // Act
        sut.Register<IService>(() => new Service());
        var result = sut.Resolve<IService>();
        // Assert
        Expect(result)
            .To.Be.An.Instance.Of<Service>();
    }

    [Test]
    public void ShouldResolveFromTypeMapWithNoConstructorParams()
    {
        // Arrange
        var sut = Create();
        // Act
        sut.Register<IService, Service>();
        var result = sut.Resolve<IService>();
        // Assert
        Expect(result)
            .To.Be.An.Instance.Of<Service>();
    }

    [Test]
    public void ShouldResolveConcreteTypeWithoutRegistration()
    {
        // Arrange
        var sut = Create();
        // Act
        var result = sut.Resolve<Service>();
        // Assert
        Expect(result)
            .To.Be.An.Instance.Of<Service>();
    }

    [Test]
    public void ShouldResolveSingleImplementationWithoutRegistration()
    {
        // Arrange
        var sut = Create();
        // Act
        var result = sut.Resolve<IService>();
        // Assert
        Expect(result)
            .To.Be.An.Instance.Of<Service>();
    }

    public class Service : IService
    {
    }

    public interface IService
    {
    }

    [Test]
    public void ShouldResolveSimpleConcreteType()
    {
        // Arrange
        var sut = Create();
        // Act
        // Assert
    }

    private static MinimalServiceProvider Create()
    {
        return new();
    }
}