// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net.Http.Headers;
using HarfBuzzSharp;

namespace CleanArchitecture.Blazor.Server.UI.Services;

public class OCRService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContext;

    public OCRService(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContext) {
        _httpClient = httpClientFactory.CreateClient("BackendApiClient");
        _httpContext = httpContext;
    }
    public async Task<string> Process(Stream stream)
    {
        var cultureCookie = _httpContext.HttpContext.Request.Cookies[".AspNetCore.Culture"];
        if (!string.IsNullOrEmpty(cultureCookie))
        {
            // 尝试解析文化信息
            var parts = cultureCookie.Split('|');
            foreach (var part in parts)
            {
                var keyValue = part.Split('=');
                if (keyValue.Length == 2 && keyValue[0] == "c")
                {
                    var cultureValue = keyValue[1];
                    if (!string.IsNullOrEmpty(cultureValue))
                    {
                        try
                        {
                            var languageValue = new StringWithQualityHeaderValue(cultureValue);
                            _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(languageValue);
                        }
                        catch (FormatException)
                        {
                            // Handle the case where the culture value is not in the correct format
                            Console.WriteLine("文化信息格式不正确: " + cultureValue);
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            var lang = _httpContext.HttpContext.Request.Headers.AcceptLanguage.First();
            if (!string.IsNullOrEmpty(lang))
            {
                // 拆分多个语言标签
                var languages = lang.Split(',')
                                    .Select(StringWithQualityHeaderValue.Parse)
                                    .ToList();

                // 清除之前的语言设置（如果有必要）
                _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();

                // 逐个添加语言标签
                foreach (var language in languages)
                {
                    _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(language);
                }
            }
        }
        using (var content = new MultipartFormDataContent())
        {
            // 创建文件的内容
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            content.Add(fileContent, "file","upload.png");

            // 发送 POST 请求
            var response = await _httpClient.PostAsync("process/", content);
            response.EnsureSuccessStatusCode();

            // 获取并打印响应内容
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return responseBody;
        }
    }
}
