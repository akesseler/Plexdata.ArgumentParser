/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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

namespace Plexdata.ArgumentParser.Extensions
{
    /// <summary>
    /// Task of class help generator is to create the help to be printed out to the 
    /// user. The user help should always describe how a program can be utilized by 
    /// its command line arguments.
    /// </summary>
    public static class HelpGenerator
    {
        /// <summary>
        /// The extension method performs help generation for the given instance. 
        /// Each line of the generated help output is not longer than 80 characters.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The generic type of a class that contains all help information.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a class for which help has to be generated.
        /// </param>
        /// <returns>
        /// The pre-formatted help text that can be directly printed out to the 
        /// user.
        /// </returns>
        public static String Generate<TInstance>(this TInstance instance) where TInstance : class
        {
            return HelpProcessor<TInstance>.Generate(instance);
        }

        /// <summary>
        /// The extension method performs help generation for the given instance 
        /// but uses the given length as right limit.
        /// </summary>
        /// <typeparam name="TInstance">
        /// The generic type of a class that contains all help information.
        /// </typeparam>
        /// <param name="instance">
        /// The instance of a class for which help has to be generated.
        /// </param>
        /// <param name="length">
        /// The maximum length of each line inside the help output. 
        /// </param>
        /// <returns>
        /// The pre-formatted help text that can be directly printed out to the 
        /// user.
        /// </returns>
        public static String Generate<TInstance>(this TInstance instance, Int32 length) where TInstance : class
        {
            return HelpProcessor<TInstance>.Generate(instance, length);
        }
    }
}
