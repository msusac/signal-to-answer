using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace SignalToAnswer.Attributes
{
    public class TransactionalAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using (var transactionScope = new TransactionScope())
            {
                ActionExecutedContext actionExecutedContext = await next();

                if (actionExecutedContext.Exception == null)
                {
                    transactionScope.Complete();
                }
            }
        }
    }
}
