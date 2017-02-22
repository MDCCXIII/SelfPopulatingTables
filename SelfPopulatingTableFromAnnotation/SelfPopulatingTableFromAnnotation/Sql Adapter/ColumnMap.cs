using System;
using System.Linq.Expressions;

namespace SelfPopulatingTableFromAnnotation.Sql_Adapter {
    [AttributeUsage(AttributeTargets.Field)]
    public class ColumnMap : Attribute {
        string ColumnName;
        object ColumnValue;
        public ColumnMap(string columnName) {
            ColumnName = columnName;
        }

        public virtual string Name {
            get { return ColumnName; }
        }

        public virtual object Value {
            get { return ColumnValue; }
            set { ColumnValue = value; }
        }
    }

    public static class ColumnMapExtensions {
        public static string DeclaredName(this string field) {
            return GetFieldName(() => field);
        }

        private static string GetFieldName<T>(Expression<Func<T>> expr) {
            var body = ((MemberExpression)expr.Body);
            return body.Member.Name;
        }
    }
}
