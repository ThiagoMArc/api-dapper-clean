using System.Net;

namespace ApiDapperClean.Domain.Results;

/// <summary>
/// Classe genérica para representar o resultado de uma operação
/// </summary>
/// <typeparam name="T">Tipo do valor de sucesso</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// HttpStatusCode representando o status da operação
    /// </summary>
    public HttpStatusCode StatusCode { get; private set; }

    /// <summary>
    /// Dados retornados em caso de sucesso
    /// </summary>
    public T? Data { get; private set; }

    /// <summary>
    /// Mensagens de erro em caso de falha
    /// </summary>
    public IList<string>? Errors { get; private set; }

    /// <summary>
    /// Inicializa uma nova instância de Result
    /// </summary>
    private Result(bool isSuccess, HttpStatusCode statusCode, T? value, IList<string>? error)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Data = value;
        Errors = error;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static Result<T> Success(T? value, HttpStatusCode statusCode)
        => new(true, statusCode, value, []);

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    public static Result<T> Failure(IList<string> errors, HttpStatusCode statusCode)
        => new(false, statusCode, default, errors);
}
