using InvesAuto.ApiTest.ApiService;
using InvestAuto.Infra.BaseTest;
using Newtonsoft.Json;
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
        public enum AiPrePromptType
        {
            ApiRequest,
            GetStockCompanysPrompts,
            DataBaseAnalyst
        }
        string GetStockCompanysPrompts = "string systemMessage = \"You are a stock market professional and financial analyst. Your task is to analyze the latest stock market news articles and provide insights. Based on the provided news, assess whether any of the mentioned stocks, sectors, or economic trends indicate a potential opportunity or risk. Your response should be clear, data-driven, and actionable.";
        string DataBaseAnalyst = "\"You are a database analytics expert. I will provide you with a database schema in JSON format containing stock data, including price, volume, timestamps, and optional indicators like RSI, moving averages, and EPS. Analyze the data and determine which of the following conditions applies based on trends in the data and general market context:\r\n1. Excellent condition for investment now (price trending upward, RSI below 70, or positive EPS improvement)\r\n2. Normal condition, neither rising nor falling (price stable, RSI between 40-60, no significant changes in indicators)\r\n3. Reverse condition, worth investing in a short (price trending downward, RSI above 30, or negative EPS worsening)\r\nRespond with only one option—1, 2, or 3. Base your decision on the provided data, prioritizing price trends over time, and supplement with indicator analysis if available. If no clear trend is present, default to 2.\"";

        public async Task<string> OpenAiServiceRequest(string userPrompts, AiPrePromptType aiRequest)
        {
            OpenAiData openAiData = new OpenAiData();
            string model = OpenAiData.model;
            string prePrompt;
            switch (aiRequest)
            {
                case AiPrePromptType.ApiRequest:
                    prePrompt = GetStockCompanysPrompts;
                    break;
                case AiPrePromptType.GetStockCompanysPrompts:
                    prePrompt = GetStockCompanysPrompts;
                    break;
                case AiPrePromptType.DataBaseAnalyst:
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

        string grokUrl = "https://api.x.ai/v1/chat/completions";
        string apiGrokKey = Environment.GetEnvironmentVariable("GROK_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");

        public async Task<string> GetGrokResponse(string userMessage, AiPrePromptType aiRequest)
        {
            using (HttpClient client = new HttpClient())
            {
                string prePrompt;

                // Assign the correct system prompt based on AiPrePromptType
                switch (aiRequest)
                {
                    case AiPrePromptType.ApiRequest:
                        prePrompt = GetStockCompanysPrompts;
                        break;
                    case AiPrePromptType.GetStockCompanysPrompts:
                        prePrompt = GetStockCompanysPrompts;
                        break;
                    case AiPrePromptType.DataBaseAnalyst:
                        prePrompt = DataBaseAnalyst;
                        break;
                    default:
                        prePrompt = "You are an AI assistant."; // Default fallback
                        break;
                }

                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiGrokKey}");

                var requestBody = new
                {
                    messages = new[]
                    {
                        new { role = "system", content = prePrompt },  // Assign AiPrePromptType here
                        new { role = "user", content = userMessage }   // User input
                },
                    model = "grok-2-1212",  // Use grok-3 if available
                    stream = false,
                    temperature = 0
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(grokUrl, jsonContent);


                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
                }
            }
        }

    }
}

