using System;
using System.Collections.Generic;
using AliceApi;

namespace MillionBoxes.Models
{
    public static class RequestToResponse
    {
        public static AliceResponse MakeResponse(AliceRequest aliceRequest, BoxesContext dataBase)
        {
            var user = dataBase.GetUserById(aliceRequest.session.session_id);

            var session = new SessionResponse
            {
                session_id = aliceRequest.session.session_id,
                message_id = aliceRequest.session.message_id,
                user_id = aliceRequest.session.user_id
            };

            var aliceResponse = new AliceResponse { session = session };
            aliceResponse.response.text = GetAnswer(aliceRequest, dataBase, user);
            aliceResponse.response.tts = aliceResponse.response.text;
            aliceResponse.response.end_session = false;

            return aliceResponse;
        }

        private static string GetAnswer(AliceRequest aliceRequest, BoxesContext dataBase, User user)
        {
            var requestMode = GetRequestMode(aliceRequest, user);
            switch (requestMode)
            {
                case RequestModes.Hello:
                    return TextResources.Hello;

                case RequestModes.Help:
                    return TextResources.Help;

                case RequestModes.OpenBox:
                    EntitiesConvertExtension.TryParseInt(aliceRequest.request, out var number);
                    dataBase.SetOpenedBoxNumber(user.UserId, number);
                    var message = dataBase.ReadFromBox(number);
                    return message.Length == 0 ? GetRandomString(TextResources.BoxIsEmpty)
                                               : $"{GetRandomString(TextResources.MessageInTheBox)}. {message}";

                case RequestModes.Repeat:
                    message = dataBase.ReadFromBox(user.OpenedBox);
                    return message.Length == 0 ? $"{TextResources.Repeat} {GetRandomString(TextResources.BoxIsEmpty)}"
                                               : $"{TextResources.Repeat} {GetRandomString(TextResources.MessageInTheBox)}. {message}";

                case RequestModes.SaveToBox:
                    dataBase.SetUserSaveMode(user.UserId, true);
                    return GetRandomString(TextResources.SaveToBox);

                case RequestModes.Dictate:
                    dataBase.SaveToBox(user.OpenedBox, aliceRequest.request.original_utterance);
                    dataBase.SetUserSaveMode(user.UserId, false);
                    return $"{GetRandomString(TextResources.MessageSaved)} {user.OpenedBox}";

                case RequestModes.Read:
                    message = dataBase.ReadFromBox(user.OpenedBox);
                    return message.Length == 0 ? GetRandomString(TextResources.BoxIsEmpty)
                                               : $"{GetRandomString(TextResources.MessageInTheBox)}. {message}";

                case RequestModes.DeleteMessage:
                    dataBase.SaveToBox(user.OpenedBox, string.Empty);
                    return GetRandomString(TextResources.DeleteMessage);

                case RequestModes.BoxIsNotOpen:
                    return TextResources.BoxIsNotOpen;

                case RequestModes.InvalidBoxNumber:
                    return GetRandomString(TextResources.WrongNumberOfBox);

                default:
                    return GetRandomString(TextResources.SomethingWrong);
            }
        }

        private static string GetRandomString(List<string> strings)
        {
            return strings[new Random().Next(strings.Count)];
        }

        private static RequestModes GetRequestMode(AliceRequest aliceRequest, User user)
        {
            var command = aliceRequest.request.command.ToLower();

            if (aliceRequest.session.New)
            {
                return RequestModes.Hello;
            }

            if (command == "справка" || command == "помощь")
            {
                return RequestModes.Help;
            }

            if (user.IsSaving)
            {
                return user.OpenedBox == 0 ? RequestModes.BoxIsNotOpen : RequestModes.Dictate;
            }

            if ((command.Contains("откр") || command.Contains("заглян") || command.Contains("посмотр"))
               && EntitiesConvertExtension.TryParseInt(aliceRequest.request, out var number))
            {
                return number > 0 && number <= 1000000 ? RequestModes.OpenBox : RequestModes.InvalidBoxNumber;
            }

            if (command.Contains("напи") || command.Contains("сохр") || command.Contains("полож") || command.Contains("остав"))
            {
                return user.OpenedBox != 0 ? RequestModes.SaveToBox : RequestModes.BoxIsNotOpen;
            }

            if (command.Contains("повтори") || command.Contains("ещё раз") || command.Contains("еще раз") ||
                command.Contains("снова"))
            {
                return user.OpenedBox != 0 ? RequestModes.Repeat : RequestModes.BoxIsNotOpen;
            }

            if (command.Contains("читай") || command.Contains("читать") || command.Contains("прочти"))
            {
                return user.OpenedBox != 0 ? RequestModes.Read : RequestModes.BoxIsNotOpen;
            }

            if (command.Contains("удали") || command.Contains("сотри") || command.Contains("стереть") ||
                command.Contains("отчист") || command.Contains("очист"))
            {
                return user.OpenedBox != 0 ? RequestModes.DeleteMessage : RequestModes.BoxIsNotOpen;
            }

            return RequestModes.SomethingWrong;
        }
    }
}
