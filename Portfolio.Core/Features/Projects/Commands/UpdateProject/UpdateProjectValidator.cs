using FluentValidation;

namespace Portfolio.Core.Features.Projects.Commands.UpdateProject
{
    internal class UpdateProjectValidator : AbstractValidator<UpdateProjectModel>
    {
        public UpdateProjectValidator()
        {
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(p => p.GitHubUrl)
             .Must(link => Uri.IsWellFormedUriString(link, UriKind.Absolute))
             .WithMessage("GitHub URL must be a valid URL.");

            RuleFor(p => p.LiveUrl)
             .Must(link => Uri.IsWellFormedUriString(link, UriKind.Absolute))
             .WithMessage("Live URL must be a valid URL.");
        }

    }
}
