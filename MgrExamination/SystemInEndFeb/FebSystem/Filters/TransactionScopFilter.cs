using FebSystem.Helper.Attritubes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace FebSystem.Filters
{
    /// <summary>
    /// start transction
    /// </summary>
    public class TransactionScopFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool IsNotUseTransaction = false;
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                var actionDec = context.ActionDescriptor as ControllerActionDescriptor;
                IsNotUseTransaction = actionDec.MethodInfo.IsDefined(typeof(NotUseTransactionAttribute), true);
            }
            if (IsNotUseTransaction)
            {
                await next();
                return;
            }
            using var txScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            var result = await next();
            if (result.Exception == null)
            {
                txScope.Complete();
            }
        }
    }
}
