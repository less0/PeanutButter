﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NExpect;
using NUnit.Framework;
using PeanutButter.RandomGenerators;
using PeanutButter.TestUtils.AspNetCore.Builders;
using static NExpect.AspNetCoreExpectations;
using static NExpect.Expectations;

namespace PeanutButter.TestUtils.AspNetCore.Tests;

[TestFixture]
public class TestActionExecutedContextBuilder
{
    [TestFixture]
    public class DefaultBuild
    {
        [Test]
        public void ShouldHaveEmptyHeaders()
        {
            // Arrange
            // Act
            var result = BuildDefault();
            // Assert
            Expect(result.HttpContext)
                .Not.To.Be.Null();
            Expect(result.HttpContext.Request)
                .Not.To.Be.Null();
            Expect(result.HttpContext.Request.Headers)
                .To.Be.Empty();
        }

        [Test]
        public void ShouldHaveEmptyFilters()
        {
            // Arrange
            // Act
            var result = BuildDefault();
            // Assert
            Expect(result.Filters)
                .To.Be.Empty();
        }

        [Test]
        public void ShouldHaveNullController()
        {
            // Arrange
            // Act
            var result = BuildDefault();
            // Assert
            Expect(result.Controller)
                .To.Be.Null();
        }

        private static ActionExecutedContext BuildDefault()
        {
            return ActionExecutedContextBuilder.BuildDefault();
        }
    }

    [Test]
    public void ShouldBeAbleToSetHeaders()
    {
        // Arrange
        var header1 = RandomValueGen.GetRandomString(10);
        var value1 = RandomValueGen.GetRandomString();
        var header2 = RandomValueGen.GetRandomString(10);
        var value2 = RandomValueGen.GetRandomString();
        // Act
        var result = ActionExecutedContextBuilder.Create()
            .WithHeader(header1, value1)
            .WithHeader(header2, value2)
            .Build();
        // Assert
        var headers = result.HttpContext.Request.Headers;
        Expect(headers)
            .To.Contain.Only(2).Items();
        Expect(headers)
            .To.Contain.Key(header1)
            .With.Value(value1);
        Expect(headers)
            .To.Contain.Key(header2)
            .With.Value(value2);
    }

    [Test]
    public void ShouldBeAbleToAddFilterMetadata()
    {
        // Arrange
        var expected = new SomeMeta();
        // Act
        var result = ActionExecutedContextBuilder.Create()
            .WithFilterMetadata(expected)
            .Build();
        // Assert
        Expect(result.Filters)
            .To.Contain.Only(1)
            .Item();
        Expect(result.Filters.Single())
            .To.Be(expected);
    }

    [Test]
    public void ShouldBeAbleToSetController()
    {
        // Arrange
        var expected = new SomeController();
        // Act
        var result = ActionExecutedContextBuilder.Create()
            .WithController(expected)
            .Build();
        // Assert
        Expect(result.Controller)
            .To.Be(expected);
    }

    public class SomeController : ControllerBase
    {
    }

    public class SomeMeta : IFilterMetadata
    {
    }
}