using System.ComponentModel;

namespace Web.Endpoints.Requests;

public record TestUserLoginRequest(string Username = "string");