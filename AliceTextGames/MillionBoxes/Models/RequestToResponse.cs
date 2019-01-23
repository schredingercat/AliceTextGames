﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliceApi;
using AliceApi.Modules;

namespace MillionBoxes.Models
{
    public static class RequestToResponse
    {
        public static AliceApi.AliceResponse MakeResponse(AliceRequest aliceRequest, BoxesContext dataBase)
        {
            

            var user = dataBase.GetUserById(aliceRequest.session.session_id);

            var session = new SessionResponse()
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
                    return "Привет!";
                case RequestModes.Help:
                    return "Вот тебе справка";
                case RequestModes.OpenBox:
                    EntitiesConvert.TryParseInt(aliceRequest.request.nlu.entities, out var number);
                    dataBase.SetOpenedBoxNumber(user.UserId, number);
                    var message = dataBase.ReadFromBox(number);
                    return message.Length==0?"В коробке ничего нет":$"В коробке оставлено сообщение. {message}";
                case RequestModes.Repeat:
                    message = dataBase.ReadFromBox(user.OpenedBox);
                    return message.Length == 0 ? "В коробке ничего нет. Если хотите оставить здесь записку, скажите 'оставь записку'" : $"В коробке оставлено сообщение. {message}";
                case RequestModes.SaveToBox:
                    dataBase.SetUserSaveMode(user.UserId, true);
                    return "Диктуйте сообщение";
                case RequestModes.Dictate:
                    dataBase.SaveToBox(user.OpenedBox, aliceRequest.request.original_utterance);
                    dataBase.SetUserSaveMode(user.UserId, false);
                    return $"Сообщение оставлено в коробке номер {user.OpenedBox}";
                case RequestModes.DeleteMessage:
                    dataBase.SaveToBox(user.OpenedBox, string.Empty);
                    return $"Содержимое коробки номер {user.OpenedBox} удалено";
                case RequestModes.BoxIsNotOpen:
                    return "Для начала давайте откроем коробку. Просто скажите 'открой коробку номер такой-то'";
                case RequestModes.InvalidBoxNumber:
                    return "Коробки с таким номером нет. Коробок ровно миллион, не больше ни меньше";
                default:
                    return "Кажется что-то пошло не так. Для начала давайте откроем коробку. Просто скажите 'открой коробку номер такой-то'. " +
                           "Либо скажите 'Справка' или 'Помощь' и я постараюсь объяснить что к чему";
            }
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
               && EntitiesConvert.TryParseInt(aliceRequest.request.nlu.entities, out var number))
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

            if (command.Contains("удали") || command.Contains("сотри") || command.Contains("стереть") ||
                command.Contains("отчисти") || command.Contains("очисти"))
            {
                return user.OpenedBox != 0 ? RequestModes.DeleteMessage : RequestModes.BoxIsNotOpen;
            }

            return RequestModes.SomethingWrong;
        }
    }
}
