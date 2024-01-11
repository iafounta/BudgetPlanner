using BudgetPlanner.Application.Interfaces;
using BudgetPlanner.Application.UseCases.Income;
using BudgetPlanner.Domain.Entities;
using BudgetPlanner.Domain.Results;
using Moq;
using static BudgetPlanner.Application.UseCases.Income.GetAllIncomes;

namespace BudgetPlanner.Application.Tests;

public class GetAllIncomesTest
{
    private readonly Mock<IIncomeRepository> _mockRepository;
    private readonly GetAllIncomeHandler _handler;

    public GetAllIncomesTest()
    {
        _mockRepository = new Mock<IIncomeRepository>();
        _handler = new GetAllIncomeHandler(_mockRepository.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenGetIncomesAsyncReturnsNull()
    {
        // Arrange
        var query = new GetAllIncomes();
        _mockRepository.Setup(repo => repo.GetIncomesAsync()).ReturnsAsync((List<Income>)null);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(result.Error, ErrorMessage.CannotFetchIncome);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenGetIncomesAsyncReturnsNonEmptyList()
    {
        // Arrange
        var query = new GetAllIncomes();
        var incomes = new List<Income> { new(), new() };
        _mockRepository.Setup(repo => repo.GetIncomesAsync()).ReturnsAsync(incomes);

        // Act
        var result = await _handler.Handle(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value, incomes);
    }
}