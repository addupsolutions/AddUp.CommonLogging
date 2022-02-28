#region License

/*
 * Copyright © 2002-2009 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;

namespace AddUp.CommonLogging
{
    /// <summary>
    /// <see cref="ILoggerFactoryAdapter"/> is used internally by <see cref="LogManager"/>.
    /// </summary>
    /// <remarks>Only developers wishing to write new adapters need to worry about this interface.</remarks>
    /// <author>Gilles Bayon</author>
    public interface ILoggerFactoryAdapter
    {
        /// <summary>
        /// Gets an <see cref="ILog"/> instance by type.
        /// </summary>
        /// <param name="type">The type to use for the logger</param>
        /// <returns></returns>
		ILog GetLogger(Type type);

        /// <summary>
        /// Gets an <see cref="ILog"/> instance by key.
        /// </summary>
        /// <param name="key">The key of the logger</param>
        /// <returns></returns>
		ILog GetLogger(string key);
    }
}
