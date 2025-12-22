using bdtool.Commands.Tools;

namespace bdtool.tests;
using bdtool;

public class VDBCommandTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        using var sw = new StringWriter();
        Console.SetOut(sw);
        
        var root = Program.BuildRootCommand();
        var result = root.Parse("vdb read \"../data/vdb/VDB_ps2_bo3_proto.XML\" ").Invoke();
        Assert.That(result, Is.EqualTo(0));
        TestContext.Out.WriteLine("HELLO???");
    }
}