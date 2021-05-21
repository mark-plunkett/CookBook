using System.Linq;

namespace CookBook.Infrastructure
{
    public abstract class BusinessRule
    {
        public static void Enforce(IBusinessRule rule, string propertyName)
        {
            if (rule.IsViolated)
                throw new BusinessRuleException(new BusinessError(propertyName, rule.Message));
        }

        public static void EnforceAll(params IBusinessRule[] rules)
        {
            var violated = rules
                .Where(rule => rule.IsViolated)
                .ToList();
            if (violated.Any())
                throw new BusinessRuleException(violated.Select(rule => new BusinessError(rule.PropertyName, rule.Message)));
        }

        public static void Enforce(IBusinessRule rule)
        {
            if (rule.IsViolated)
                throw new BusinessRuleException(new BusinessError(rule.Message));
        }

        public static void EnforceWithMessage(IBusinessRule rule, string message)
        {
            if (rule.IsViolated)
                throw new BusinessRuleException(new BusinessError(message));
        }

        public static void Enforce(IBusinessRule rule, string message, string propertyName)
        {
            if (rule.IsViolated)
                throw new BusinessRuleException(new BusinessError(propertyName, message));
        }
    }
}