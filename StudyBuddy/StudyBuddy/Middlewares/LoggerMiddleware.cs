using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

    public LoggerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";

        await LogAsync("INFO", $"[{clientIp}] Request: {context.Request.Method} {context.Request.Path}");

        try
        {
            await _next(context);

            if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500)
            {
                await LogAsync("WARN", $"[{clientIp}] Client Error {context.Response.StatusCode}: {context.Request.Method} {context.Request.Path}");
            }
        }
        catch (Exception ex)
        {
            string errorDetails = $"[{clientIp}] Exception: {ex.Message}\nStack Trace:\n{ex.StackTrace}";
            await LogAsync("ERROR", errorDetails);
            throw; // rethrow after logging
        }
    }

    private async Task LogAsync(string level, string message)
    {
        var date = DateTime.UtcNow.ToString("yyyy-MM-dd");
        var folderPath = Path.Combine(_logRootPath, level);
        Directory.CreateDirectory(folderPath);

        var logFilePath = Path.Combine(folderPath, $"{date}.log");
        var logLine = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] [{level}] {message}{Environment.NewLine}";

        await File.AppendAllTextAsync(logFilePath, logLine);
    }
}
