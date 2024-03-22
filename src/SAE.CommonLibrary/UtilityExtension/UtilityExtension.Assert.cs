using System;
using System.Collections.Generic;
using System.Linq;

namespace SAE.CommonLibrary.Extension
{
    public static partial class UtilityExtension
    {
        #region IsNullOrWhiteSpace
        /// <summary>
        /// <see cref="IAssert{String}.Current"/>由空零或一系列空格组成
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<string> IsNullOrWhiteSpace(this IAssert<string> assert)
        {
            return assert.IsNullOrWhiteSpace($"{assert.Name},Not null or a string of spaces");
        }

        /// <summary>
        /// <see cref="IAssert{String}.Current"/>由空零或一系列空格组成
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
        public static IAssert<string> IsNullOrWhiteSpace(this IAssert<string> assert, string message)
        {
            if (!string.IsNullOrWhiteSpace(assert.Current))
            {
                throw new SAEException(StatusCodes.ParameterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region NotNullOrWhiteSpace

        /// <summary>
        /// <see cref="IAssert{String}.Current"/>不为空或空字符串
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<string> NotNullOrWhiteSpace(this IAssert<string> assert)
        {
            return assert.NotNullOrWhiteSpace($"{assert.Name},Is null or a series of consecutive spaces");
        }

        /// <summary>
        /// <see cref="IAssert{String}.Current"/>不为空或空字符串
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
        public static IAssert<string> NotNullOrWhiteSpace(this IAssert<string> assert, string message)
        {
            if (string.IsNullOrWhiteSpace(assert.Current))
            {
                throw new SAEException(StatusCodes.ParameterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region True
        /// <summary>
        /// <see cref="IAssert{Boolean}.Current"/> == <see cref="bool.TrueString"/>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<bool> True(this IAssert<bool> assert)
        {
            return assert.True($"{assert.Name},Does not meet the true condition");
        }

        /// <summary>
        /// <see cref="IAssert{Boolean}.Current"/> == <see cref="bool.TrueString"/>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
        public static IAssert<bool> True(this IAssert<bool> assert, string message)
        {
            if (!assert.Current)
            {
                throw new SAEException(StatusCodes.ParameterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region False

        /// <summary>
        /// <see cref="IAssert{Boolean}.Current"/> == <see cref="bool.FalseString"/>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<bool> False(this IAssert<bool> assert) => assert.False($"{assert.Name},False condition not met");
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/> == <see cref="bool.FalseString"/>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
        public static IAssert<bool> False(this IAssert<bool> assert, string message)
        {
            if (assert.Current)
            {
                throw new SAEException(StatusCodes.ParameterInvalid, message);
            }
            return assert;
        }
        #endregion

        #region Null
        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>为<em>null</em>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<TAssert> Null<TAssert>(this IAssert<TAssert> assert)
        {
            return assert.Null($"{assert.Name},not null");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>为<em>null</em>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
        public static IAssert<TAssert> Null<TAssert>(this IAssert<TAssert> assert, string message)
        {
            if (assert.Current != null)
            {
                throw new SAEException(StatusCodes.ParameterInvalid,message);
            }
            return assert;
        }

        #endregion

        #region NotNull

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>不为<em>null</em>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <returns>断言对象</returns>
        public static IAssert<TAssert> NotNull<TAssert>(this IAssert<TAssert> assert)
        {
            return assert.NotNull($"{assert.Name},Cannot be null");
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>不为<em>null</em>
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="message">错误信息</param>
        /// <returns>断言对象</returns>
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
        /// 传入一个委托继续进行断言
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="delegate">断言委托</param>
        /// <param name="name">参赛名</param>
        /// <typeparam name="TAssert">参与短语的类型</typeparam>
        /// <typeparam name="TThen"><paramref name="delegate"/>返回的类型</typeparam>
        /// <returns><see cref="IAssert{TThen}"/></returns>
        public static IAssert<TThen> Then<TAssert, TThen>(this IAssert<TAssert> assert, Func<TAssert, TThen> @delegate, string name = "")
        {
            return Assert.Build(@delegate(assert.Current), name);
        }
        #endregion

        #region Any

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>集合中是否有满足条件的项
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="predicate"></param>
        /// <param name="message"></param>
        /// <returns>断言对象</returns>
        public static IAssert<IEnumerable<TAssert>> Any<TAssert>(this IAssert<IEnumerable<TAssert>> assert, Func<TAssert, bool> predicate, string message = null)
        {
            assert.NotNull()
                  .Then(s => s.Any(predicate))
                  .True(message ?? "No match exists");
            return assert;
        }

        /// <summary>
        /// <see cref="IAssert{TAssert}.Current"/>集合中没有满足条件的项
        /// </summary>
        /// <param name="assert">断言对象</param>
        /// <param name="predicate"></param>
        /// <param name="message"></param>
        /// <returns>断言对象</returns>
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
