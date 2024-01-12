
using System;

namespace QuickJob.Client.Models;

internal static class ClientPaths
{
    private const string Delimiter = "/";
    
    public const string Orders = "orders";
    
    public static string Order(Guid id) => Orders + Delimiter + id;
}