// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.AspNet.Mvc
{
    public class TempDataActionFilter : IActionFilter
    {
        public void OnActionExecuting([NotNull] ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            controller.TempData.Load(context, controller.TempDataProvider);
        }

        public void OnActionExecuted([NotNull] ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            controller.TempData.Save(context, controller.TempDataProvider);
        }
    }
}