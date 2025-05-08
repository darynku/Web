using System.ComponentModel;

namespace Web.Endpoints.Requests;

public record TestAdminLoginRequest(string Username = "admin");