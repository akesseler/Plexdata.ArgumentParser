/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using Plexdata.ArgumentParser.Processors;
using System;
using System.Collections.Generic;

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// The argument resolver extension.
    /// </summary>
    /// <remarks>
    /// Task of class argument resolve is to perform the command line processing 
    /// as well as the command line validation.
    /// </remarks>
    public static class ArgumentResolver
    {
        #region Publics

        /// <summary>
        /// Performs an attribute validation of given instance.
        /// </summary>
        /// <remarks>
        /// The extension method performs an attribute validation of given instance.
        /// </remarks>
        /// <typeparam name="TInstance">
        /// The generic type of a class that represents all available command line arguments 
        /// of a particular program.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a class that represents all available command line arguments of 
        /// a particular program.
        /// </param>
        public static void Validate<TInstance>(this TInstance instance) where TInstance : class
        {
            ArgumentProcessor<TInstance>.Validate(instance);
        }

        /// <summary>
        /// Performs the processing of all given command line arguments and tries to assign 
        /// each argument to its corresponding properties.
        /// </summary>
        /// <remarks>
        /// The extension method performs the processing of all given command line arguments 
        /// and tries to assign each argument to its corresponding properties.
        /// </remarks>
        /// <typeparam name="TInstance">
        /// The generic type of a class that represents all available command line arguments 
        /// of a particular program.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a class that represents all available command line arguments of 
        /// a particular program.
        /// </param>
        /// <param name="arguments">
        /// An array containing all currently used command line arguments. Such an array is 
        /// typically the "args" parameter of a program's "main" method.
        /// </param>
        public static void Process<TInstance>(this TInstance instance, String[] arguments) where TInstance : class
        {
            ArgumentProcessor<TInstance>.Process(instance, arguments);
        }

        /// <summary>
        /// Performs the processing of all given command line arguments and tries to assign 
        /// each argument to its corresponding properties.
        /// </summary>
        /// <remarks>
        /// The extension method performs the processing of all given command line arguments 
        /// and tries to assign each argument to its corresponding properties.
        /// </remarks>
        /// <typeparam name="TInstance">
        /// The generic type of a class that represents all available command line arguments 
        /// of a particular program.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a class that represents all available command line arguments of 
        /// a particular program.
        /// </param>
        /// <param name="arguments">
        /// A list containing all currently used command line arguments. Such a list typically 
        /// represents the "args" parameter of a program's "main" method.
        /// </param>
        public static void Process<TInstance>(this TInstance instance, List<String> arguments) where TInstance : class
        {
            ArgumentProcessor<TInstance>.Process(instance, arguments);
        }

        #endregion
    }
}
