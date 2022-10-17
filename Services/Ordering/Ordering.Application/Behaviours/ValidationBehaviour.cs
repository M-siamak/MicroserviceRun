using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Ordering.Application.Exceptions.ValidationException;


namespace Ordering.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validator;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validator = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            if (_validator.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults =
                await Task.WhenAll(
                    _validator.Select(v => v.ValidateAsync(context, cancellationToken))
                    );

                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f is not null)
                    .ToList();

                if (failures.Count() != 0)
                    throw new ValidationException(failures);

            }
            return await next();
        }
    }
}
