﻿namespace BudgetPlanner.Application.Interfaces;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
