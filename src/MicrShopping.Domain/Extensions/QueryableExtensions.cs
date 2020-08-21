﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MicrShopping.Domain.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIF<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> WhereIF<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IEnumerable<T> WhereIF<T>(this IEnumerable<T> query, bool condition, Func<T, bool> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }
    }
}