using System.ComponentModel;

namespace Web.Endpoints.Requests;

public record TestRequests(string Username = "admin");

public record TestUserLoginRequest(string Username = "string");