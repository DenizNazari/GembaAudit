using System.Reflection;

namespace GembaAudit.Api.IntegrationTests;

public class AssemblyReferenceTests
{
    [Theory]
    [InlineData("GembaAudit.Api")]
    [InlineData("GembaAudit.Application")]
    [InlineData("GembaAudit.Infrastructure")]
    [InlineData("GembaAudit.Contracts")]
    public void ReferencedAssemblies_ShouldLoad(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);

        Assert.NotNull(assembly);
        Assert.Equal(assemblyName, assembly.GetName().Name);
    }
}
