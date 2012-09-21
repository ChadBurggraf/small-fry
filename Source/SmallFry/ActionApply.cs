//-----------------------------------------------------------------------------
// <copyright file="ActionApply.cs" company="Tasty Codes">
//     Copyright (c) 2012 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------------

namespace SmallFry
{
    using System;

    /// <summary>
    /// Provides partial function application to <see cref="Action"/> delegates
    /// </summary>
    public static class ActionApply
    {
        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T">The type of the first argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>A partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T>(this Action<T> action, T arg1)
        {
            return () => action(arg1);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2> Apply<T1, T2>(this Action<T1, T2> action, T1 arg1)
        {
            return a => action(arg1, a);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3> Apply<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1)
        {
            return (a, b) => action(arg1, a, b);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3> Apply<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3, T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1)
        {
            return (a, b, c) => action(arg1, a, b, c);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3, T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T4> Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1)
        {
            return (a, b, c, d) => action(arg1, a, b, c, d);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3, T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T4, T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T5> Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3, T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1)
        {
            return (a, b, c, d, e) => action(arg1, a, b, c, d, e);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3, T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T4, T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T5, T6> Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T6> Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3, T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1)
        {
            return (a, b, c, d, e, f) => action(arg1, a, b, c, d, e, f);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3, T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T4, T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T5, T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T6, T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T7> Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <param name="arg7">The seventh argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6).Apply(arg7);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T2, T3, T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1)
        {
            return (a, b, c, d, e, f, g) => action(arg1, a, b, c, d, e, f, g);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T3, T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2)
        {
            return action.Apply(arg1).Apply(arg2);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T4, T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T5, T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T6, T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T7, T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <param name="arg7">The seventh argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action<T8> Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6).Apply(arg7);
        }

        /// <summary>
        /// Partially applies arguments to an <see cref="Action"/> delegate.
        /// </summary>
        /// <typeparam name="T1">The type of the first argument.</typeparam>
        /// <typeparam name="T2">The type of the second argument.</typeparam>
        /// <typeparam name="T3">The type of the third argument.</typeparam>
        /// <typeparam name="T4">The type of the fourth argument.</typeparam>
        /// <typeparam name="T5">The type of the fifth argument.</typeparam>
        /// <typeparam name="T6">The type of the sixth argument.</typeparam>
        /// <typeparam name="T7">The type of the seventh argument.</typeparam>
        /// <typeparam name="T8">The type of the eighth argument.</typeparam>
        /// <param name="action">The <see cref="Action"/> delegate to partially apply arguments to.</param>
        /// <param name="arg1">The first argument to apply.</param>
        /// <param name="arg2">The second argument to apply.</param>
        /// <param name="arg3">The third argument to apply.</param>
        /// <param name="arg4">The fourth argument to apply.</param>
        /// <param name="arg5">The fifth argument to apply.</param>
        /// <param name="arg6">The sixth argument to apply.</param>
        /// <param name="arg7">The seventh argument to apply.</param>
        /// <param name="arg8">The eighth argument to apply.</param>
        /// <returns>The partially applied <see cref="Action"/> delegate.</returns>
        public static Action Apply<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return action.Apply(arg1).Apply(arg2).Apply(arg3).Apply(arg4).Apply(arg5).Apply(arg6).Apply(arg7).Apply(arg8);
        }
    }
}