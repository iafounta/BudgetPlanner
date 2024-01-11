using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Expenses;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Expenses.AddExpense;

namespace BudgetPlanner.Application.Tests;

public class AddExpenseTests
{
    private readonly Mock<IExpenseRepository> _mockRepository;
    private readonly AddExpensesHandler _handler;

    public AddExpenseTests()
    {
        _mockRepository = new Mock<IExpenseRepository>();
        _handler = new AddExpensesHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenSaveExpenseAsyncReturnsZero()
    {
        // Arrange
        var command = new AddExpense("Test", 1000, "Monthly");
        _mockRepository.Setup(repo => repo.SaveExpenseAsync(It.IsAny<Domain.Entities.Expense>())).ReturnsAsync(0);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotSaveExpenses);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenSaveExpenseAsyncReturnsValidId()
    {
        // Arrange
        var command = new AddExpense("Test", 3000, "Monthly");
        _mockRepository.Setup(repo => repo.SaveExpenseAsync(It.IsAny<Domain.Entities.Expense>())).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value);
    }
}