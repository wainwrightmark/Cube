using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Xunit;

namespace CombinationPuzzle.Test
{
    public static class TestExtensions
    {



        public static async Task ShouldSucceed(this Task<Result> resultTask)
        {
            var r = await resultTask;
            r.ShouldBeSuccessful();
        }

        public static async Task ShouldSucceed<T>(this Task<Result<T>> resultTask)
        {
            var r = await resultTask;
            r.ShouldBeSuccessful();
        }


        public static void ShouldBe<T>(this Result<T> result, Result<T> expectedResult)
        {
            if (expectedResult.IsSuccess)
            {
                result.ShouldBeSuccessful();
                result.Value.Should().Be(expectedResult.Value);
            }
            else
            {
                result.ShouldBeFailure();
                result.Error.Should().Be(expectedResult.Error);
            }
        }

        public static void ShouldBeSuccessful(this Result result)
        {
            var (_, isFailure, error) = result;
            Assert.False(isFailure, error);
        }


        public static void ShouldBeSuccessful<T>(this Result<T> result)
        {
            var (_, isFailure, _, error) = result;
            Assert.False(isFailure, error);
        }


        public static void ShouldBeFailure(this Result result)
        {
            var (_, isFailure, _) = result;
            Assert.True(isFailure);
        }


        public static void ShouldBeFailure<T>(this Result<T> result)
        {
            var (_, isFailure, _, _) = result;
            Assert.True(isFailure);
        }


    }
}
