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
    /// Valor retornado em caso de sucesso
    /// </summary>
    public T? Value { get; private set; }

    /// <summary>
    /// Mensagens de erro em caso de falha
    /// </summary>
    public IList<string>? Errors { get; private set; }

    /// <summary>
    /// Códigos de erro
    /// </summary>
    public IList<int>? ErrorCodes { get; private set; }

    /// <summary>
    /// Inicializa uma nova instância de Result
    /// </summary>
    private Result(bool isSuccess, T? value, IList<string>? error, IList<int>? errorCode)
    {
        IsSuccess = isSuccess;
        Value = value;
        Errors = error;
        ErrorCodes = errorCode;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static Result<T> Success(T value)
        => new(true, value, null, null);

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    public static Result<T> Failure(IList<string> error, IList<int>? errorCode = null)
        => new(false, default, error, errorCode);
}

/// <summary>
/// Classe para representar o resultado de uma operação sem valor de retorno
/// </summary>
public class Result
{
    /// <summary>
    /// Indica se a operação foi bem-sucedida
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Mensagem de erro em caso de falha
    /// </summary>
    public string? Error { get; private set; }

    /// <summary>
    /// Código de erro
    /// </summary>
    public int? ErrorCode { get; private set; }

    /// <summary>
    /// Inicializa uma nova instância de Result
    /// </summary>
    private Result(bool isSuccess, string? error, int? errorCode)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorCode = errorCode;
    }

    /// <summary>
    /// Cria um resultado de sucesso
    /// </summary>
    public static Result Success()
        => new(true, null, null);

    /// <summary>
    /// Cria um resultado de falha
    /// </summary>
    public static Result Failure(string error, int? errorCode = null)
        => new(false, error, errorCode);
}
