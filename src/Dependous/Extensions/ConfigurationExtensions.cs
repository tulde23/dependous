using System;
using System.Linq;

namespace Dependous.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static (Type attributeType, object mapper, bool enabled) GetAttributeScanningConfiguration(this IDependousConfiguration dependousConfiguration)
        {
            var attributes = dependousConfiguration.AdditionalTypes.FirstOrDefault(x => x.GetType().IsGenericType);
            if (attributes != null)
            {
                var mapper = attributes.GetType().GetProperty("AttributeMapper").GetValue(attributes);
                return (attributes.GetType().GetGenericArguments()[0], mapper, true);
            }
            return (null, null, false);
        }
    }
}