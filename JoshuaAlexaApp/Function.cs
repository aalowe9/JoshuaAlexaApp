using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Internal;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using JoshuaAlexaApp.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace JoshuaAlexaApp
{
    public class Function
    {
       
       
        public Resources GetResources()
        {
            Resources resources = new Resources("en-GB")
            {
                SkillName = "Joshua App",
                StopMessage = "Goodbye!",
                HelpMessage = "What can i help you with?",
                HelpReprompt = "How can i help?"
            };

            return resources;
        }
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            Resources resources = GetResources();

            SkillResponse response = new SkillResponse();
            response.Response = new ResponseBody();
            response.Response.ShouldEndSession = false;
            IOutputSpeech innerResponse = null;
            var log = context.Logger;

            // HANDLE LAUNCH
            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                log.LogLine($"Default LaunchRequest made: 'Alexa, open Joshua App");
                innerResponse = new PlainTextOutputSpeech();
                (innerResponse as PlainTextOutputSpeech).Text = "Welcome to Joshua App";
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;
                switch (intentRequest.Intent.Name)
                {
                    case "AMAZON.CancelIntent":
                        log.LogLine($"AMAZON.CancelIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = resources.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.StopIntent":
                        log.LogLine($"AMAZON.StopIntent: send StopMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = resources.StopMessage;
                        response.Response.ShouldEndSession = true;
                        break;
                    case "AMAZON.HelpIntent":
                        log.LogLine($"AMAZON.HelpIntent: send HelpMessage");
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = resources.HelpMessage;
                        break;
                    case "AddDate":
                        var val = intentRequest.Intent.Slots["activity"].Value;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage r1 = await client.GetAsync("http://ec2-52-51-10-4.eu-west-1.compute.amazonaws.com/api/alexa/"+ val);

                        if (r1.IsSuccessStatusCode)
                        {
                            var x = 1;
                        }
                        
                        log.LogLine($"AddDate sent: Adding date: " +val);
                        innerResponse = new PlainTextOutputSpeech();
                        (innerResponse as PlainTextOutputSpeech).Text = "Well done Joshua";


                        break;
                    default:
                        log.LogLine($"Unknown intent: " + intentRequest.Intent.Name);
                        //    innerResponse = new PlainTextOutputSpeech();
                        //   (innerResponse as PlainTextOutputSpeech).Text = "Unknown intent";
                        break;
                }
            }

            response.Response.OutputSpeech = innerResponse;
            response.Version = "1.0";

            return response;
        }
    }

  
}
