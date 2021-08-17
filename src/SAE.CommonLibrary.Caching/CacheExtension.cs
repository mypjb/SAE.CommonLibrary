using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAE.CommonLibrary.Extension;

namespace SAE.CommonLibrary.Caching
{
    public static class CacheExtension
    {
        #region Add

        #region Sync
        /// <summary>
        /// 根据<paramref name="description"/>添加缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static bool Add(this ICache cache, CacheDescription description)
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
        public static bool Add(this ICache cache, string key, object value)
        {
            return cache.Add(new CacheDescription(key, value));
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
        public static bool Add(this ICache cache, string key, object value, DateTime dateTime)
        {
            return cache.Add(new CacheDescription(key, value) { AbsoluteExpiration = dateTime });
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
        public static bool Add(this ICache cache, string key, object value, TimeSpan timeSpan)
        {
            return cache.Add(new CacheDescription(key, value) { AbsoluteExpirationRelativeToNow = timeSpan });
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
        public static bool Add(this ICache cache, string key, object value, long second)
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
        public static bool Add(this ICache cache, string key, TimeSpan timeSpan, object value)
        {
            return cache.Add(new CacheDescription(key, value) { SlidingExpiration = timeSpan });
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
        public static bool Add(this ICache cache, string key, long second, object value)
        {
            return cache.Add(key, TimeSpan.FromSeconds(second), value);
        }
        /// <summary>
        /// 通过<paramref name="descriptions"/>批量添加缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="descriptions"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add(this ICache cache, IEnumerable<CacheDescription> descriptions)
        {
            return cache.AddAsync(descriptions).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Add<TValue>(this ICache cache, IDictionary<string, TValue> pairs)
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
        public static IEnumerable<bool> Add<TValue>(this ICache cache, IDictionary<string, TValue> pairs, DateTime dateTime)
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
        public static IEnumerable<bool> Add<TValue>(this ICache cache, IDictionary<string, TValue> pairs, TimeSpan timeSpan)
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
        public static IEnumerable<bool> Add<TValue>(this ICache cache, IDictionary<string, TValue> pairs, long second)
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
        public static IEnumerable<bool> Add<TValue>(this ICache cache, TimeSpan timeSpan, IDictionary<string, TValue> pairs)
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
        public static IEnumerable<bool> Add<TValue>(this ICache cache, long second, IDictionary<string, TValue> pairs)
        {
            return cache.AddAsync(second, pairs)
                        .GetAwaiter()
                        .GetResult();
        }
        #endregion

        #region Async
        /// <summary>
        /// 添加<paramref name="key"/>的缓存<paramref name="value"/>
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Task<bool> AddAsync(this ICache cache, string key, object value)
        {
            return cache.AddAsync(new CacheDescription(key, value));
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
        public static Task<bool> AddAsync(this ICache cache, string key, object value, DateTime dateTime)
        {
            return cache.AddAsync(new CacheDescription(key, value) { AbsoluteExpiration = dateTime });
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
        public static Task<bool> AddAsync(this ICache cache, string key, object value, TimeSpan timeSpan)
        {
            return cache.AddAsync(new CacheDescription(key, value) { AbsoluteExpirationRelativeToNow = timeSpan });
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
        public static Task<bool> AddAsync(this ICache cache, string key, object value, long second)
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
        public static Task<bool> AddAsync(this ICache cache, string key, TimeSpan timeSpan, object value)
        {
            return cache.AddAsync(new CacheDescription(key, value) { SlidingExpiration = timeSpan });
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
        public static Task<bool> AddAsync(this ICache cache, string key, long second, object value)
        {
            return cache.AddAsync(key, value, TimeSpan.FromSeconds(second));
        }

        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, IDictionary<string, TValue> pairs)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription(dic.Key, dic.Value)).ToArray());
        }
        /// <summary>
        /// 使用<paramref name="pairs"/>作为缓存键值对，进行缓存添加
        /// 并以<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="pairs"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, IDictionary<string, TValue> pairs, DateTime dateTime)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription(dic.Key, dic.Value)
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
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, IDictionary<string, TValue> pairs, TimeSpan timeSpan)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription(dic.Key, dic.Value)
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
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, IDictionary<string, TValue> pairs, long second)
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
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, TimeSpan timeSpan, IDictionary<string, TValue> pairs)
        {
            return cache.AddAsync(pairs.Select(dic => new CacheDescription(dic.Key, dic.Value)
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
        public static Task<IEnumerable<bool>> AddAsync<TValue>(this ICache cache, long second, IDictionary<string, TValue> pairs)
        {
            return cache.AddAsync(TimeSpan.FromSeconds(second), pairs);
        }
        #endregion


        #endregion

        #region Remove
        /// <summary>
        /// 通过<paramref name="key"/>移除缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(this ICache cache, string key)
        {
            return cache.RemoveAsync(key).GetAwaiter().GetResult();
        }
        /// <summary>
        /// 根据<paramref name="keys"/>批量移除缓存
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static IEnumerable<bool> Remove(this ICache cache, IEnumerable<string> keys)
        {
            return cache.RemoveAsync(keys).GetAwaiter().GetResult();
        }

        #endregion

        #region IDistributedCache Get

        #region Sync
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDistributedCache cache, string key)
        {
            return cache.GetAsync<T>(key).GetAwaiter().GetResult();
        }

        public static IEnumerable<T> Get<T>(this IDistributedCache cache, IEnumerable<string> keys)
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, valueFactory)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<T> valueFactory, DateTime dateTime)
        {
            return cache.GetOrAddAsync(key, valueFactory, dateTime)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<T> valueFactory, TimeSpan timeSpan)
        {
            return cache.GetOrAddAsync(key, valueFactory, timeSpan)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, Func<T> valueFactory, long second)
        {
            return cache.GetOrAddAsync(key, valueFactory, second)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, long second, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, second, valueFactory).GetAwaiter().GetResult();
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
        public static T GetOrAdd<T>(this IDistributedCache cache, string key, TimeSpan timeSpan, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, timeSpan, valueFactory).GetAwaiter().GetResult();
        }
        #endregion

        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IDistributedCache cache, string key)
        {
            var json = (await cache.GetAsync(key))?.ToString();
            if (json.IsNullOrWhiteSpace()) return default(T);
            return json.ToObject<T>();
        }

