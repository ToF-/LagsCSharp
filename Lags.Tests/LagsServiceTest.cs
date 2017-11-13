using System;
using Xunit;
using Lags;

namespace Lags.Tests
{
    public class LagsServiceTest
    {
        private readonly LagsService _lagsService;

         public LagsServiceTest()
        {
            _lagsService = new LagsService();
        }

        [Fact]
        public void FakeTest()
        {

            Assert.False(true, "fake Test");

        }
    }
}
