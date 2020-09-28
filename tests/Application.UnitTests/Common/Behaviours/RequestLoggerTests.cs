using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using PayMeWithRocks.Application.Common.Behaviours;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.UnitTests.Common.Behaviours
{
    public class RequestLoggerTests
    {
        private readonly Mock<ILogger<GetMerchantUsersQuery>> _logger;
        private readonly Mock<ICurrentUserService> _currentUserService;

        public RequestLoggerTests()
        {
            _logger = new Mock<ILogger<GetMerchantUsersQuery>>();

            _currentUserService = new Mock<ICurrentUserService>();
        }

        [Test]
        public async Task ShouldCallGetUserNameAsyncOnceIfAuthenticated()
        {
            _currentUserService.Setup(x => x.UserId).Returns("Administrator");

            var requestLogger = new LoggingBehaviour<GetMerchantUsersQuery>(_logger.Object, _currentUserService.Object);

            await requestLogger.Process(new GetMerchantUsersQuery(), new CancellationToken());
        }

        [Test]
        public async Task ShouldNotCallGetUserNameAsyncOnceIfUnauthenticated()
        {
            var requestLogger = new LoggingBehaviour<GetMerchantUsersQuery>(_logger.Object, _currentUserService.Object);

            await requestLogger.Process(new GetMerchantUsersQuery(), new CancellationToken());
        }
    }
}