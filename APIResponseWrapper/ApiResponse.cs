using System.Net;

namespace APIResponseWrapper;

/// <summary>
/// Represents a standardized response format for API requests.
/// </summary>
/// <typeparam name="T">The type of data returned in the response.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the request was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message that provides additional context about the response.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the data returned from the API. This could be a single object or a list.
    /// </summary>
    public object Data { get; set; }

    /// <summary>
    /// Gets or sets the metadata associated with the response, such as pagination details.
    /// </summary>
    public MetaData Meta { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code of the response.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Gets or sets any additional headers that should be included in the response.
    /// </summary>
    public Dictionary<string, string> Headers { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// </summary>
    /// <param name="success">Indicates whether the request was successful.</param>
    /// <param name="message">The message explaining the result of the request.</param>
    /// <param name="data">The data returned from the API (optional).</param>
    /// <param name="meta">The metadata providing pagination or additional details (optional).</param>
    /// <param name="statusCode">The HTTP status code for the response (default is OK).</param>
    /// <param name="headers">Optional additional headers.</param>
    public ApiResponse(bool success, string message, object data = null, MetaData meta = null, HttpStatusCode statusCode = HttpStatusCode.OK, Dictionary<string, string> headers = null)
    {
        Success = success;
        Message = message;
        Data = data;
        Meta = meta;
        StatusCode = statusCode;
        Headers = headers ?? new Dictionary<string, string>();
    }

    #region Static Methods

    /// <summary>
    /// Creates a successful response with a single data item and optional metadata.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    /// <param name="meta">Pagination or other metadata (optional).</param>
    /// <param name="statusCode">The HTTP status code for the response (default is OK).</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing a successful response with the data.</returns>
    public static ApiResponse<T> CreateSuccessResponse(T data, MetaData meta = null, HttpStatusCode statusCode = HttpStatusCode.OK, Dictionary<string, string> headers = null)
    {
        return new ApiResponse<T>(true, "Request successfully completed.", data, meta, statusCode, headers);
    }

    /// <summary>
    /// Creates a successful response with a list of data and pagination metadata.
    /// </summary>
    /// <param name="data">The list of data to include in the response.</param>
    /// <param name="meta">Pagination metadata (must be provided when returning a list).</param>
    /// <param name="statusCode">The HTTP status code for the response (default is OK).</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing a successful response with the list of data.</returns>
    public static ApiResponse<T> CreateSuccessResponse(List<T> data, MetaData meta, HttpStatusCode statusCode = HttpStatusCode.OK, Dictionary<string, string> headers = null)
    {
        if (meta == null)
            throw new ArgumentException("MetaData must be provided when returning a list of data.");

        return new ApiResponse<T>(true, "Request successfully completed.", data, meta, statusCode, headers);
    }

    /// <summary>
    /// Creates a failure response with the specified error message and HTTP status code.
    /// </summary>
    /// <param name="message">The error message to return.</param>
    /// <param name="statusCode">The HTTP status code for the response (default is BadRequest).</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing a failure response.</returns>
    public static ApiResponse<T> CreateFailureResponse(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest, Dictionary<string, string> headers = null)
    {
        return new ApiResponse<T>(false, message, null, null, statusCode, headers);
    }

    /// <summary>
    /// Creates a response indicating that the requested resource was not found.
    /// </summary>
    /// <param name="message">The error message to return.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing a not found response.</returns>
    public static ApiResponse<T> CreateNotFoundResponse(string message, Dictionary<string, string> headers = null)
    {
        return CreateFailureResponse(message, HttpStatusCode.NotFound, headers);
    }

    /// <summary>
    /// Creates a response indicating that the request was unauthorized.
    /// </summary>
    /// <param name="message">The error message to return.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing an unauthorized response.</returns>
    public static ApiResponse<T> CreateUnauthorizedResponse(string message, Dictionary<string, string> headers = null)
    {
        return CreateFailureResponse(message, HttpStatusCode.Unauthorized, headers);
    }

    /// <summary>
    /// Creates a response indicating an internal server error occurred.
    /// </summary>
    /// <param name="message">The error message to return.</param>
    /// <param name="headers">Optional additional headers.</param>
    /// <returns>An instance of <see cref="ApiResponse{T}"/> representing an internal server error response.</returns>
    public static ApiResponse<T> CreateInternalServerErrorResponse(string message, Dictionary<string, string> headers = null)
    {
        return CreateFailureResponse(message, HttpStatusCode.InternalServerError, headers);
    }

    #endregion
}
