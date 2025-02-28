// Application/Common/IQuery.cs

using Ardalis.Result;
using MediatR;

namespace Application.Common.Interfaces;

public interface IQuery<TResponse>
    : IRequest<Result<TResponse>> { }