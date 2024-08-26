using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MultiplayerMod.Core.Reflection;
using NUnit.Framework;

namespace MultiplayerMod.Test.Core.Reflection;

[TestFixture]
public class DynamicMethodDelegateTests {

    private const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

    [Test]
    public void ReturnValueType() {
        var method = typeof(DynamicMethodDelegateTests).GetMethod(nameof(GetFive), flags)!;
        var dynamic = new DynamicMethodDelegate(this, method);
        var result = (int) dynamic.Invoke()!;
        Assert.That(result, Is.EqualTo(5));
    }

    [Test]
    public void ReturnObject() {
        var method = typeof(DynamicMethodDelegateTests).GetMethod(nameof(GetList), flags)!;
        var dynamic = new DynamicMethodDelegate(this, method);
        var result = (List<string>) dynamic.Invoke()!;
        Assert.That(result, Is.EqualTo(new[] { "one", "two", "three" }));
    }

    [Test]
    public void ArrayParameter() {
        var method = typeof(DynamicMethodDelegateTests).GetMethod(nameof(ArraySum), flags)!;
        var dynamic = new DynamicMethodDelegate(this, method);
        var result = (int) dynamic.Invoke(new [] { 1, 2, 3 })!;
        Assert.That(result, Is.EqualTo(6));
    }

    [Test]
    public void ObjectParameter() {
        var method = typeof(DynamicMethodDelegateTests).GetMethod(nameof(Concat), flags)!;
        var dynamic = new DynamicMethodDelegate(this, method);
        var result = (string) dynamic.Invoke("Foo", "Bar")!;
        Assert.That(result, Is.EqualTo("FooBar"));
    }

    [Test]
    public void ReferenceArgument() {
        var method = typeof(DynamicMethodDelegateTests).GetMethod(nameof(IncreaseByRef), flags)!;
        var dynamic = new DynamicMethodDelegate(this, method);
        var args = new object[] { 42 };
        var result = dynamic.Invoke(args);
        Assert.That(result, Is.EqualTo(42));
        Assert.That(args[0], Is.EqualTo(43));
    }

    private int GetFive() => 5;

    private int IncreaseByRef(ref int value) => value++;

    private List<string> GetList() => ["one", "two", "three"];

    private int ArraySum(int[] array) => array.Sum();

    private string Concat(string a, string b) => a + b;

}
