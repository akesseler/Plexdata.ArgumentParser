/*
 * MIT License
 * 
 * Copyright (c) 2020 plexdata.de
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

namespace Plexdata.ArgumentParser.Constants
{
    /// <summary>
    /// The type describing how to handle dependency list entries.
    /// </summary>
    /// <remarks>
    /// The dependency type describes how entries in a dependency list have to be handled.
    /// </remarks>
    public enum DependencyType
    {
        /// <summary>
        /// All entries in a dependency list are handled with an OR operation.
        /// </summary>
        /// <remarks>
        /// Using this dependency type means, that just one of the referenced dependencies must be applied.
        /// </remarks>
        Optional,

        /// <summary>
        /// All entries in a dependency list are handled with an AND operation.
        /// </summary>
        /// <remarks>
        /// Using this dependency type means, that all of the referenced dependencies have to be applied.
        /// </remarks>
        Required,
    }
}
