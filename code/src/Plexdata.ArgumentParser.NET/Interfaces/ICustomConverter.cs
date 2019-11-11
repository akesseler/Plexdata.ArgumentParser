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

using System;

namespace Plexdata.ArgumentParser.Interfaces
{
    /// <summary>
    /// Allows users to parse command line arguments for custom types.
    /// </summary>
    /// <remarks>
    /// This interface is intended to enable users to parse command 
    /// line arguments for their own custom types.
    /// </remarks>
    /// <typeparam name="TTarget">
    /// The custom target type to be used.
    /// </typeparam>
    public interface ICustomConverter<TTarget>
    {
        /// <summary>
        /// Converts provided <paramref name="argument"/> and applies all 
        /// fitting properties of resulting target type.
        /// </summary>
        /// <remarks>
        /// This method converts provided <paramref name="argument"/> and 
        /// applies all fitting properties of resulting target type.
        /// </remarks>
        /// <param name="parameter">
        /// The corresponding parameter from the command line for a property 
        /// of that type.
        /// </param>
        /// <param name="argument">
        /// The corresponding argument from the command line for a property 
        /// of that type.
        /// </param>
        /// <param name="delimiter">
        /// The delimiter to be used to split each argument value.
        /// </param>
        /// <returns>
        /// The custom type to apply some command line arguments.
        /// </returns>
        TTarget Convert(String parameter, String argument, String delimiter);
    }
}
