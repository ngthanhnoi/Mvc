// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Framework.Internal;

namespace Microsoft.AspNet.Mvc
{
    public class TempDataActionFilter : IActionFilter
    {
        public void OnActionExecuting([NotNull] ActionExecutingContext context)
        {
        }

        public void OnActionExecuted([NotNull] ActionExecutedContext context)
        {
            var controller = context.Controller as Controller;
            controller.TempData.Save();
        }
    }
}