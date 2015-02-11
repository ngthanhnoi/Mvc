// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc
{
    public interface ITempDataProvider
    {
        IDictionary<string, object> LoadTempData([NotNull] ActionExecutingContext context);

        void SaveTempData([NotNull] ActionExecutedContext context, IDictionary<string, object> values);
    }
}