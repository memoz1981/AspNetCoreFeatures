using Microsoft.AspNetCore.Mvc.Testing;

namespace MiddlewareTests;

public class AspWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class {}
