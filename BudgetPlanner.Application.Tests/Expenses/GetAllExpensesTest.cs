using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Expenses;
using BudgetPlanner.Domain.Entities;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Expenses.GetAllExpense;

namespace BudgetPlanner.Application.Tests;

public class GetAllExpensesTest
{
    private readonly Mock<IExpenseRepository> _mockRepository;
    private readonly GetAllExpenseHandler _handler;

    public GetAllExpensesTest()
    {
        _mockRepository = new Mock<IExpenseRepository>();
        _handler = new GetAllExpenseHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGetExpenseAsyncReturnsNull()
    {
        // Arrange
        var query = new GetAllExpense();
        _mockRepository.Setup(repo => repo.GetExpensesAsync()).ReturnsAsync((List<Expense>)null);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotFetchExpenses);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenGetExpenseAsyncReturnsNonEmptyList()
    {
        // Arrange
        var query = new GetAllExpense();
        var expense = new List<Expense> { new(), new() };
        _mockRepository.Setup(repo => repo.GetExpensesAsync()).ReturnsAsync(expense);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value, expense);
    }
}