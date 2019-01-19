using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AliceApi.Modules
{
    public static class EntitiesConvert
    {
        public static bool TryParseInt(List<object> entities, out int result)
        {
            result = 0;
            var textValue = string.Empty;
            foreach (var n in entities)
            {
                textValue = $"{n}";
                if (textValue.Contains("YANDEX.NUMBER"))
                {
                    textValue = string.Join("", textValue.Substring(textValue.IndexOf("value", StringComparison.Ordinal)).Where(char.IsDigit));
                    break;
                }
            }

            if (int.TryParse(textValue, out int value))
            {
                result = value;
                return true;
            }

            return false;
        }
    }
}
