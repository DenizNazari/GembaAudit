using System.Reflection;

namespace GembaAudit.Application.Tests;

public class AssemblyReferenceTests
{
    [Theory]
    [InlineData("GembaAudit.Application")]
    [InlineData("GembaAudit.Domain")]
    public void ReferencedAssemblies_ShouldLoad(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);

        Assert.NotNull(assembly);
        Assert.Equal(assemblyName, assembly.GetName().Name);
    }
}
