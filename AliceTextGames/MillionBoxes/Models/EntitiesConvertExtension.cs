using System;
using System.Linq;
using AliceApi;

namespace MillionBoxes.Models
{
    public static class EntitiesConvertExtension
    {
        public static bool TryParseInt(Request request, out int result)
        {
            var entities = request.nlu.entities;
            result = 0;
            var textValue = string.Empty;
            foreach (var entity in entities)
            {
                textValue = $"{entity}";
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

            if (request.command.ToLower().Contains("миллион"))
            {
                result = 1000000;
                return true;
            }

            return false;
        }
    }
}
