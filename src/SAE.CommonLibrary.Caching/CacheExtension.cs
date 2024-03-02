using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Caching
{
    /// <summary>
    /// <see cref="ICache"/>扩展
    /// </summary>
    public static class CacheExtension
    {


        /// <summary>
        /// 根据<paramref name="description"/>添加缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, CacheDescription<T> description)
        {
            return cache.AddAsync(description)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, T value)
        {
            return cache.Add(new CacheDescription<T>(key, value));
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, T value, DateTime dateTime)
        {
            return cache.Add(new CacheDescription<T>(key, value) { AbsoluteExpiration = dateTime });
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, T value, TimeSpan timeSpan)
        {
            return cache.Add(new CacheDescription<T>(key, value) { AbsoluteExpirationRelativeToNow = timeSpan });
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, T value, long second)
        {
            return cache.Add(key, value, TimeSpan.FromSeconds(second));
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="timeSpan"/>作为滚动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, TimeSpan timeSpan, T value)
        {
            return cache.Add(new CacheDescription<T>(key, value) { SlidingExpiration = timeSpan });
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="second"/>作为滚动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="second"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add<T>(this ICache cache, string key, long second, T value)
        {
            return cache.Add(key, TimeSpan.FromSeconds(second), value);
        }
        /// <summary>
        /// 通过<paramref name="descriptions"/>批量添加缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, IEnumerable<CacheDescription<T>> descriptions)
        {
            return cache.AddAsync(descriptions).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(pairs)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, IDictionary<string, T> pairs, DateTime dateTime)
        {
            return cache.AddAsync(pairs, dateTime)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, IDictionary<string, T> pairs, TimeSpan timeSpan)
        {
            return cache.AddAsync(pairs, timeSpan)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, IDictionary<string, T> pairs, long second)
        {
            return cache.AddAsync(pairs, second)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="timeSpan"/>作为滑动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="timeSpan"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, TimeSpan timeSpan, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(timeSpan, pairs)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="second"/>作为滑动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="second"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<T>(this ICache cache, long second, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(second, pairs)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, T value)
        {
            return cache.AddAsync(new CacheDescription<T>(key, value));
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, T value, DateTime dateTime)
        {
            return cache.AddAsync(new CacheDescription<T>(key, value) { AbsoluteExpiration = dateTime });
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, T value, TimeSpan timeSpan)
        {
            return cache.AddAsync(new CacheDescription<T>(key, value) { AbsoluteExpirationRelativeToNow = timeSpan });
        }

        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, T value, long second)
        {
            return cache.AddAsync(key, value, TimeSpan.FromSeconds(second));
        }

        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="timeSpan"/>作为滚动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, TimeSpan timeSpan, T value)
        {
            return cache.AddAsync(new CacheDescription<T>(key, value) { SlidingExpiration = timeSpan });
        }
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// 并使用<paramref name="second"/>作为滚动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="second"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync<T>(this ICache cache, string key, long second, T value)
        {
            return cache.AddAsync(key, value, TimeSpan.FromSeconds(second));
        }

        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription<T>(dic.Key, dic.Value)).ToArray());
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, IDictionary<string, T> pairs, DateTime dateTime)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription<T>(dic.Key, dic.Value)
            {
                AbsoluteExpiration = dateTime
            }).ToArray());
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, IDictionary<string, T> pairs, TimeSpan timeSpan)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription<T>(dic.Key, dic.Value)
            {
                AbsoluteExpirationRelativeToNow = timeSpan
            }).ToArray());
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, IDictionary<string, T> pairs, long second)
        {
            return cache.AddAsync(pairs, TimeSpan.FromSeconds(second));
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="timeSpan"/>作为滑动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="timeSpan"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, TimeSpan timeSpan, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription<T>(dic.Key, dic.Value)
            {
                SlidingExpiration = timeSpan
            }).ToArray());
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="second"/>作为滑动缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="second"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<T>(this ICache cache, long second, IDictionary<string, T> pairs)
        {
            return cache.AddAsync(TimeSpan.FromSeconds(second), pairs);
        }


        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="cache"></param>
        public static void Clear(this ICache cache)
        {
            cache.ClearAsync()
                 .GetAwaiter()
                 .GetResult();
        }

        /// <summary>
        /// 通过<paramref name="key"/>移除缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(this ICache cache, string key)
        {
            return cache.DeleteAsync(key).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 根据<paramref name="keys"/>批量移除缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Delete(this ICache cache, IEnumerable<string> keys)
        {
            return cache.DeleteAsync(keys).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 采用正则表达式的方式进行删除
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pattern">正则表达式</param>
        public static bool DeletePattern(this ICache cache, string pattern)
        {
            return cache.DeletePatternAsync(pattern).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 采用正则表达式的方式进行删除
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pattern">正则表达式</param>
        public static async Task<bool> DeletePatternAsync(this ICache cache, string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                return false;
            }

            var keys = await cache.GetKeysAsync();

            var regex = new Regex(pattern);

            keys = keys.Where(s => regex.IsMatch(s)).ToArray();

            if (keys.Any())
            {
                var bools = await cache.DeleteAsync(keys);

                return bools.All(s => s);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this ICache cache, string key)
        {
            return cache.GetAsync<T>(key).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 批量获得缓存<paramref name="keys"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="keys">缓存键集合</param>
        /// <returns></returns>
        public static IEnumerable<T> Get<T>(this ICache cache, IEnumerable<string> keys)
        {
            return cache.GetAsync<T>(keys).GetAwaiter().GetResult();
        }


        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }).GetAwaiter()
              .GetResult();
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, Func<T> valueFactory, DateTime dateTime)
        {
            return cache.GetOrAddAsync(key, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }, dateTime).GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, Func<T> valueFactory, TimeSpan timeSpan)
        {
            return cache.GetOrAddAsync(key, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }, timeSpan).GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, Func<T> valueFactory, long second)
        {
            return cache.GetOrAddAsync(key, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }, second).GetAwaiter()
                      .GetResult();
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="second"/>作为滑动缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="second"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, long second, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, second, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="timeSpan"/>作为滑动缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ICache cache, string key, TimeSpan timeSpan, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, timeSpan, () =>
            {
                return Task.FromResult(valueFactory.Invoke());
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this ICache cache, string key, Func<Task<T>> valueFactory)
        {
            T value;

            if (await cache.ExistAsync(key))
            {
                value = await cache.GetAsync<T>(key);
            }
            else
            {
                value = await valueFactory.Invoke();
                await cache.AddAsync(key, value);
            }

            return value;
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this ICache cache, string key, Func<Task<T>> valueFactory, DateTime dateTime)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = await valueFactory.Invoke();
                await cache.AddAsync(key, value, dateTime);
            }

            return value;
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="timeSpan"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this ICache cache, string key, Func<Task<T>> valueFactory, TimeSpan timeSpan)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = await valueFactory.Invoke();
                await cache.AddAsync(key, value, timeSpan);
            }

            return value;
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this ICache cache, string key, Func<Task<T>> valueFactory, long second)
        {
            return cache.GetOrAddAsync(key, valueFactory, TimeSpan.FromSeconds(second));
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="second"/>作为滑动缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="second"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static Task<T> GetOrAddAsync<T>(this ICache cache, string key, long second, Func<Task<T>> valueFactory)
        {
            return cache.GetOrAddAsync(key, TimeSpan.FromSeconds(second), valueFactory);
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="timeSpan"/>作为滑动缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="timeSpan"></param>
        /// <param name="valueFactory"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this ICache cache, string key, TimeSpan timeSpan, Func<Task<T>> valueFactory)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = await valueFactory.Invoke();
                await cache.AddAsync(key, timeSpan, value);
            }

            return value;
        }

    }
}