        public static async Task<IEnumerable<T>> GetAsync<T>(this IDistributedCache cache, IEnumerable<string> keys)
        {
            List<T> ts = new List<T>();
            var results = await cache.GetAsync(keys);
            foreach (object result in results)
            {
                if (string.IsNullOrWhiteSpace(result?.ToString()))
                {
                    ts.Add(default);
                }
                else
                {
                    ts.Add(result.ToString().ToObject<T>());
                }
            }
            return ts;
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
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> valueFactory)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
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
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> valueFactory, DateTime dateTime)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
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
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> valueFactory, TimeSpan timeSpan)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
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
        public static Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, Func<T> valueFactory, long second)
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
        public static Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, long second, Func<T> valueFactory)
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
        public static async Task<T> GetOrAddAsync<T>(this IDistributedCache cache, string key, TimeSpan timeSpan, Func<T> valueFactory)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
                await cache.AddAsync(key, timeSpan, value);
            }

            return value;
        }


        #endregion

        #region IMemoryCache Get
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this IMemoryCache cache, string key)
        {
            return (T)await cache.GetAsync(key);
        }
        /// <summary>
        /// 根据<paramref name="key"/>获得缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IMemoryCache cache, string key)
        {
            return cache.GetAsync<T>(key).GetAwaiter().GetResult();
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
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, valueFactory)
                        .GetAwaiter()
                        .GetResult();
        }
        /// <summary>
        /// /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, Func<T> valueFactory, DateTime dateTime)
        {
            return cache.GetOrAddAsync(key, valueFactory, dateTime)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, Func<T> valueFactory, TimeSpan timeSpan)
        {
            return cache.GetOrAddAsync(key, valueFactory, timeSpan)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, Func<T> valueFactory, long second)
        {
            return cache.GetOrAddAsync(key, valueFactory, second)
                        .GetAwaiter()
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
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, long second, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, second, valueFactory).GetAwaiter().GetResult();
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
        public static T GetOrAdd<T>(this IMemoryCache cache, string key, TimeSpan timeSpan, Func<T> valueFactory)
        {
            return cache.GetOrAddAsync(key, timeSpan, valueFactory).GetAwaiter().GetResult();
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
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, Func<T> valueFactory)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
                await cache.AddAsync(key, value);
            }

            return value;
        }
        /// <summary>
        /// /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="dateTime"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, Func<T> valueFactory, DateTime dateTime)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
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
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, Func<T> valueFactory, TimeSpan timeSpan)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
                await cache.AddAsync(key, value, timeSpan);
            }

            return value;
        }
        /// <summary>
        /// /// 根据<paramref name="key"/>获得缓存,如果缓存不存在
        /// 则通过<paramref name="valueFactory"/>添加
        /// 并使用<paramref name="second"/>作为绝对缓存失效时间
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="valueFactory"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, Func<T> valueFactory, long second)
        {
            var value = await cache.GetAsync<T>(key);
            if (value == null)
            {
                value = valueFactory.Invoke();
                await cache.AddAsync(key, value, second);
            }

            return value;
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
        public static Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, long second, Func<T> valueFactory)
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
        public static async Task<T> GetOrAddAsync<T>(this IMemoryCache cache, string key, TimeSpan timeSpan, Func<T> valueFactory)
        {
            var value = await cache.GetAsync<T>(key);

            if (value == null)
            {
                value = valueFactory.Invoke();
                await cache.AddAsync(key, timeSpan, value);
            }

            return value;
        }

        #endregion
    }
}
