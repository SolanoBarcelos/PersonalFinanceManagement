using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    /// <summary>
    /// Middleware global para interceptação, tratamento e padronização de exceções na pipeline HTTP.
    /// </summary>
    /// <remarks>
    /// Este componente centraliza o tratamento de erros das camadas de Domain, Application e Infrastructure,
    /// convertendo exceções de lógica de negócio em respostas HTTP amigaveis, e também evita o vazamento de dados sensíveis com error 500.
    /// </remarks>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        /// <summary>
        /// Inicializa uma nova instância do middleware.
        /// </summary>
        /// <param name="next">Próximo processo de requisição na pipeline.</param>
        /// <param name="logger">Serviço de log para registro de erros críticos.</param>
        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Chama o middleware para processar a requisição atual.
        /// </summary>
        /// <param name="context">Contexto HTTP da requisição.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Captura erros de todas as camadas e registra no log para depuração.
                _logger.LogError(ex, "Exceção capturada pelo middleware global.");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Traduz exceções do C# para respostas JSON com códigos de status HTTP amigaveis.
        /// </summary>
        /// <param name="context">Contexto da resposta.</param>
        /// <param name="exception">A exceção capturada.</param>
        /// <returns>Uma tarefa que escreve a resposta JSON no corpo da requisição.</returns>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Mapeamento estratégico de exceções de domínio para Status Codes.
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,          // 400 (Erro de validação)
                InvalidOperationException => (int)HttpStatusCode.BadRequest,  // 400 (Violação de regra de negócio)
                KeyNotFoundException => (int)HttpStatusCode.NotFound,         // 404 (Recurso não encontrado)
                _ => (int)HttpStatusCode.InternalServerError                  // 500 (Erro inesperado)
            };

            // Estrutura de resposta padronizada para o cliente.
            var response = new
            {
                StatusCode = context.Response.StatusCode,

                // Proteção de dados: erros 500 ocultam detalhes técnicos, os outros retornam a mensagem da exceção.
                Message = context.Response.StatusCode == 500
                    ? "Erro interno no servidor"
                    : exception.Message
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}