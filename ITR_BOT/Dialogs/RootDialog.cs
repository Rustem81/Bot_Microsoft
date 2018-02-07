using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using YandexLinguistics.NET;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace ITR_BOT.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            var text = activity.Text ?? String.Empty;
            var yandexSpeller = new Speller();
            var yandexSpellerResult = yandexSpeller.CheckText(text);
            var resutText = new StringBuilder(text);

            if (yandexSpellerResult.Errors.Any())
            {
                var shift = 0;
                foreach (var error in yandexSpellerResult.Errors)
                {
                    resutText.Insert(error.Position + shift, "**");
                    resutText.Insert(error.Position + error.Length + shift + 2, "**");
                    resutText.Append($"\n\r**{ error.Word}**:{ error.Steer}");
                    shift += 4;
                }


            }

            //var yandexSpeller = new Speller();
            // var yandexSpellerResult = yandexSpeller.CheckText(text);

            // var resulText = new StringBuilder();


            // calculate something for us to return
            // int length = (activity.Text ?? string.Empty).Length;

            // return our reply to the user
            // await context.PostAsync($"You sent {activity.Text} which was {length} characters");await context.PostAsync($"11111111111111 {text} which was {text} characters");
            await context.PostAsync(resutText.ToString());

            context.Wait(MessageReceivedAsync);
            
        }
    }
}