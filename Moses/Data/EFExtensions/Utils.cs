using Moses.Data.EFExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moses.Data
{
    /// <summary>
    /// General purpose helper methods.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Zips together two sequences (aligns values in the two sequences and returns them pair-wise).
        /// </summary>
        /// <typeparam name="TLeft">Element type for the left sequence.</typeparam>
        /// <typeparam name="TRight">Element type for the right sequence.</typeparam>
        /// <typeparam name="TResult">Element type for the result sequence.</typeparam>
        /// <param name="left">Left sequence.</param>
        /// <param name="right">Right sequence.</param>
        /// <param name="resultSelector">Result selector that takes a (left, right) pair.</param>
        /// <returns>Zipped results.</returns>
        public static IEnumerable<TResult> Zip<TLeft, TRight, TResult>(this IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TRight, TResult> resultSelector)
        {
            Utility.CheckArgumentNotNull(resultSelector, "resultSelector");

            if (null == left || null == right)
            {
                return Enumerable.Empty<TResult>();
            }
            else
            {
                return ZipIterator(left, right, resultSelector);
            }
        }

        private static IEnumerable<TResult> ZipIterator<TLeft, TRight, TResult>(IEnumerable<TLeft> left, IEnumerable<TRight> right, Func<TLeft, TRight, TResult> resultSelector)
        {
            using (IEnumerator<TLeft> leftEnumerator = left.GetEnumerator())
            using (IEnumerator<TRight> rightEnumerator = right.GetEnumerator())
            {
                while (leftEnumerator.MoveNext() && rightEnumerator.MoveNext())
                {
                    yield return resultSelector(leftEnumerator.Current, rightEnumerator.Current);
                }
            }
        }

        /// <summary>
        /// Foreach element in the input, runs the specified action. Yields input elements.
        /// </summary>
        /// <typeparam name="TResult">Element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="action">Action to perform on each element of the source.</param>
        /// <returns>Source elements.</returns>
        public static IEnumerable<TResult> ForEach<TResult>(this IEnumerable<TResult> source, Action<TResult> action)
        {
            Utility.CheckArgumentNotNull(source, "source");
            Utility.CheckArgumentNotNull(action, "action");

            return ForEachIterator(source, action);
        }

        private static IEnumerable<TResult> ForEachIterator<TResult>(IEnumerable<TResult> source, Action<TResult> action)
        {
            foreach (TResult element in source)
            {
                action(element);
                yield return element;
            }
        }

        /// <summary>
        /// Determines whether a generic type definition is assignable from a type given some
        /// generic type arguments. For instance, <code>typeof(IEnumerable&lt;&gt;).IsGenericAssignableFrom(typeof(List&lt;int&gt;), out genericArguments)</code>
        /// returns true with generic arguments { typeof(int) }.
        /// </summary>
        /// <param name="toType">Target generic type definition (to which the value would be assigned).</param>
        /// <param name="fromType">Source type (instance of which is being assigned)</param>
        /// <param name="genericArguments">Returns generic type arguments required for the assignment to succeed
        /// or null if no such assignment exists.</param>
        /// <returns>true if the type can be assigned; otherwise false</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#")]
        public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
        {
            Utility.CheckArgumentNotNull(toType, "toType");
            Utility.CheckArgumentNotNull(fromType, "fromType");

            if (!toType.IsGenericTypeDefinition ||
                fromType.IsGenericTypeDefinition)
            {
                // if 'toType' is not generic or 'fromType' is generic, the assignment pattern 
                // is not matched (e.g. toType<genericArguments>.IsAssignableFrom(fromType)
                // cannot be satisfied)
                genericArguments = null;
                return false;
            }

            if (toType.IsInterface)
            {
                // if the toType is an interface, simply look for the interface implementation in fromType
                foreach (Type interfaceCandidate in fromType.GetInterfaces())
                {
                    if (interfaceCandidate.IsGenericType && interfaceCandidate.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = interfaceCandidate.GetGenericArguments();
                        return true;
                    }
                }
            }
            else
            {
                // if toType is not an interface, check hierarchy for match
                while (fromType != null)
                {
                    if (fromType.IsGenericType && fromType.GetGenericTypeDefinition() == toType)
                    {
                        genericArguments = fromType.GetGenericArguments();
                        return true;
                    }
                    fromType = fromType.BaseType;
                }
            }
            genericArguments = null;
            return false;
        }

        internal static void CheckArgumentNotNull<T>(T argumentValue, string argumentName)
            where T : class
        {
            if (null == argumentValue)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        internal static void CheckArgumentNotNullOrEmpty(string argumentValue, string argumentName)
        {
            CheckArgumentNotNull(argumentValue, argumentName);

            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentException(Messages.EmptyArgument, argumentName);
            }
        }
    }
}