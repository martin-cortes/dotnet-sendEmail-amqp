using System.ComponentModel;
using System.Reflection;

namespace Application.Common.Extensions.Enumerator
{
    public static class EnumeratorExtensions
    {
        public static string GetDescription(this Enum data)
        {
            FieldInfo field = data
                .GetType()
                .GetField(data.ToString());

            DescriptionAttribute[] desAttribute = field?
                .GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (desAttribute?.Length > 0)
            {
                return desAttribute[0].Description;
            }

            return string.Empty;
        }
    }
}
