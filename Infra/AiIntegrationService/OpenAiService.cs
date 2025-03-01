using InvesAuto.ApiTest.ApiService;
using InvestAuto.Infra.BaseTest;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvesAuto.Infra.AiIntegrationService
{
    public class OpenAiService : InfraApiService
    {
        public enum AiRequestType
        {
            ApiRequest,
            GetStockCompanysPrompts,
            DataBaseAnalyst
        }
        string GetStockCompanysPrompts = "You are a professional AI stock analysis agent tasked with collecting the latest stock market news. Review the URL news I provide next and respond with just two words: a noteworthy stock to consider now. If multiple stocks are notable, prefix with the count (e.g., found 2 stocks: 1: LLY.  2: SOLV)";
        string DataBaseAnalyst = "\"You are a database analytics expert. I will provide you with a database schema in JSON format containing stock data, including price, volume, timestamps, and optional indicators like RSI, moving averages, and EPS. Analyze the data and determine which of the following conditions applies based on trends in the data and general market context:\r\n1. Excellent condition for investment now (price trending upward, RSI below 70, or positive EPS improvement)\r\n2. Normal condition, neither rising nor falling (price stable, RSI between 40-60, no significant changes in indicators)\r\n3. Reverse condition, worth investing in a short (price trending downward, RSI above 30, or negative EPS worsening)\r\nRespond with only one option—1, 2, or 3. Base your decision on the provided data, prioritizing price trends over time, and supplement with indicator analysis if available. If no clear trend is present, default to 2.\"";

        public async Task<string> OpenAiServiceRequest(string userPrompts, AiRequestType aiRequest)
        {
            OpenAiData openAiData = new OpenAiData();
            string model = OpenAiData.model;
            string prePrompt;
            switch (aiRequest)
            {
                case AiRequestType.ApiRequest:
                    prePrompt = GetStockCompanysPrompts;
                    break;
                case AiRequestType.GetStockCompanysPrompts:
                    prePrompt = GetStockCompanysPrompts;
                    break;
                case AiRequestType.DataBaseAnalyst:
                    prePrompt = DataBaseAnalyst;
                    break;
                default:
                    prePrompt = "";
                    break;
            }

            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");
            string apiResponce = "An error occurred or no response was returned.";
            string combinedPrompt = $"{prePrompt}\n\n{userPrompts}";
            try
            {
                ChatClient client = new ChatClient(model, apiKey);
                UserChatMessage message = new UserChatMessage(combinedPrompt);

                ChatCompletion completion = await client.CompleteChatAsync(message);

                if (completion?.Content != null && completion.Content.Count > 0)
                {
                    apiResponce = completion.Content[0].Text;

                }
                else
                {
                    apiResponce = "No valid content found in the response.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return apiResponce;
        }
    }
}
