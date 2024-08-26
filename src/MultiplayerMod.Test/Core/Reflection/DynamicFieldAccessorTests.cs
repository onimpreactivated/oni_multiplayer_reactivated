using System.Reflection;
using MultiplayerMod.Core.Reflection;
using NUnit.Framework;

namespace MultiplayerMod.Test.Core.Reflection;

[TestFixture]
public class DynamicFieldAccessorTests {

    private const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

    private int intValue = 1;
    private string stringValue = "one";

    [Test]
    public void GetValueTypeField() {
        var field = typeof(DynamicFieldAccessorTests).GetField(nameof(intValue), flags)!;
        var accessor = new DynamicFieldAccessor<int>(this, field);
        Assert.That(accessor.Get(), Is.EqualTo(1));
    }

    [Test]
    public void SetValueTypeField() {
        var field = typeof(DynamicFieldAccessorTests).GetField(nameof(intValue), flags)!;
        var accessor = new DynamicFieldAccessor<int>(this, field);
        accessor.Set(2);
        Assert.That(intValue, Is.EqualTo(2));
    }

    [Test]
    public void GetObjectField() {
        var field = typeof(DynamicFieldAccessorTests).GetField(nameof(stringValue), flags)!;
        var accessor = new DynamicFieldAccessor<string>(this, field);
        Assert.That(accessor.Get(), Is.EqualTo("one"));
    }

    [Test]
    public void SetObjectField() {
        var field = typeof(DynamicFieldAccessorTests).GetField(nameof(stringValue), flags)!;
        var accessor = new DynamicFieldAccessor<string>(this, field);
        accessor.Set("two");
        Assert.That(stringValue, Is.EqualTo("two"));
    }

}
