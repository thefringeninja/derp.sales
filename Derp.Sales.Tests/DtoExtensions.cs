using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Derp.Sales.Tests
{
    public static class DtoExtensions
    {
        public static string ToApplicationFormUrlEncoded<T>(this T dto)
        {
            var builder = new StringBuilder();
            return builder.Append(
                String.Join(
                    "&",
                    InputModel(dto).Select(input => input.Item1.Underscore().Dasherize() + "=" + input.Item2)))
                          .ToString();
        }

        private static IEnumerable<Tuple<string, object>> InputModel<T>(T command)
        {
            var members = (from field in command.GetType().GetFields()
                           select new
                           {
                               field.Name,
                               Type = field.FieldType,
                               GetValue = new Func<T, object>(c => field.GetValue(c))
                           }).Union(
                               from property in command.GetType().GetProperties()
                               select new
                               {
                                   property.Name,
                                   Type = property.PropertyType,
                                   GetValue = new Func<T, object>(c => property.GetValue(c))
                               });
            var formFields = from member in members
                             let values = SelectManyIfEnumerable(member.GetValue(command))
                             from value in values
                             select Tuple.Create(
                                 member.Name,
                                 value);
            return formFields;
        }

        private static IEnumerable<object> SelectManyIfEnumerable(object value)
        {
            if (value is string)
            {
                yield return value;
                yield break;
            }

            var items = value as IEnumerable;
            if (items == null)
            {
                yield return value;
                yield break;
            }

            foreach (var item in items)
                yield return item;
        }
    }
}