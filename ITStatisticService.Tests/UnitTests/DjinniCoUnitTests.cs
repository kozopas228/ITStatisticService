using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ITStatisticService.Logic.Domain;
using ITStatisticService.Logic.Implementation.DjinniCO;
using Xunit;
using Moq;
using Moq.Protected;

namespace ITStatisticService.Tests.UnitTests
{
    public class DjinniCoTests
    {
        [Fact]
        public async Task DjinniCoParser_Parse_ShouldInvokeHttpClientWithCorrectUrl()
        {
            //Arrange
            var handlerMock = SetupHttpMessageHandlerMock("content");
            var httpClient = new HttpClient(handlerMock.Object);
            var fakeParserSettings = SetupParserSettings();
            var parser = new DjinniCoParser(httpClient, fakeParserSettings.Object);

            //Act
            await parser.Parse(Technologies.Java);
            
            //Assert
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => req.RequestUri.ToString() == "https://base/certain"),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DjinniCoParser_Parse_ShouldParseSalaryWithoutDash()
        {
            //Arrange
            string document = "<div class=\"public-salary-item\">" +
                              "$1000" +
                              "</div>";
            var handlerMock = SetupHttpMessageHandlerMock(document);
            var httpClient = new HttpClient(handlerMock.Object);
            var fakeParserSettings = SetupParserSettings();
            var parser = new DjinniCoParser(httpClient, fakeParserSettings.Object);

            //Act
            var result = await parser.Parse(Technologies.Java);
            
            //Assert
            Assert.True(result.First().Salary == 1000);
        }
        
        [Fact]
        public async Task DjinniCoParser_Parse_ShouldParseSalaryWithDash()
        {
            //Arrange
            string document = "<div class=\"public-salary-item\">" +
                              "$1000-2000" +
                              "</div>";
            var handlerMock = SetupHttpMessageHandlerMock(document);
            var httpClient = new HttpClient(handlerMock.Object);
            var fakeParserSettings = SetupParserSettings();
            var parser = new DjinniCoParser(httpClient, fakeParserSettings.Object);

            //Act
            var result = await parser.Parse(Technologies.Java);
            
            //Assert
            Assert.True(result.First().Salary == 1500);
        }

        private Mock<IDjinniCoParserSettings> SetupParserSettings()
        {
            var fakeParserSettings = new Mock<IDjinniCoParserSettings>();
            fakeParserSettings.Setup(x => x.BaseUrl).Returns("https://base");
            fakeParserSettings.Setup(x => x.CertainUrl).Returns("/certain");
            fakeParserSettings.Setup(x => x.PagesAmount).Returns(1);
            return fakeParserSettings;
        }
        
        private Mock<HttpMessageHandler> SetupHttpMessageHandlerMock(string content)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(content)
            };
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            return handlerMock;
        }
    }
}