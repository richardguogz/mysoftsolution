#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Utilities;
using System.Collections;

namespace Newtonsoft.Json.Serialization
{
  /// <summary>
  /// Contract details for a <see cref="Type"/> used by the <see cref="JsonSerializer"/>.
  /// </summary>
  public class JsonArrayContract : JsonContract
  {
    internal Type CollectionItemType { get; private set; }

    private readonly bool _isCollectionItemTypeNullableType;
    private readonly Type _genericCollectionDefinitionType;
    private Type _genericWrapperType;
    private MethodCall<object, object> _genericWrapperCreator;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonArrayContract"/> class.
    /// </summary>
    /// <param name="underlyingType">The underlying type for the contract.</param>
    public JsonArrayContract(Type underlyingType)
      : base(underlyingType)
    {
      if (ReflectionUtils.ImplementsGenericDefinition(underlyingType, typeof(ICollection<>), out _genericCollectionDefinitionType))
      {
        CollectionItemType = _genericCollectionDefinitionType.GetGenericArguments()[0];
      }
      else
      {
        CollectionItemType = ReflectionUtils.GetCollectionItemType(UnderlyingType);
      }

      if (CollectionItemType != null)
        _isCollectionItemTypeNullableType = ReflectionUtils.IsNullableType(CollectionItemType);

      if (IsTypeGenericCollectionInterface(UnderlyingType))
      {
        CreatedType = ReflectionUtils.MakeGenericType(typeof(List<>), CollectionItemType);
      }
    }

    internal IWrappedCollection CreateWrapper(object list)
    {
      if ((list is IList && (CollectionItemType == null || !_isCollectionItemTypeNullableType))
        || UnderlyingType.IsArray)
        return new CollectionWrapper<object>((IList)list);

      if (_genericCollectionDefinitionType != null)
      {
        EnsureGenericWrapperCreator();
        return (IWrappedCollection) _genericWrapperCreator(null, list);
      }
      else
      {
        IList values = ((IEnumerable) list).Cast<object>().ToList();

        if (CollectionItemType != null)
        {
          Array array = Array.CreateInstance(CollectionItemType, values.Count);
          for (int i = 0; i < values.Count; i++)
          {
            array.SetValue(values[i], i);
          }

          values = array;
        }

        return new CollectionWrapper<object>(values);
      }
    }

    private void EnsureGenericWrapperCreator()
    {
      if (_genericWrapperType == null)
      {
        _genericWrapperType = ReflectionUtils.MakeGenericType(typeof (CollectionWrapper<>), CollectionItemType);

        Type constructorArgument = (ReflectionUtils.InheritsGenericDefinition(_genericCollectionDefinitionType, typeof (List<>)))
                                     ? ReflectionUtils.MakeGenericType(typeof (ICollection<>), CollectionItemType)
                                     : _genericCollectionDefinitionType;

        ConstructorInfo genericWrapperConstructor = _genericWrapperType.GetConstructor(new[] { constructorArgument });
        _genericWrapperCreator = JsonTypeReflector.ReflectionDelegateFactory.CreateMethodCall<object>(genericWrapperConstructor);
      }
    }

    private bool IsTypeGenericCollectionInterface(Type type)
    {
      if (!type.IsGenericType)
        return false;

      Type genericDefinition = type.GetGenericTypeDefinition();

      return (genericDefinition == typeof(IList<>)
              || genericDefinition == typeof(ICollection<>)
              || genericDefinition == typeof(IEnumerable<>));
    }
  }
}