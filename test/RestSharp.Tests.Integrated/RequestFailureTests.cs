using System.Net;
using RestSharp.Tests.Integrated.Fixtures;

namespace RestSharp.Tests.Integrated;

[Collection(nameof(TestServerCollection))]
public class RequestFailureTests {
    readonly RestClient        _client;
    readonly TestServerFixture _fixture;

    public RequestFailureTests(TestServerFixture fixture) {
        _client  = new RestClient(fixture.Server.Url);
        _fixture = fixture;
    }

    [Fact]
    public async Task Handles_GET_Request_Errors() {
        var request  = new RestRequest("status?code=404");
        var response = await _client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Handles_GET_Request_Errors_With_Response_Type() {
        var request  = new RestRequest("status?code=404");
        var response = await _client.ExecuteAsync<Response>(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Data.Should().Be(null);
    }

    [Fact]
    public async Task Throws_on_unsuccessful_call() {
        var client  = new RestClient(new RestClientOptions(_fixture.Server.Url) { ThrowOnAnyError = true });
        var request = new RestRequest("status?code=404");

        var task = () => client.ExecuteAsync<Response>(request);
        await task.Should().ThrowExactlyAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetAsync_throws_on_unsuccessful_call() {
        var request = new RestRequest("status?code=404");

        var task = () => _client.GetAsync(request);
        await task.Should().ThrowExactlyAsync<HttpRequestException>();
    }

    [Fact]
    public async Task GetAsync_generic_throws_on_unsuccessful_call() {
        var request = new RestRequest("status?code=404");

        var task = () => _client.GetAsync<Response>(request);
        await task.Should().ThrowExactlyAsync<HttpRequestException>();
    }

    class Response {
        public string Message { get; set; }
    }
}