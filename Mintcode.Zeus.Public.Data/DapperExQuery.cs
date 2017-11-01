using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.Zeus.Public.Data
{
    /// <summary>
    /// DapperEx查询方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperExQuery<T>
        where T : class
    {

        private List<DapperExSingleSqlQuery<T>> experList = new List<DapperExSingleSqlQuery<T>>();

        #region 查询操作符

        /// <summary>
        /// AndWhere查询
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DapperExQuery<T> AndWhere(Expression<Func<T, object>> expr, OperationMethod operation, object value)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                Expression = expr,
                Operation = operation,
                Value = value,
                SelectOperation = DapperExSingleSqlQuerySelect.And
            }
                );
            return this;
        }

        /// <summary>
        /// OrWhere查询
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="operation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DapperExQuery<T> OrWhere(Expression<Func<T, object>> expr, OperationMethod operation, object value)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                Expression = expr,
                Operation = operation,
                Value = value,
                SelectOperation = DapperExSingleSqlQuerySelect.Or
            });
            return this;
        }

        /// <summary>
        /// Order查询（暂时不用泛型查询）
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public DapperExQuery<T> Order(Expression<Func<T, object>> expr, bool isDesc)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                Expression = expr,
                desc = isDesc,
                SelectOperation = DapperExSingleSqlQuerySelect.Order
            });
            return this;
        }

        /// <summary>
        /// Top查询
        /// </summary>
        /// <param name="topNum"></param>
        /// <returns></returns>
        public DapperExQuery<T> Top(int topNum)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                TopNum = topNum,
                SelectOperation = DapperExSingleSqlQuerySelect.Top
            });
            return this;
        }

        /// <summary>
        /// 左括号(
        /// </summary>
        /// <param name="isAnd">true为AND false为OR</param>
        /// <returns></returns>
        public DapperExQuery<T> LeftInclude(bool isAnd = true)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                LeftAnd = isAnd,
                SelectOperation = DapperExSingleSqlQuerySelect.LeftInclude
            });
            return this;
        }

        /// <summary>
        /// 右括号)
        /// </summary>
        /// <returns></returns>
        public DapperExQuery<T> RightInclude()
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                SelectOperation = DapperExSingleSqlQuerySelect.RightInclude
            });
            return this;
        }


        /// <summary>
        ///  设置Order排序SQL语句
        /// </summary>
        /// <param name="orderString"></param>
        public void SetOrder(string orderString)
        {
            experList.Add(new DapperExSingleSqlQuery<T>()
            {
                OrderString = orderString,
                SelectOperation = DapperExSingleSqlQuerySelect.Order
            });
        }

        #endregion

        /// <summary>
        /// 获得SqlQuery查询条件
        /// </summary>
        /// <param name="dbBase"></param>
        /// <returns></returns>
        public SqlQuery GetSqlQuery(DbBase dbBase)
        {
            SqlQuery<T> query = SqlQuery<T>.Builder(dbBase);
            query.SetWhere(experList);
            return query;
        }

        /// <summary>
        /// 转化为InWhere能够查询的对象
        /// </summary>
        /// <param name="sqlquery"></param>
        /// <returns></returns>
        public InWhereQuery TranInWhereQuery(string selectField, SqlQuery sqlquery)
        {
            //解析
            return new InWhereQuery()
            {
                SelectStr = sqlquery.QuerySql,
                Param = (List<DynamicPropertyModel>)sqlquery._Param,
                ParamValues = (Dictionary<string, object>)sqlquery.ParamValues,
                SelectField = selectField
            };
        }


    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperExSingleSqlQuery<T>
       where T : class
    {
        /// <summary>
        /// 选择字段（用于Where和Order查询)
        /// </summary>
        public Expression<Func<T, object>> Expression { get; set; }

        /// <summary>
        /// 操作符(用于Where查询)
        /// </summary>
        public OperationMethod Operation { get; set; }

        /// <summary>
        /// 值 (用于Where查询）
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Left的操作符And,Or(用于Left操作符)
        /// </summary>
        public bool LeftAnd { get; set; }

        /// <summary>
        /// Top操作的数量（用于Top操作符)
        /// </summary>
        public int TopNum { get; set; }

        /// <summary>
        /// Order操作的排序（用于Order操作符）
        /// </summary>
        public bool desc { get; set; }

        /// <summary>
        /// 多个条件查询之间的操作符类型
        /// </summary>
        public DapperExSingleSqlQuerySelect SelectOperation { get; set; }

        /// <summary>
        /// 排序SQL字符串
        /// </summary>
        public string OrderString { get; set; }


    }

    /// <summary>
    /// 多个条件查询之间的操作符
    /// </summary>
    public enum DapperExSingleSqlQuerySelect
    {
        //And操作符
        And = 1,
        //Or操作符
        Or = 2,
        //Order的操作符
        Order = 3,
        //Left的(操作符
        LeftInclude = 4,
        //Right的)操作符
        RightInclude = 5,
        //Top的操作符
        Top = 6,
        //InWhere的操作符
        InWhere = 7

    }

    /// <summary>
    /// 子查询操作对象
    /// </summary>
    public class InWhereQuery
    {
        public string SelectStr { get; set; }

        public List<DynamicPropertyModel> Param { get; set; }

        public Dictionary<string, object> ParamValues { get; set; }

        public string SelectField { get; set; }
    }
}
