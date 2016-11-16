using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace TeamWorkNet.Base
{
  public class NullToEmptyStringResolver : DefaultContractResolver
  {
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
      return type.GetRuntimeProperties()
              .Select(p =>
              {
                var jp = base.CreateProperty(p, memberSerialization);
                jp.ValueProvider = new NullToEmptyStringValueProvider(p);
                return jp;
              }).ToList();
    }
  }

  public class NullToEmptyStringValueProvider : IValueProvider
  {
    PropertyInfo _MemberInfo;
    public NullToEmptyStringValueProvider(PropertyInfo memberInfo)
    {
      _MemberInfo = memberInfo;
    }

    public object GetValue(object target)
    {
      object result = _MemberInfo.GetValue(target);
      if (_MemberInfo.PropertyType == typeof(string) && result == null) result = "";
      return result;

    }

    public void SetValue(object target, object value)
    {
      _MemberInfo.SetValue(target, value);
    }
  }
}
