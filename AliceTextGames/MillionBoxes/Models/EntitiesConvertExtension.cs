using System;
using System.Collections.Generic;
using System.Linq;

namespace MillionBoxes.Models
{
    public static class EntitiesConvertExtension
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

            //TODO Исправить распознавание миллиона
            foreach (var n in entities)
            {
                textValue = $"{n}";
                if (textValue.Contains("миллион"))
                {
                    result = 1000000;
                    return true;
                }
            }

            return false;
        }
    }
}
