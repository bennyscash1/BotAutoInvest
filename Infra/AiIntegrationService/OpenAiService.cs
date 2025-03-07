﻿using InvesAuto.ApiTest.ApiService;
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
            DataBaseAnalyst,
            PromptScanUrl
        }
        string GetStockCompanysPrompts = "You are a professional AI stock analysis agent tasked with collecting the latest stock market news. Review the URL news I provide next and respond with just two words: a noteworthy stock to consider now. If multiple stocks are notable, prefix with the count for example if you found 2 return me 1: value, 2: value, return only the Ticker Symbol for example for Apple retturn AAPL";
        string DataBaseAnalyst = "\"You are a database analytics expert. I will provide you with a database schema in JSON format containing stock data, including price, volume, timestamps, and optional indicators like RSI, moving averages, and EPS. Analyze the data and determine which of the following conditions applies based on trends in the data and general market context:\r\n1. Excellent condition for investment now (price trending upward, RSI below 70, or positive EPS improvement)\r\n2. Normal condition, neither rising nor falling (price stable, RSI between 40-60, no significant changes in indicators)\r\n3. Reverse condition, worth investing in a short (price trending downward, RSI above 30, or negative EPS worsening)\r\nRespond with only one option—1, 2, or 3. Base your decision on the provided data, prioritizing price trends over time, and supplement with indicator analysis if available. If no clear trend is present, default to 2.\"";
        string promptScanStringFromResponceNews =
        "You are a financial AI specialized in stock market analysis. Your task is to analyze the content of financial news articles and extract relevant stock information.\n\n"
        + "**Instructions:**\n"
        + "- Identify the **stock ticker symbols** mentioned in the article.\n"
        + "- Count the number of unique ticker symbols found.\n"
        + "- Output the result in **exactly this format**, without any additional words:\n\n"
        + "**Response Format:**\n"
        + "Amount: [Number of Symbols]\n"
        + "Symbols: [Comma-separated list of symbols]\n\n"
        + "**Example Outputs:**\n"
        + "- If no symbols are found:\n"
        + "  Amount: 0\n"
        + "  Symbols: \n\n"
        + "- If one symbol is found:\n"
        + "  Amount: 1\n"
        + "  Symbols: AAPL\n\n"
        + "- If multiple symbols are found:\n"
        + "  Amount: 3\n"
        + "  Symbols: AAPL, TSLA, AMZN\n\n"
        + "Do **not** include explanations, comments, or extra text—return only the response in the exact format specified.";
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
                case AiPrePromptType.PromptScanUrl:
                    prePrompt = promptScanStringFromResponceNews;
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
            Console.WriteLine("Before ai");
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");
            string apiResponce = "An error occurred or no response was returned.";
            //string combinedPrompt = $"{prePrompt}\n\n{userPrompts}";
            Console.WriteLine(  "after ai");
            try
            {
                ChatClient client = new ChatClient(model, apiKey);

                var openAiRequest = new
                {
                    model = model,
                    messages = new[]
                    {
                 new { role = "system", content = prePrompt },
                 new { role = "user", content = userPrompts }
             }
                };
                string jsonBody = JsonConvert.SerializeObject(openAiRequest, Formatting.Indented);

                UserChatMessage message = new UserChatMessage(jsonBody);
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
        public async Task<string> GetGrokResponse(string userMessage, AiPrePromptType aiRequest)
        {

            string grokUrl = "https://api.x.ai/v1/chat/completions";
            string apiGrokKey = Environment.GetEnvironmentVariable("GROK_API_KEY") ?? throw new InvalidOperationException("API key is missing from environment variables.");

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

