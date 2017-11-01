using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mintcode.Zeus.Public.Data
{
    /// <summary>
    /// DapperEx的ORM操作类
    /// </summary>
    public static class DapperEx
    {

        #region 增 删 改



        /// <summary>
        /// 插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Insert<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {

            try
            {
                #region 验证

                //DBValidateCommon.ValidateTargetModel(t);

                #endregion



                var db = dbs.DbConnecttion;
               
                SqlQuery sql = SqlQuery<T>.Builder(dbs);
                bool isAutoId = false;
                string insertSql = sql.InsertSql;

                ModelDes modelDes = Common.GetModelDes<T>();
                if (modelDes != null && modelDes.Properties != null)
                {
                    foreach (var item in modelDes.Properties)
                    {
                        if ((item.CusAttribute is IdAttribute) && ((item.CusAttribute as IdAttribute).CheckAutoId))
                        {
                            isAutoId = true;


                            break;
                        }
                    }
                }

                int flag = 0;

                if (t is ModelBase)
                {
                    ModelBase store = t as ModelBase;
                    sql = sql.AppendParam<T>(t).SetExcProperties<T>(store.Fields);
                }
                else
                {
                    sql = sql.AppendParam<T>(t);
                }

                if (isAutoId)
                {
                    insertSql += ";SELECT @@IDENTITY;";
                    var recordId = db.ExecuteScalar(insertSql, sql.Param, transaction, commandTimeout, null);
                    //flag = recordId.MConvertTo<int>();
                    flag = Convert.ToInt32(recordId);
                }
                else
                {
                    flag = db.Execute(insertSql, sql.Param, transaction, commandTimeout, null);
                }


                if (isAutoId)
                {
                    #region 如果主键是自增Id,则将生成的id赋值到实体类中

                    if (modelDes != null && modelDes.Properties != null)
                    {
                        foreach (var item in modelDes.Properties)
                        {
                            if ((item.CusAttribute is IdAttribute) && ((item.CusAttribute as IdAttribute).CheckAutoId))
                            {
                                var filed = t.GetType().GetProperty(item.Field);
                                filed.SetValue(t, Convert.ChangeType(flag, filed.PropertyType), null);

                                break;
                            }
                        }
                    }

                    #endregion

                    return true;
                }

                return flag > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
        }


        /// <summary>
        ///  批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool InsertBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
           
            try
            {
                #region 验证
                //foreach (var t in lt)
                //{
                //    DBValidateCommon.ValidateTargetModel(t);
                //}
                #endregion

                var db = dbs.DbConnecttion;
              
                var sql = SqlQuery<T>.Builder(dbs);
                var flag = db.Execute(sql.InsertSql, lt, transaction, commandTimeout, null);
                
                return flag == lt.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 按条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, SqlQuery sql = null, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
           
            try
            {
                var db = dbs.DbConnecttion;
                
                if (sql == null)
                {
                    sql = SqlQuery<T>.Builder(dbs);
                }
                var f = db.Execute(sql.DeleteSql, sql.Param, transaction, commandTimeout, null);
                
                return f > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 按照实体进行删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool Delete<T>(this DbBase dbs, T t, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
           
            try
            {
                var db = dbs.DbConnecttion;
              
                var sql = SqlQuery<T>.Builder(dbs);
                var f = db.Execute(sql.DeleteSql, t, transaction, commandTimeout, null);
                
                return f > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="lt"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool DeleteBatch<T>(this DbBase dbs, IList<T> lt, IDbTransaction transaction = null, int? commandTimeout = null) where T : class
        {
          
            try
            {
                var db = dbs.DbConnecttion;
               
                var sql = SqlQuery<T>.Builder(dbs);
                var flag = db.Execute(sql.DeleteSql, lt, transaction, commandTimeout, null);
               
                return flag == lt.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改数据（根据某几个字段进行修改）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="t">如果sql为null，则根据t的主键进行修改</param>
        /// <param name="sql">按条件修改</param>
        /// <returns></returns>
        public static bool Update<T>(this DbBase dbs, T t, IDbTransaction transaction = null, SqlQuery sql = null, int? commandTimeout = null) where T : class
        {
          
            try
            {
                #region 验证

                //DBValidateCommon.ValidateTargetModel(t);

                #endregion

                var db = dbs.DbConnecttion;
               
                if (sql == null)
                {
                    sql = SqlQuery<T>.Builder(dbs);
                }
                //获取查询内容
                if (t is ModelBase)
                {
                    ModelBase store = t as ModelBase;
                    sql = sql.AppendParam<T>(t).SetExcProperties<T>(store.Fields);
                }
                else
                {
                    sql = sql.AppendParam<T>(t);
                }
                var f = db.Execute(sql.UpdateSql, sql.Param, transaction, commandTimeout, null);
               
                return f > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 查询

        /// <summary>
        /// 获取默认一条数据，没有则为NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbBase dbs, SqlQuery sql) where T : class
        {
           
            var db = dbs.DbConnecttion;
            
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            sql = sql.Top(1);
            var result = db.Query<T>(sql.QuerySql, sql.Param, null, false, null, null);
            
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static List<T> PageData<T>(this DbBase dbs, int pageIndex, int pageSize, out long dataCount, SqlQuery sqlQuery = null) where T : class
        {
           
            var db = dbs.DbConnecttion;

            var result = new List<T>();
            dataCount = 0;
          
            if (sqlQuery == null)
            {
                sqlQuery = SqlQuery<T>.Builder(dbs);
            }
            sqlQuery = sqlQuery.Page(pageIndex, pageSize);
            var para = sqlQuery.Param;
            var cr = db.Query(sqlQuery.CountSql, para, null, false, null, null).SingleOrDefault();
            dataCount = (long)cr.DataCount;
           
            //pageCount = cr.DataCount % pageSize == 0 ? (int)cr.DataCount / pageSize : (int)cr.DataCount / pageSize + 1;//计算页数
            result = db.Query<T>(sqlQuery.PageSql, para, null, false, null, null).ToList();
            
            return result;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="dataCount"></param>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public static List<T> PageDataByRowNumber<T>(this DbBase dbs, int startRow, int endRow, out long dataCount, SqlQuery sqlQuery = null) where T : class
        {
           
            var db = dbs.DbConnecttion;

            var result = new List<T>();
            dataCount = 0;
           
            if (sqlQuery == null)
            {
                sqlQuery = SqlQuery<T>.Builder(dbs);
            }
            sqlQuery = sqlQuery.PageRowNumber(startRow, endRow);
            var para = sqlQuery.Param;
            var cr = db.Query(sqlQuery.CountSql, para, null, false, null, null).SingleOrDefault();
            dataCount = (long)cr.DataCount;
           
            //pageCount = cr.DataCount % pageSize == 0 ? (int)cr.DataCount / pageSize : (int)cr.DataCount / pageSize + 1;//计算页数
            result = db.Query<T>(sqlQuery.PageSql, para, null, false, null, null).ToList();
           
            return result;
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<T> QueryData<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
            
            var db = dbs.DbConnecttion;
           
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            var result = db.Query<T>(sql.QuerySql, sql.Param, null, false, null, null);
            
            return result.ToList();
        }

        /// <summary>
        /// 数据数量
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static long Count<T>(this DbBase dbs, SqlQuery sql = null) where T : class
        {
           
            var db = dbs.DbConnecttion;
           
            if (sql == null)
            {
                sql = SqlQuery<T>.Builder(dbs);
            }
            var cr = db.Query(sql.CountSql, sql.Param, null, false, null, null).SingleOrDefault();
            
            return (long)cr.DataCount;
        }

        #endregion

        #region 存储过程

        /// <summary>
        ///  执行存储过程
        /// 具体二次分装需要参考：http://blog.csdn.net/wang463584281/article/details/21244933
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="sotreName"></param>
        /// <param name="inParems"></param>
        /// <param name="outResult"></param>
        /// <returns></returns>
        public static int StoreProcedure(this DbBase dbs, string sotreName, DynamicParameters inParems, string outResult)
        {
            var db = dbs.DbConnecttion;
            db.Execute(sotreName, inParems, null, null, CommandType.StoredProcedure);
            return inParems.Get<int>(outResult);
        }
    
        /// <summary>
        /// 存储过程返回集合(待优化)
        /// 参考:https://github.com/StackExchange/dapper-dot-net
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbs"></param>
        /// <param name="storeName">存储过程名称</param>
        /// <param name="param">输入参数</param>
        /// <returns></returns>
        public static List<T> ExecuteStoredProcedureWithParms<T>(this DbBase dbs, string storeName, object param = null) where T : class
        {
           
            try
            {
                var db = dbs.DbConnecttion;

                var result = db.Query<T>(storeName, param, null, false, null, CommandType.StoredProcedure);
              
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 存储过程无返回(可获取 OutPut,ReturnValue)
        /// </summary>
        /// <param name="dbs"></param>
        /// <param name="storeName">存储过程名称</param>
        /// <param name="param">输入参数</param>
        public static void ExecuteStoredProcedure(this DbBase dbs, string storeName, object param = null)
        {
           
            try
            {
                var db = dbs.DbConnecttion;

                db.Execute(storeName, param, null, null, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
