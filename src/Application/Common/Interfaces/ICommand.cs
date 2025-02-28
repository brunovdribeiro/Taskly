using Ardalis.Result;
using MediatR;

namespace Application.Common.Interfaces;

public interface ICommand<TResponse>
    : IRequest<Result<TResponse>> { }

public interface ICommand
    : IRequest<Result> { }