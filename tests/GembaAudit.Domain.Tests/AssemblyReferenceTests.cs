using System.Reflection;

namespace GembaAudit.Domain.Tests;

public class AssemblyReferenceTests
{
    [Fact]
    public void DomainAssembly_ShouldLoad()
    {
        var assembly = Assembly.Load("GembaAudit.Domain");

        Assert.NotNull(assembly);
        Assert.Equal("GembaAudit.Domain", assembly.GetName().Name);
    }
}
