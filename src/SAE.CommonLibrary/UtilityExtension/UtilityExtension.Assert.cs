﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region IsNullOrWhiteSpace
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>Consists of empty nulls or a series of spaces
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<string> IsNullOrWhiteSpace(this IAssert<string> assert)
        {
            return assert.IsNullOrWhiteSpace($"{assert.Name},Not null or a string of spaces");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>Consists of empty nulls or a series of spaces
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<string> IsNullOrWhiteSpace(this IAssert<string> assert, string message)
        {
            if (!string.IsNullOrWhiteSpace(assert.Current))
            {
                throw new SAEException(StatusCodes.ParamesterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region NotNullOrWhiteSpace

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>Not null or a string of spaces
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<string> NotNullOrWhiteSpace(this IAssert<string> assert)
        {
            return assert.NotNullOrWhiteSpace($"{assert.Name},Is null or a series of consecutive spaces");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>Not null or a string of spaces
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<string> NotNullOrWhiteSpace(this IAssert<string> assert, string message)
        {
            if (string.IsNullOrWhiteSpace(assert.Current))
            {
                throw new SAEException(StatusCodes.ParamesterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region True
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is <see cref="bool.TrueString"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<bool> True(this IAssert<bool> assert)
        {
            return assert.True($"{assert.Name},Does not meet the true condition");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is <see cref="bool.TrueString"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<bool> True(this IAssert<bool> assert, string message)
        {
            if (!assert.Current)
            {
                throw new SAEException(StatusCodes.ParamesterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region False

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is <see cref="bool.FalseString"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<bool> False(this IAssert<bool> assert) => assert.False($"{assert.Name},False condition not met");
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is <see cref="bool.FalseString"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<bool> False(this IAssert<bool> assert, string message)
        {
            if (assert.Current)
            {
                throw new SAEException(StatusCodes.ParamesterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region Null
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is null
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<TAssert> Null<TAssert>(this IAssert<TAssert> assert)
        {
            return assert.Null($"{assert.Name},not null");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> is null
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<TAssert> Null<TAssert>(this IAssert<TAssert> assert, string message)
        {
            if (assert.Current != null)
            {
                throw new SAEException(StatusCodes.ParamesterInvalid,message);
            }
            return assert;
        }

        #endregion

        #region NotNull

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> Cannot be null
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<TAssert> NotNull<TAssert>(this IAssert<TAssert> assert)
        {
            return assert.NotNull($"{assert.Name},Cannot be null");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>Cannot be null
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IAssert<TAssert> NotNull<TAssert>(this IAssert<TAssert> assert, string message)
        {
            if (assert.Current == null)
            {
                throw new SAEException(StatusCodes.ResourcesNotExist, message);
            }
            return assert;
        }
        #endregion

        #region Then
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="delegate"></param>
        /// <param name="name"></param>
        /// <typeparam name="TAssert"></typeparam>
        /// <typeparam name="TThen"></typeparam>
        /// <returns></returns>
        public static IAssert<TThen> Then<TAssert, TThen>(this IAssert<TAssert> assert, Func<TAssert, TThen> @delegate, string name = "")
        {
            return Assert.Build(@delegate(assert.Current), name);
        }
        #endregion

        #region Any

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<IEnumerable<TAssert>> Any<TAssert>(this IAssert<IEnumerable<TAssert>> assert, Func<TAssert, bool> predicate, string message = null)
        {
            assert.NotNull()
                  .Then(s => s.Any(predicate))
                  .True(message ?? "No match exists");
            return assert;
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>
        /// </summary>
        /// <param name="assert"></param>
        /// <returns></returns>
        public static IAssert<IEnumerable<TAssert>> NotAny<TAssert>(this IAssert<IEnumerable<TAssert>> assert, Func<TAssert, bool> predicate, string message = null)
        {
            assert.NotNull()
                  .Then(s => s.Any(predicate))
                  .False(message ?? "Match already exists");
            return assert;
        }
        #endregion
    }
}
