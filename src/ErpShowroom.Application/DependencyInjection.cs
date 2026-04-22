using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ErpShowroom.Application.Common.Behaviors;

namespace ErpShowroom.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddAutoMapper(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
